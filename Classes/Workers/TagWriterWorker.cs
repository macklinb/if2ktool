using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shell;
using TagLib;

namespace if2ktool
{
    public static class TagWriterWorker
    {
        static BackgroundWorker worker;
        static Main mainForm;

        static TagWriterWorkerArgs args;
        static TagWriterWorkerArgs lastArgs;

        static bool isPaused;

        // Call  Reset() to pause, Set() to unpause
        // Don't actually do this in code though, use the Paused property instead to keep in sync with isPaused
        static ManualResetEvent manualResetEvent;

        // Dictionary of entries that occurred during the tagwriter
        static Dictionary<Entry, string> entryErrors;

        static TaskbarItemInfo taskbarItemInfo;

        public static bool InProgress
        {
            get { return worker.IsBusy; }
        }

        public static bool Paused
        {
            get { return isPaused; }
            set
            {
                isPaused = value;

                // Pause the worker
                if (value == true)
                    manualResetEvent.Reset();
                else if (value == false)
                    manualResetEvent.Set();
            }
        }

        public class TagWriterWorkerArgs
        {
            public EntryFilter filter;
            public bool filterNot;
            public bool skipDateAdded;
            public bool skipLastPlayed;
            public bool skipPlayCount;
            public bool skipRating;
            public bool removeTags;
            public IEnumerable<DataGridViewRow> rows;
        }

        // Static ctor
        static TagWriterWorker()
        {
            mainForm = (Main)Application.OpenForms["Main"];

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_Completed;

            Console.CancelKeyPress += Console_CancelKeyPress;

            manualResetEvent = new ManualResetEvent(false);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            StopWorker();
        }

        public static void StartWorker(TagWriterWorkerArgs args)
        {
            if (worker.IsBusy)
                Debug.LogError("Cannot start writing tags while already writing tags!");
            else
            {
                // Ensure the manualResetEvent is set (unpaused)
                Paused = false;

                mainForm.ShowProgress(true);
                worker.RunWorkerAsync(args);
            }
        }

        public static void StopWorker(bool force = false)
        {
            if (worker.IsBusy)
            {
                if (!force)
                {
                    // Stop processing while the MessageBox is showing
                    Paused = true;

                    // Ask the user if they want to cancel writing tags
                    if (MessageBox.Show("Do you want to cancel the current tag writing process?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        // Resume processing
                        Paused = false;

                        return;
                    }
                }

                // Cancel the worker
                worker.CancelAsync();

                // Un-pause worker (so that the execution continues to exit)
                Paused = false;
            }
        }

        // Called on the main thread when the worker has reported it's progress
        // This updates the form to select the row with the currentRowIndex, setting the progress bar value and the status label text
        private static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var args = (ProgressArgs)e.UserState;
            mainForm.SetRowSelection(args.currentRowIndex, true);
            mainForm.SetProgress(args.processed, args.count, args.timeMs);

            // Set the progress value on the taskbar icon
            TaskbarManager.Instance.SetProgressValue(args.processed, args.count);
        }

        // Called on the main thread when the worker is complete
        private static void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            // Hide the progress state for the taskbar icon
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            // Check to see if the worker was cancelled.
            // If we weren't already removing/reverting tags, ask the user if they want to revert the changes that were made
            if (e.Cancelled && lastArgs != null && !lastArgs.removeTags)
            {
                if (MessageBox.Show("Do you want to revert changes made to files, and erase the playback statistics tags that have already been written to files?", "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var args = lastArgs;
                    args.removeTags = true;

                    // Get subset of entries where the tags have already been written
                    args.rows = args.rows.Where(x => ((Entry)x.DataBoundItem).wroteTags);

                    // Start a new worker with the same args, but removeTags true
                    // Takes into account skip arguments too
                    StartWorker(args);

                    return;
                }
            }

            // If any errors occurred, show a dialogue containing the errors
            if (entryErrors != null && entryErrors.Count > 0)
            {
                new TagWriterErrors(entryErrors).Show();
            }
            
            args = lastArgs = null;
            Console.CursorVisible = true;
            mainForm.ShowProgress(false);
        }

        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (TagWriterWorkerArgs)e.Argument;

            // Assign args to static variable. This is mainly used for cancellation/reverting, to understand what files were targeted in the operation
            lastArgs = args;

            int processed = 0;
            int count = args.rows.Count();
            int rowIndex = 0;

            // Misc
            int success = 0;                // <- File was successfully written to
            int unsupportedFormat = 0;      // <- File was of an unsupported format (UnsupportedFormatException)
            int corruptFile = 0;            // <- File was corrupted (CorruptFileException)
            int ioError = 0;                // <- Error occured while loading the file (IOException)
            int unmapped = 0;               // <- Entry didn't have a file mapped to it
            int otherError = 0;             // <- Another exception occurred

            Debug.Log(string.Format("\nWriting tags for {0} files, using the following parameters:\nfilter:\t\t{1}\nskipDateAdded:\t{3}\nskipLastPlayed:\t{4}\nskipPlayCount:\t{5}\nskipRating:\t{6}\nremoveTags:\t{7}\n\nUse Ctrl-C to terminate", count, args.filter, args.filterNot, args.skipDateAdded, args.skipLastPlayed, args.skipPlayCount, args.skipRating, args.removeTags));

            // Exit here if we have nothing to process
            if (count == 0)
            {
                Debug.LogError("No entries to process!", true);
                return;
            }

            // Wait 3 seconds before starting
            if (Settings.Current.workerDelayStart)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(".");
                    System.Threading.Thread.Sleep(1000);
                }
            }

            TagLib.File file = null;
            Entry entry = null;

            // A dictionary of entries and errors that have occured 
            entryErrors = new Dictionary<Entry, string>();

            // Progress report counter, gets set to the sw.ElapsedMilliseconds at every report progress interval
            long progressReportCounter = 0;

            // Start a new stopwatch
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // Set the TaskbarProgressState
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

            Console.Write("\n\n");

            void ReportProgress()
            {
                // Report progress to main thread to update DataGridView selection
                worker.ReportProgress(0, new ProgressArgs()
                {
                    processed = processed,
                    count = count,
                    timeMs = sw.ElapsedMilliseconds,
                    currentRowIndex = rowIndex
                });
            }

            foreach (DataGridViewRow row in args.rows)
            {
                if (isPaused)
                {
                    Debug.Log("--- PAUSED ---");
                    sw.Stop();

                    // Set the taskbar ProgressState to Paused
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                    // Halt execution until unpaused (Reset)
                    manualResetEvent.WaitOne();

                    // Only restart the stopwatch and taskbar if the worker isn't being cancelled
                    if (!worker.CancellationPending)
                    {
                        Debug.Log("--- RESUMED ---");
                        sw.Start();

                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    }
                }

                // Check if we should cancel
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    Debug.LogError("--- CANCELLED ---");
                    return;
                }

                // Keep track of the row Index so UpdateProgress can use it
                rowIndex = row.Index;
                processed++;

                // Only report progress every <progressReportInterval> milliseconds
                if (Settings.Current.workerReportsProgress && 
                    sw.ElapsedMilliseconds - progressReportCounter > Settings.Current.workerReportProgressInterval)
                {
                    progressReportCounter = sw.ElapsedMilliseconds;
                    ReportProgress();
                }

                // Fetch the DataBoundItem from the DataGridViewRow
                entry = (Entry)row.DataBoundItem;

                Debug.Log("#" + processed + " - " + entry.fileName);

                // Check if the entry is mapped to a file
                if (entry.isMapped == false)
                {
                    string message = "Entry is not matched to file";
                    Debug.LogWarning(" -> " + message);
                    unmapped++;
                    entryErrors.Add(entry, message);
                    continue;
                }

                bool isRetry = false;

                Retry:
                
                // Create TagLib file representation, catch to see if the file is valid
                try
                {
                    file = TagLib.File.Create(entry.mappedFilePath, isRetry ? ReadStyle.Average : ReadStyle.PictureLazy);
                }
                catch (Exception ex)
                {
                    var type = ex.GetType();
                    string errorMessage = "";

                    if (type == typeof(UnsupportedFormatException))
                    {
                        errorMessage = "File format with extension \"" + Path.GetExtension(entry.mappedFilePath) + "\" is unsupported (" + ex.Message + ")";
                        unsupportedFormat++;
                    }
                    else if (type == typeof(CorruptFileException))
                    {
                        errorMessage = "File appears corrupt! " + ex.Message;
                        corruptFile++;
                    }
                    else if (type == typeof(IOException))
                    {
                        errorMessage = "An IO exception occured while loading the file! " + ex.Message;

                        // Get the locking processes (if any)
                        var lockingProcesses = FileLockUtility.GetProcessesLockingFile(entry.mappedFilePath);

                        // Log locking processes
                        if (lockingProcesses != null && lockingProcesses.Count > 0)
                            Debug.LogError("\tThe file appears to be locked by the process(es): " +
                                string.Join(", ", lockingProcesses.Select(p => p.ProcessName).ToArray()));

                        ioError++;
                    }
                    else
                    {
                        errorMessage = "An error occured while loading the file! " + ex.Message;
                        otherError++;
                    }
                    
                    Debug.LogError(" -> " + errorMessage);
                    entryErrors.Add(entry, errorMessage);
                    continue;
                }

                // If we're in remove move
                if (args.removeTags)
                {
                    if (!args.skipDateAdded)
                        RemoveCustomTag(file, Consts.TAG_DATE_ADDED);
                    if (!args.skipLastPlayed)
                        RemoveCustomTag(file, Consts.TAG_LAST_PLAYED);
                    if (!args.skipPlayCount)
                        RemoveCustomTag(file, Consts.TAG_PLAY_COUNT);
                    if (!args.skipRating)
                        RemoveRating(file);

                    entry.wroteTags = false;
                }

                // If we're in writing mode
                else
                {
                    // Date Added
                    if (!args.skipDateAdded && entry.dateAdded != DateTime.MinValue)
                    {
                        // Create LDAP/Windows File Time long
                        long dateAddedWinTime = entry.dateAdded.ToFileTime();

                        if (Settings.Current.fullLogging)
                            Debug.Log(string.Format("\tWriting {0}: {1}\n\t-> Converted to {3} (or {2})", Consts.PLIST_KEY_DATE_ADDED, entry.dateAdded.ToString("o", System.Globalization.CultureInfo.InvariantCulture), entry.dateAdded.ToString(), dateAddedWinTime));

                        // Write the tag
                        WriteCustomTag(file, Consts.TAG_DATE_ADDED, dateAddedWinTime.ToString());
                    }

                    // Last Played
                    if (!args.skipLastPlayed && entry.lastPlayed != DateTime.MinValue)
                    {
                        // Create LDAP/Windows File Time long
                        long lastPlayedWinTime = entry.lastPlayed.ToFileTime();

                        if (Settings.Current.fullLogging)
                            Debug.Log(string.Format("\tWriting {0}: {1}\n\t-> Converted to {3} (or {2})", Consts.PLIST_KEY_LAST_PLAYED, entry.lastPlayed.ToString("o", System.Globalization.CultureInfo.InvariantCulture), entry.lastPlayed.ToString(), lastPlayedWinTime));

                        // Write the tag
                        WriteCustomTag(file, Consts.TAG_LAST_PLAYED, lastPlayedWinTime.ToString());
                    }

                    // Play Count
                    if (!args.skipPlayCount && entry.playCount > 0)
                    {
                        if (Settings.Current.fullLogging)
                            Debug.Log(string.Format("\tWriting {0}: {1}", Consts.PLIST_KEY_PLAY_COUNT, entry.playCount));

                        // Write the tag
                        WriteCustomTag(file, Consts.TAG_PLAY_COUNT, entry.playCount.ToString());
                    }

                    // Rating
                    if (!args.skipRating && entry.rating != Rating.Unrated)
                    {
                        int ratingInStars = (int)entry.rating;

                        if (Settings.Current.fullLogging)
                            Debug.Log(string.Format("\tWriting {0}: {1} ({2} stars)", Consts.PLIST_KEY_RATING, ratingInStars * 20, ratingInStars));

                        // Write the tag
                        WriteRating(file, ratingInStars);
                    }

                    entry.wroteTags = true;
                }

                // Write the modified tags to the file
                try
                {
                    if (Settings.Current.dontAddID3v1 || Settings.Current.removeID3v1)
                    {
                        // Remove the ID3v1 tag from the tags-to-write if dontAddID3v1 is enabled or if removeID3v1 is enabled 
                        // This is needed as TagLib by default creates an ID3v1 tag for writing
                        // (but only if the tags on disk didn't originally have them - as to preserve the original tags)
                        // (or if we're forcibly removing the ID3v1 tags from every file)
                        if (Settings.Current.removeID3v1 || file.TagTypesOnDisk.HasFlag(TagTypes.Id3v1) == false)
                            file.RemoveTags(TagTypes.Id3v1);
                    }

                    file.Save();

                    // If we're here, it means the TagLib.File was saved correctly
                    success++;

                    if (isRetry)
                    {
                        Debug.LogSuccess("\tReattempt succeeded");
                    }
                }
                catch (IOException ioex)
                {
                    // If this was a retry...
                    if (isRetry)
                    {
                        string message = "Could not save file after retry. An exception occurred: " + ioex.Message;
                        Debug.LogError("\t" + message);
                        ioError++;

                        entryErrors.Add(entry, message);
                    }
                    else
                    {
                        Debug.LogError("\t-> An IO exception occured while saving tags to the file \"" + entry.fileName + "\"");

                        // Get the locking processes (if any)
                        var lockingProcesses = FileLockUtility.GetProcessesLockingFile(entry.mappedFilePath);

                        // Print the process(es) that are locking the file
                        if (lockingProcesses != null && lockingProcesses.Count > 0)
                        {
                            string message = "The file appears to be locked by the process: " +
                                string.Join(", ", lockingProcesses.Select(p => p.ProcessName).ToArray());
                            Debug.LogError("\t" + message);
                            ioError++;

                            entryErrors.Add(entry, message);
                        }

                        // Otherwise if there is no locking process, this is a known issue in TagLib while using ReadStyle.PictureLazy.
                        // Reattempt, using the default ReadStyle.Average
                        else
                        {
                            Debug.LogError("\t-> Reattempting load with different parameters...");
                            isRetry = true;
                            goto Retry;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = "An error occurred while saving tags to \"" + entry.fileName + "\": (" + ex.Message + ")";
                    otherError++;
                    entryErrors.Add(entry, message);
                }

                file.Dispose();
            }

            sw.Stop();

            // Do final ReportProgress
            ReportProgress();

            string resultStr = string.Format("Took {0}ms ({1})\n {2} files were written to successfully\n {3} files could not be written to\n  {4} unsupported format\n  {5} corrupt\n  {6} IO errors\n  {7} unmapped\n  {8} other errors occurred", sw.ElapsedMilliseconds, TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).ToString("m\\:ss"), success, unsupportedFormat + corruptFile + ioError + unmapped + otherError, unsupportedFormat, corruptFile, ioError, unmapped, otherError);

            Debug.Log("Done! " + resultStr);
            MessageBox.Show(resultStr, "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (args.removeTags)
                Debug.Log("\n" + Consts.REMOVE_TAGS_FINISHED);
            else
                Debug.Log("\n" + Consts.WRITE_TAGS_FINISHED);
        }

        // --- TagLib Methods ---

        // Write custom, non-standard key-value pair to tags
        public static void WriteCustomTag(TagLib.File file, string key, string value)
        {
            // Write ID3v2 TXXX Frame for MP3/AAC/AIFF
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                var textframe = TagLib.Id3v2.UserTextInformationFrame.Get(tag, key, StringType.UTF16, true);
                textframe.Text = new string[] { value };

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote ID3V2 TXXX Frame: key=\"" + key + "\", value=\"" + value + "\"");
            }

            // Write AppleTag AppleAnnotationBox for ALAC/MPEG4
            else if (file.TagTypes.HasFlag(TagTypes.Apple))
            {
                var tag = (TagLib.Mpeg4.AppleTag)file.GetTag(TagTypes.Apple, false);

                // Check to see if we don't already have a DashBox with this key, and if we do - remove it (this format allows multiple DashBoxes with the same key)
                if (tag.GetDashBox(Consts.APPLE_TAG_MEANSTRING, key) != null)
                    RemoveCustomTag(file, key);

                // AppleAdditionalInfoBoxes prepend the data with 4 null characters for some reason, so hey lets do the same! I'm sure it's for a good cause
                tag.SetDashBox(Consts.APPLE_TAG_PREFIX + Consts.APPLE_TAG_MEANSTRING,
                               Consts.APPLE_TAG_PREFIX + key,
                               value.ToString());

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote AppleAnnotationBox tag: key=\"" + key + "\", value=\"" + value + "\"");
            }

            // Write XiphComment field for FLAC
            else if (file.TagTypes.HasFlag(TagTypes.FlacMetadata))
            {
                var tag = (TagLib.Ogg.XiphComment)file.GetTag(TagTypes.Xiph);
                tag.SetField(key, new string[] { value });

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote Ogg.XiphComment: key=\"" + key + "\", value=\"" + value + "\"");
            }

            else
            {
                Debug.LogWarning("\t-> Could not write data. Unsupported or missing tag type!");
            }
        }

        // Removes a custom tag with <key> from <file>. Returns true if a tag was found and removed
        public static bool RemoveCustomTag(TagLib.File file, string key)
        {
            // Remove ID3v2 TXXX Frame for MP3/AAC/AIFF
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                var textFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, key, StringType.UTF16, false);

                if (textFrame != null)
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed ID3V2 TXXX Frame with the key \"" + key + "\"");

                    tag.RemoveFrame(textFrame);
                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An ID3V2 TXXX Frame with the key \"" + key + "\" was not present");

                    return false;
                }
            }

            // Remove AppleTag AppleAnnotationBox for ALAC/MPEG4
            else if (file.TagTypes.HasFlag(TagTypes.Apple))
            {
                var tag = (TagLib.Mpeg4.AppleTag)file.GetTag(TagTypes.Apple, false);

                // Only remove if there is something to remove
                if (tag.GetDashBox(Consts.APPLE_TAG_MEANSTRING, key) != null)
                {
                    // We have to loop until the DashBox is no longer present, since MP4 tags support multiple
                    // tagboxes with the same key, and the user may have written to the file more than once
                    while (tag.GetDashBox(Consts.APPLE_TAG_MEANSTRING, key) != null)
                    {
                        if (Settings.Current.fullLogging)
                            Debug.Log("\t-> Removed AppleAnnotationBox with the key \"" + key + "\"");

                        // Setting the DashBox to an empty string will remove it. For some reason we don't need to include 4 null characters when removing
                        tag.SetDashBox(Consts.APPLE_TAG_MEANSTRING, key, string.Empty);
                    }

                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An AppleAnnotationBox with the key \"" + key + "\" was not present");

                    return false;
                }
            }

            // Write XiphComment field for FLAC
            else if (file.TagTypes.HasFlag(TagTypes.FlacMetadata))
            {
                var tag = (TagLib.Ogg.XiphComment)file.GetTag(TagTypes.Xiph);

                if (tag.GetField(key) != null)
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed XiphComment with the key \"" + key + "\"");

                    tag.RemoveField(key);
                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> A XiphComment with the key \"" + key + "\" was not present");

                    return false;
                }
            }
            else
            {
                Debug.LogWarning("\t-> Could not remove data. Unsupported or missing tag type!");
            }

            return false;
        }

        // Write rating to tags using Rating enum
        public static void WriteRating(TagLib.File file, Rating rating)
        {
            // Do not write if rating is unrated
            if (rating == Rating.Unrated)
            {
                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Rating not written, since the entry was unrated");

                return;
            }
            else
                WriteRating(file, (int)rating);
        }

        // Write rating to tags using a star rating
        public static void WriteRating(TagLib.File file, int starRating)
        {
            // Write Id3v2 POPM frame
            // See https://en.wikipedia.org/wiki/ID3#ID3v2_rating_tag_issue
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                var popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(tag, Consts.ID3_POPM_USER, true);

                switch (starRating)
                {
                    case 1: popmFrame.Rating = Consts.ID3_POPM_RATING_1; break;
                    case 2: popmFrame.Rating = Consts.ID3_POPM_RATING_2; break;
                    case 3: popmFrame.Rating = Consts.ID3_POPM_RATING_3; break;
                    case 4: popmFrame.Rating = Consts.ID3_POPM_RATING_4; break;
                    case 5: popmFrame.Rating = Consts.ID3_POPM_RATING_5; break;
                    default:
                    {
                        Debug.LogWarning("\t-> Invalid rating of " + starRating + " (" + (starRating * 20) + "). Skipping write...");
                        popmFrame = null;
                        break;
                    }
                }

                if (Settings.Current.fullLogging && popmFrame != null)
                    Debug.Log("\t-> Wrote ID3V2 Popularimeter (POMP) frame: value: " + popmFrame.Rating + " (" + starRating + " stars)");
            }

            // Apple uses a regular text field, with a 1-5 value
            // FLAC uses a regular vorbis comment, with a 1-5 value
            else if (file.TagTypes.HasFlag(TagTypes.Apple) || file.TagTypes.HasFlag(TagTypes.Xiph))
            {
                WriteCustomTag(file, Consts.TAG_RATING, starRating.ToString());
            }
        }

        public static void RemoveRating(TagLib.File file)
        {
            // Remove Id3v2 POPM frame
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                var popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(tag, Consts.ID3_POPM_USER, false);

                if (popmFrame != null)
                {
                    tag.RemoveFrame(popmFrame);

                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed ID3V2 Popularimeter (POMP) frame");
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An ID3V2 Popularimeter (POMP) frame was not present, so it was not removed");
                }
            }

            // For all other fields, use custom rating field
            else if (file.TagTypes.HasFlag(TagTypes.Apple) || file.TagTypes.HasFlag(TagTypes.Xiph))
            {
                RemoveCustomTag(file, Consts.TAG_RATING);
            }
        }
    }
}
