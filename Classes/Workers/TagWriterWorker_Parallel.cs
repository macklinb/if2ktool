using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;

namespace if2ktool
{
    // This is an experimental version of TagWriterWorker that writes tags in a ParallelFor.
    // This improves performance on the first iteration of files, especially with more threads. For subsequent tasks, the processing will be (in some cases) 400% faster due to disk caching, and the effect is inverted - with more threads degrading the performance. Writing tags in parallel thrashes the disk IO, and so the performance depends on your disks random IO speeds.

    /*
    First process:
	    No limit: 12:16
	    4 threads max: 12:53
	    1 thread max: 15:09
	    No parallel: 15:16
	
    Subsequent processes (files cached):
	    No limit: Average time = 3:16
	    4 threads max: Average time = 2:52
	    1 thread max: Average time = 2:50
	    No parallel: Average time = 2:44
    */
    
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
        static ConcurrentDictionary<Entry, string> entryErrors;

        public static bool InProgress
        {
            get { return worker.IsBusy; }
        }

		public static bool Paused
        {
			get { return isPaused;  }
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

        public enum TagWriterReturnCode : byte
        {
            Success = 0,
            Unmapped = 1,
            UnsupportedFormat = 2,
            CorruptFile = 3,
            IOEError = 4,
            OtherError = 5,
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

            if (Settings.Current.workerScrollWithSelection)
                mainForm.SetRowSelection(args.currentRowIndex, true);

            mainForm.SetProgress(args.processed, args.count, args.timeMs);

            // Set the progress value on the taskbar icon
            TaskbarManager.Instance.SetProgressValue(args.processed, args.count);
        }

        // Called on the main thread when the worker is complete, or is cancelled
        private static void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            // Hide the progress state for the taskbar icon
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            // Reset the "Processed" flag on all rows
            foreach (var entry in args.rows.Select(x => x.DataBoundItem).Cast<Entry>().Where(x => x.processed))
                entry.processed = false;

            if (e.Cancelled) Debug.Log("--- CANCELLED ---");

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
            args = (TagWriterWorkerArgs)e.Argument;

            // Assign args to static variable. This is mainly used for cancellation/reverting, to understand what files were targeted in the operation
            lastArgs = args;

            int processed = 0;
            int count = args.rows.Count();
            int rowIndex = 0;

            // Misc
            int success = 0;                // <- File was successfully written to
            int unmapped = 0;               // <- Entry didn't have a file mapped to it
            int unsupportedFormat = 0;      // <- File was of an unsupported format (UnsupportedFormatException)
            int corruptFile = 0;            // <- File was corrupted (CorruptFileException)
            int ioError = 0;                // <- Error occured while loading the file (IOException)
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

            // A dictionary of entries and errors that have occured 
            entryErrors = new ConcurrentDictionary<Entry, string>();

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
                    currentRowIndex = rowIndex,
                });
            }
			
            int lowestBreakIndex = 0;

            // Set up the ParallelOptions
            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = Settings.Current.maxParallelThreads };

        Resume:

            // Split the work into multiple parallel threads
            ParallelLoopResult parallelLoopResult = Parallel.ForEach(args.rows.Skip(lowestBreakIndex), parallelOptions, (row, parallelLoopState) =>
            {
                // Firstly, do some processing stuffs that can only be done in the worker

                // Check if we should pause
                if (isPaused)
                {
                    // Breaking allows current parallel iterations to complete before stopping
                    parallelLoopState.Break();
                    return;
                }

                // Check if we should cancel
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    parallelLoopState.Stop();
                    return;
                }

                // Check if we should report progress
                // Only report progress every <progressReportInterval> milliseconds
                if (Settings.Current.workerReportsProgress && sw.ElapsedMilliseconds - progressReportCounter > Settings.Current.workerReportProgressInterval)
                {
                    rowIndex = row.Index;
                    progressReportCounter = sw.ElapsedMilliseconds;
                    ReportProgress();
                }

                // Fetch the DataBoundItem from the DataGridViewRow
                var entry = (Entry)row.DataBoundItem;

                // Check if the entry hasn't already been processed in this session
                // This may be the case if we paused previously
                if (!entry.processed)
                {
                    // Create a new DebugStack
                    // We're unable to log from a parallel for, as the logs would be out of sync - and wouldn't be clustered together
                    // So instead of logging normally, logging within a Parallel is done to a DebugStack, which essentially just collects the logs so that we can fire them all at once
                    // This is done here instead of in WriteTagsForRow, as that method has many exit points, therefore we would have to return a debugstack object from it anyway
                    var ds = new DebugStack();

                    // Write the tags for this Entry
                    var result = WriteTagsForRow(entry, ++processed, ds);

                    // Increment the error/success counter
                    switch (result)
                    {
                        case TagWriterReturnCode.Success: success++; break;
                        case TagWriterReturnCode.Unmapped: unmapped++; break;
                        case TagWriterReturnCode.UnsupportedFormat: unsupportedFormat++; break;
                        case TagWriterReturnCode.CorruptFile: corruptFile++; break;
                        case TagWriterReturnCode.IOEError: ioError++; break;
                        case TagWriterReturnCode.OtherError: otherError++; break;
                    }

                    // Set the processed flag on this entry to true
                    entry.processed = true;

                    // Log the DebugStack
                    ds.Push();
                }
            });

            // If the Parallel.ForEach was paused, isPaused will be set
            if (isPaused)
            {
                // Save the index of the lowest last item processed
                lowestBreakIndex = (int)parallelLoopResult.LowestBreakIteration;

                Debug.Log("--- PAUSED ---");
                sw.Stop();

                // Set the taskbar ProgressState to Paused
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                // Halt execution until unpaused (Reset)
                manualResetEvent.WaitOne();

				// If we're here, the operation was unpaused

				// Handle cancelling while paused
				if (worker.CancellationPending || e.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

				// Otherwise resume
				else
                {
                    Debug.Log("--- RESUMED ---");
                    sw.Start();

                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

                    // Jump back up to Resume
                    goto Resume;
                }
            }

            // Return if we cancelled
            if (e.Cancel)
                return;

            sw.Stop();
			
            // Do final ReportProgress
            rowIndex = args.rows.Last().Index;
            ReportProgress();

            string resultStr = string.Format("Took {0}ms ({1})\n {2} files were written to successfully\n {3} files could not be written to\n  {4} unsupported format\n  {5} corrupt\n  {6} IO errors\n  {7} unmapped\n  {8} other errors occurred", sw.ElapsedMilliseconds, TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).ToString("m\\:ss"), success, unsupportedFormat + corruptFile + ioError + unmapped + otherError, unsupportedFormat, corruptFile, ioError, unmapped, otherError);

            Debug.Log("Done! " + resultStr);

            mainForm.Invoke(() => mainForm.Flash(false));
            System.Media.SystemSounds.Exclamation.Play();

            MessageBox.Show(resultStr, "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (args.removeTags)
                Debug.Log("\n" + Consts.REMOVE_TAGS_FINISHED);
            else
                Debug.Log("\n" + Consts.WRITE_TAGS_FINISHED);
        }

        // --- Main Process Method ---
        private static TagWriterReturnCode WriteTagsForRow(Entry entry, int processed, DebugStack ds)
        {
            TagLib.File file;
            ds.Log("#" + processed + " - " + entry.fileName);

            // Check if the entry is mapped to a file
            if (entry.isMapped == false)
            {
                string message = "Entry is not matched to file";
                ds.LogWarning(" -> " + message);
                entryErrors.TryAdd(entry, message);
                return TagWriterReturnCode.Unmapped;
            }

            bool isRetry = false;

        Retry:

            // Create TagLib file representation, catch to see if the file is valid
            try
            {
                file = TagLib.File.Create(entry.mappedFilePath, isRetry ? ReadStyle.Average : ReadStyle.PictureLazy);
            }
			catch (Exception exception)
            {
                var type = exception.GetType();
                string errorMessage = "";
                TagWriterReturnCode returnCode;

                if (type == typeof(UnsupportedFormatException))
                {
                    errorMessage = "File format with extension \"" + Path.GetExtension(entry.mappedFilePath) + "\" is unsupported (" + exception.Message + ")";
                    returnCode = TagWriterReturnCode.UnsupportedFormat;
                }
                else if (type == typeof(CorruptFileException))
                {
                    errorMessage = "File appears corrupt! " + exception.Message;
                    returnCode = TagWriterReturnCode.CorruptFile;
                }
                else if (type == typeof(IOException))
                {
                    errorMessage = "An IO exception occured while loading the file! " + exception.Message;

                    // Get the locking processes (if any)
                    var lockingProcesses = FileLockUtility.GetProcessesLockingFile(entry.mappedFilePath);

                    // Log locking processes
                    if (lockingProcesses != null && lockingProcesses.Count > 0)
                        Debug.LogError("\tThe file appears to be locked by the process(es): " +
                            string.Join(", ", lockingProcesses.Select(p => p.ProcessName).ToArray()));

                    returnCode = TagWriterReturnCode.IOEError;
                }
                else
                {
                    errorMessage = "An error occured while loading the file! " + exception.Message;
                    returnCode = TagWriterReturnCode.OtherError;
                }

                ds.LogError(" -> " + errorMessage);
                entryErrors.TryAdd(entry, errorMessage);
                return returnCode;
            }

            // Before doing anything, remove any tags that aren't on the disk.
            TagTypes tagLibCreatedTags = file.TagTypes ^ file.TagTypesOnDisk;
            file.RemoveTags(tagLibCreatedTags);

            // Then create a new tag type if needed
            TagLibUtility.CreateTagIfRequired(file, out TagTypes usedTagTypes);

            // Remove the ID3v1 tag if set in the settings at this point
            if (Settings.Current.removeID3v1 && file.TagTypes.HasFlag(TagTypes.Id3v1))
                file.RemoveTags(TagTypes.Id3v1);

			// --- RESUME COPYING OVER FROM HERE ---

            // If we're in remove move
            if (args.removeTags)
            {
                if (!args.skipDateAdded)
                    TagLibUtility.RemoveCustomTag(file, Consts.TAG_DATE_ADDED, ds);
                if (!args.skipLastPlayed)
                    TagLibUtility.RemoveCustomTag(file, Consts.TAG_LAST_PLAYED, ds);
                if (!args.skipPlayCount)
                    TagLibUtility.RemoveCustomTag(file, Consts.TAG_PLAY_COUNT, ds);
                if (!args.skipRating)
                    TagLibUtility.RemoveRating(file, ds);

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
                        ds.Log(string.Format("\tWriting {0}: {1}\n\t-> Converted to {3} (or {2})", Consts.PLIST_KEY_DATE_ADDED, entry.dateAdded.ToString("o", System.Globalization.CultureInfo.InvariantCulture), entry.dateAdded.ToString(), dateAddedWinTime));

                    // Write the tag
                    TagLibUtility.WriteCustomTag(file, Consts.TAG_DATE_ADDED, dateAddedWinTime.ToString(), ds);
                }

                // Last Played
                if (!args.skipLastPlayed && entry.lastPlayed != DateTime.MinValue)
                {
                    // Create LDAP/Windows File Time long
                    long lastPlayedWinTime = entry.lastPlayed.ToFileTime();

                    if (Settings.Current.fullLogging)
                        ds.Log(string.Format("\tWriting {0}: {1}\n\t-> Converted to {3} (or {2})", Consts.PLIST_KEY_LAST_PLAYED, entry.lastPlayed.ToString("o", System.Globalization.CultureInfo.InvariantCulture), entry.lastPlayed.ToString(), lastPlayedWinTime));

                    // Write the tag
                    TagLibUtility.WriteCustomTag(file, Consts.TAG_LAST_PLAYED, lastPlayedWinTime.ToString(), ds);
                }

                // Play Count
                if (!args.skipPlayCount && entry.playCount > 0)
                {
                    if (Settings.Current.fullLogging)
                        ds.Log(string.Format("\tWriting {0}: {1}", Consts.PLIST_KEY_PLAY_COUNT, entry.playCount));

                    // Write the tag
                    TagLibUtility.WriteCustomTag(file, Consts.TAG_PLAY_COUNT, entry.playCount.ToString(), ds);
                }

                // Rating
                if (!args.skipRating && entry.rating != Rating.Unrated)
                {
                    int ratingInStars = (int)entry.rating;

                    if (Settings.Current.fullLogging)
                        ds.Log(string.Format("\tWriting {0}: {1} ({2} stars)", Consts.PLIST_KEY_RATING, ratingInStars * 20, ratingInStars));

                    // Write the tag
                    TagLibUtility.WriteRating(file, ratingInStars, ds);
                }

                // Process WAV files (after writing stats, since an ID3v2 tag should exist then)
                if (Settings.Current.writeInfoToWavFiles && file.MimeType == "taglib/wav")
                {
                    // This will compare the file with the entry, and will write any missing info that is present in the entry - but not present in the file. Limited to the following tags: Name, Artist, Album Artist, Album, Genre, Comments, Year, Disc Number, Disc Count, Track Number, Track Count

                    ds.Log("\tWriting WAV info:");

                    // Create an Id3v2 tag if it doesn't already exist.
                    var id3v2tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);

                    // Create a RIFF INFO tag if it doesn't already exist
                    // foobar2000 seems to require a RIFF INFO chunk with at least one tag to detect the ID3v2 tags
                    var riffInfo = (TagLib.Riff.InfoTag)file.GetTag(TagTypes.RiffInfo, true);

                    // Write track title
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_TITLE)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_TITLE)) && !string.IsNullOrEmpty(entry.trackTitle))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_TITLE, entry.trackTitle, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_TITLE, entry.trackTitle, ds);
                    }

                    // Write artist
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_ARTIST)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_ARTIST)) && !string.IsNullOrEmpty(entry.artist))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_ARTIST, entry.artist, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_ARTIST, entry.artist, ds);
                    }

                    // Write album artist (ID3v2 only)
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_ALBUM_ARTIST)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_ALBUM)) && !string.IsNullOrEmpty(entry.albumArtist))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_ALBUM_ARTIST, entry.albumArtist, ds);
                    }

                    // Write album
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_ALBUM)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_ALBUM)) && !string.IsNullOrEmpty(entry.album))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_ALBUM, entry.album);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_ALBUM, entry.album);
                    }

                    // Write genre
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_GENRE)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_GENRE)) && !string.IsNullOrEmpty(entry.genre))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_GENRE, entry.genre, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_GENRE, entry.genre, ds);
                    }

                    // Write year
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_DATE)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_YEAR)) && !string.IsNullOrEmpty(entry.year))
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_DATE, entry.year, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_YEAR, entry.year, ds);
                    }

                    // Write track number
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_TRACK_NUMBER)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_TRACK_NUMBER)) && entry.trackNumber != null)
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_TRACK_NUMBER, entry.trackNumberDisplay, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_TRACK_NUMBER, entry.trackNumberDisplay, ds);
                    }

                    // Write disc number (ID3v2 only)
                    if (string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.ID3v2_FRAME_DISC_NUMBER)) && entry.discNumber != null)
                    {
                        TagLibUtility.WriteTextTag(id3v2tag, Consts.ID3v2_FRAME_DISC_NUMBER, entry.discNumberDisplay, ds);
                    }

                    // Write comments
                    if (string.IsNullOrEmpty(TagLibUtility.GetCommentsTag(file)) &&
                        string.IsNullOrEmpty(TagLibUtility.GetTextTag(file, Consts.RIFF_ID_COMMENTS)) && !string.IsNullOrEmpty(entry.comments))
                    {
                        TagLibUtility.WriteCommentsTag(file, entry.comments, ds);
                        //TagLibUtility.WriteTextTag(riffInfo, Consts.RIFF_ID_COMMENTS, entry.comments, ds);
                    }
                }

                entry.wroteTags = true;
            }

            // Write the modified tags to the file
            try
            {
                if (Settings.Current.fullLogging)
                {
                    TagTypes newTags = file.TagTypesOnDisk ^ file.TagTypes;

                    ds.Log("\tTags for writing:\t" + file.TagTypes.ToString());
                    ds.Log("\t    New tags:\t\t" + newTags.ToString());
                    ds.Log("\t    Existing tags:\t" + file.TagTypesOnDisk.ToString());
                }

                if (!Settings.Current.dryRun)
                    file.Save();

                // If we're here, it means the TagLib.File was saved correctly

                if (isRetry)
                {
                    ds.LogSuccess("\tReattempt succeeded");
                }
            }
            catch (IOException ioex)
            {
                // If this was a retry...
                if (isRetry)
                {
                    string message = "Could not save file after retry. An exception occurred: " + ioex.Message;
                    ds.LogError("\t" + message);

                    entryErrors.TryAdd(entry, message);
                    return TagWriterReturnCode.IOEError;
                }
                else
                {
                    ds.LogError("\t-> An IO exception occured while saving tags to the file \"" + entry.fileName + "\"");

                    // Get the locking processes (if any)
                    var lockingProcesses = FileLockUtility.GetProcessesLockingFile(entry.mappedFilePath);

                    // Print the process(es) that are locking the file
                    if (lockingProcesses != null && lockingProcesses.Count > 0)
                    {
                        string message = "The file appears to be locked by the process: " + string.Join(", ", lockingProcesses.Select(p => p.ProcessName).ToArray());
                        ds.LogError("\t" + message);

                        entryErrors.TryAdd(entry, message);
                        return TagWriterReturnCode.IOEError;
                    }

                    // Otherwise if there is no locking process, this is a known issue in TagLib while using ReadStyle.PictureLazy.
                    // Reattempt, using the default ReadStyle.Average
                    else
                    {
                        ds.LogError("\t-> Reattempting load with different parameters...");
                        isRetry = true;
                        goto Retry;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "An error occurred while saving tags to \"" + entry.fileName + "\": (" + ex.Message + ")";
                ds.LogError("\t" + message);
                entryErrors.TryAdd(entry, message);
                return TagWriterReturnCode.OtherError;
            }

            file.Dispose();
            return TagWriterReturnCode.Success;
        }
    }
}
