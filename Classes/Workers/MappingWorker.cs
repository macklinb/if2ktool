using Microsoft.WindowsAPICodePack.Taskbar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TagLib;

namespace if2ktool
{
    public static class MappingWorker
    {
        static BackgroundWorker worker;
        static Main mainForm;

        // Call  Reset() to pause, Set() to unpause
        static ManualResetEvent manualResetEvent;

        public static bool InProgress
        {
            get { return worker.IsBusy;  }
        }

        static bool isPaused;

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

        public class MatchingWorkerArgs
        {
            // Determines how the mapping occurs
            public MappingMode mappingMode;

            // Filter used to produce the rows enumerable, informational use only
            public EntryFilter filter;

            // Check if the filtered rows were retrieved with a NOT boolean operation, informational use only
            // This means that EntryFilter.Mapped and filterNot == true will return unmapped rows
            public bool filterNot;

            // The path to the library to be matched to, in MappingMode.Remapped
            // The path to a JSON representing track data from foobar2000, in MappingMode.Lookup
            public string libraryPath;

            // Rows to perform mapping on
            public IEnumerable<DataGridViewRow> rows;
        }

        public class LookupCollection
        {
            public List<LookupEntry> tracks;
        }

        // This is used to store information from the data exported out of foobar2000, for cross referencing
        public class LookupEntry
        {
            public int index;
            public string title;
            public string artist;
            public string album;

            private string m_trackNumber;
            public string trackNumber
            {
                get { return m_trackNumber; }
                set
                {
                    // Only allow valid numbers to be set
                    // The conversion is to remove padded zeroes that may sneak through
                    if (!string.IsNullOrEmpty(value) && Int32.TryParse(value, out int n))
                        m_trackNumber = n.ToString();
                    else
                        m_trackNumber = null;
                }
            }
            public string path;

            public string displayTitle
            {
                get
                {
                    if (string.IsNullOrEmpty(title))
                        return fileName;
                    else
                        return title;
                }
            }

            public string fileName { get { return Path.GetFileName(path); } }
            public bool isMatched;
        }

        // Static ctor
        static MappingWorker()
        {
            mainForm = (Main)Application.OpenForms["Main"];

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_Completed;

            Console.CancelKeyPress += Console_CancelKeyPress;

            manualResetEvent = new ManualResetEvent(true);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (worker.IsBusy)
                worker.CancelAsync();
        }

        public static void StartWorker(MatchingWorkerArgs args)
        {
            if (worker.IsBusy)
            {
                Debug.LogError("Cannot start mapping while a mapping is already in progress!", true);
                return;
            }
            else
            {
                // Ensure the manualResetEvent is set (unpaused)
                manualResetEvent.Set();

                mainForm.ShowProgress(true);
                //mainForm.dgvEntries.SuspendLayout();
                worker.RunWorkerAsync(args);
            }
        }

        public static void StopWorker()
        {
            if (worker.IsBusy)
            {
                // Cancel the worker
                worker.CancelAsync();

                // Un-pause worker (so that the execution continues to exit)
                Paused = false;
            }
        }

        private static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var args = (ProgressArgs)e.UserState;

            if (Settings.Current.workerScrollWithSelection)
                mainForm.SetRowSelection(args.currentRowIndex, true);

            mainForm.SetProgress(args.processed, args.count, args.timeMs);

            TaskbarManager.Instance.SetProgressValue(args.processed, args.count);

            // Update properties view
            if (args.updateProperties)
                mainForm.UpdateProperties();
        }

        private static void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            mainForm.ShowProgress(false);
            //mainForm.dgvEntries.ResumeLayout(true);

            // Hide the progress state for the taskbar icon
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        // Matching worker BackgroundWorker thread operation
        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load some parameters from the passed arguments
            var args = (MatchingWorkerArgs)e.Argument;
            var mappingMode = args.mappingMode;
            var filter = args.filter;
            var filterNot = args.filterNot;
            var srcLibraryPath = Main.sourceLibraryFolderPath;
            var libraryPath = args.libraryPath;
            var rows = args.rows;
            
            int processed = 0;          // <- Tracks processed
            int count = rows.Count();
            int rowIndex = 0;

            // Get the number of entries that we skipped by using this selectionMode
            int skipped = mainForm.dgvEntries.Rows.Count - rows.Count();

            // Mapping matches
            int numMatches = 0;           // <- Amount of files that were matched successfully with the current mode
            //int directMatches = 0;      // <- Amount of files that were present at the exact path in the XML
            //int remappedMatches = 0;    // <- Amount of files that were mapped to the library path provided by the user
            //int lookupMatches = 0;      // <- Amount of files that were mapped to an entry in the foobar-sourced lookup JSON

            int normalizedMatches = 0;  // <- Amount of tracks that were matched to the normalized version of a path
            int anyExtMatches = 0;      // <- Amount of tracks that were matched to an extension other than the original
            int fuzzyMatches = 0;       // <- Amount of tracks that we matched to a fuzzily-matched file name
            int referredMatches = 0;    // <- Amount of tracks that were referred by using one of the manually matched paths
            int manualMatches = 0;      // <- Amount of tracks that were fixed by the user during matching
            int unmapped = 0;           // <- Amount of tracks that we failed to find a file for (this is calculated automatically)
            int removedMappings = 0;    // <- Amount of tracks that we removed the existing mapping from
            int removedLookupIndexes = 0; // <- Amount of tracks that we removed the existing lookup index from

            // Interval at which to report progress
            //const int progressReportInterval = 20;
            long progressReportCounter = 0;

            // Lookup JSON gets deserialized to a LookupCollection
            LookupCollection lookup = null;

            // List of paths we've manually matched, so that we can refer to these directories for missing files
            var referredPaths = new List<string>();

            // List of paths we should ignore missing matches in.
            var ignoreMissingPaths = new HashSet<string>();

            // List of specific entries we should ignore (used in lookup mode)
            var ignoreEntries = new HashSet<Guid>();

            // List of paths that we've matched, so that we can check for duplicate matches (not in Lookup mode)
            // ! Note that this doesn't take into account previous matches, only current matches !
            var matchedPaths = new HashSet<string>();

            // Do not prompt or error out for unmapped files in this session, or for 
            bool doNotPrompt = false;

            // Set to true if the user checkes "Always do this", and then accepts the MatchConfirmPrompt.
            // If this is true, we do not prompt the user to accept a fuzzily-matched path, and accept it anyway
            bool alwaysAcceptFuzzyMatch = false;

            // Set to true if the user checks "Always do this", and then skips the entry in the MatchConfirmPrompt
            // If this is true, we do not prompt the user to accept a fuzzily-matched path, and skip it instead
            bool alwaysIgnoreFuzzyMatch = false;
            
            Console.Write("\n\n");
            //Console.CursorVisible = false;

            // Exit here if we have nothing to process
            if (count == 0)
            {
                Debug.LogError("No entries to process!", true);
                return;
            }

            System.Diagnostics.Stopwatch sw;

            void ReportProgress(bool updateProperties = false)
            {
                // Report progress to main thread to update DataGridView selection
                worker.ReportProgress(0, new ProgressArgs()
                {
                    processed = processed,
                    count = count,
                    timeMs = sw.ElapsedMilliseconds,
                    currentRowIndex = rowIndex,
                    updateProperties = updateProperties
                });
            }

            // Returns true if an entry was already matched to this path (therefore it is a duplicate)
            bool IsDuplicate(string searchString)
            {
                return matchedPaths.Contains(searchString);
            }

            Debug.Log(string.Format("Mapping " + count + " entries with the following parameters:\n mappingMode:\t\t{0}\n filter:\t\t{1}{2}\n libraryPath:\t\t{3}\n srcLibraryPath:\t{4}\nSettings:\n anyExtension:\t\t{5}\n fuzzy:\t\t\t{6} (distance = {7})\n normalizeStrings:\t{8}\n\nUse Ctrl-C to terminate", mappingMode, filter, filterNot ? " (NOT)" : "", libraryPath, srcLibraryPath, Settings.Current.matchingAnyExtension, Settings.Current.matchingFuzzy, Settings.Current.matchingFuzzyDistance, Settings.Current.matchingNormalize));

            // Wait 3 seconds before starting
            if (Settings.Current.workerDelayStart)
            {
                for (int i = 0; i < 3; i++)
                {
                    Debug.LogInline(".");
                    System.Threading.Thread.Sleep(1000);
                }
                
                Debug.LogLine();
            }

            sw = System.Diagnostics.Stopwatch.StartNew();

            // If the MappingMode is Lookup, load the lookup JSON
            if (mappingMode == MappingMode.Lookup)
            {
                // The file is already confirmed as existing
                string jsonStr = System.IO.File.ReadAllText(libraryPath);

                // List of serialization errors
                List<string> errors = new List<string>();

                var jss = new JsonSerializerSettings
                {
                    Error = delegate (object z, Newtonsoft.Json.Serialization.ErrorEventArgs eea)
                    {
                        errors.Add(eea.ErrorContext.Error.Message);
                        eea.ErrorContext.Handled = true;
                    }
                };
                
                try
                {
                    lookup = Newtonsoft.Json.JsonConvert.DeserializeObject<LookupCollection>(jsonStr);
                }

                // Make sure the file can be deserialized (the Error delegate above doesn't handle malformed json, only deserialization errors)
                catch (Exception x)
                {
                    Debug.LogError("An exception occurred while deserializing (it this a well formed JSON?)\n" + x.Message.ToString(), true);
                    return;
                }

                // Stop if there were internal errors in deserialization
                if (errors.Any())
                {
                    Debug.LogError("One or more errors occurred during deserialization!\n" + 
                                    string.Join("\n", errors.Select(x => "    " + x)), true);
                    return;
                }

                int removedCount = 0;

                // Reverse loop over the lookup and ensure that there are no archives (path contains '|')
                for (int i = lookup.tracks.Count - 1; i >= 0; i--)
                {
                    string path = lookup.tracks[i].path;

                    // Reverse loop over path string (a pipe is more likely to be on the right)
                    for (int c = path.Length - 1; c >= 0; c--)
                    {
                        if (path[c] == '|')
                        {
                            Debug.Log("Removed entry for: " + lookup.tracks[i].displayTitle);
                            removedCount++;
                            lookup.tracks.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (removedCount > 0)
                {
                    sw.Stop();
                    bool prompt = Settings.Current.workerErrorAction == WorkerPauseAction.Prompt;
                    Debug.LogWarning("Writing tags to archived files is not supported!\nRemoved " + removedCount + " entries from the imported JSON file (see the log for more)", true);

                    if (Settings.Current.workerErrorAction == WorkerPauseAction.Wait)
                        System.Threading.Thread.Sleep(Settings.Current.workerErrorWaitTime);

                    sw.Start();
                }
            }

            // Before we start processing rows, show the taskbar progress bar
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

            // Start processing rows
            foreach (DataGridViewRow row in rows)
            {
                // Check if we should pause
                if (isPaused)
                {
                    Debug.Log("--- PAUSED ---");

                    // Stop the timer and report progress
                    sw.Stop();
                    ReportProgress();

                    // Set the taskbar progress bar to paused
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                    // Halt thread and wait until reset
                    manualResetEvent.WaitOne();

                    // If the execution is here, the worker has been unpaused and ManualResetEvent.Reset() was called
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
                    Debug.Log("--- CANCELLED ---");
                    return;
                }

                // Keep track of the row Index so UpdateProgress can use it
                rowIndex = row.Index;
                processed++;

                // Only report progress every <progressReportInterval> milliseconds
                if (Settings.Current.workerReportsProgress && sw.ElapsedMilliseconds - progressReportCounter > Settings.Current.workerReportProgressInterval)
                {
                    progressReportCounter = sw.ElapsedMilliseconds;
                    ReportProgress();
                }

                var entry = (Entry)row.DataBoundItem;

                Debug.Log("#" + processed + ": " + entry.albumArtist + " - \"" + entry.trackTitle + "\"");

                // Log location
                //Debug.Log(string.Format("\t-> {0} (XML): {1}", Consts.PLIST_KEY_LOCATION, entry.location));

                // Remove an already-existing mapping from the entry
                if (entry.isMapped)
                {
                    removedMappings++;
                    entry.mappedFilePath = string.Empty;
                }

                // Remove an already-existing lookup index from the entry
                if (entry.lookupIndex != -1)
                {
                    removedLookupIndexes++;
                    entry.lookupIndex = -1;
                }
                
                // --- Mapped ---

                if (mappingMode == MappingMode.Direct || mappingMode == MappingMode.Mapped)
                {
                    // Get the original path
                    string filePath = entry.filePath;

                    // Combine the new libraryPath with the relativeFilePath portion of the entry
                    if (mappingMode == MappingMode.Mapped)
                    {
                        filePath = Path.Combine(libraryPath, entry.relativeFilePath.TrimStart('\\'));
                    }

                    bool fileExists = System.IO.File.Exists(filePath);

                    // Check to see if the file exists OR if the path is a duplicate (the methods below are in order of severity and accuracy, the bottommost requiring more processing and can provide incorrect results)
                    if (!fileExists || IsDuplicate(filePath))
                    {
                        if (Settings.Current.fullLogging)
                        {
                            if (!fileExists)
                                Debug.LogError("\t-> File doesn't exist at path: " + filePath);
                            else
                                Debug.LogError("\t-> File at path already matched to an entry!");

                            Debug.Log("\t-> Finding replacement...");
                        }

                        bool foundReplacement = false;
                        string directory = Path.GetDirectoryName(filePath);
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

                        // Try to find the file with the entire path normalized, don't allow dupes
                        if (Settings.Current.matchingNormalize && DoNormalizedMatch(filePath, out string normalizedMatchPath) && !IsDuplicate(normalizedMatchPath))
                        {
                            if (Settings.Current.fullLogging)
                                Debug.Log("\t-> DoNormalizeMatch : Normalized to existing path:\n\t\tOld: " + filePath + "\n\t\tNew: " + normalizedMatchPath);

                            normalizedMatches++;
                            filePath = normalizedMatchPath;
                            foundReplacement = true;
                        }

                        // Try find files with a different extension, don't allow dupes
                        else if (Settings.Current.matchingAnyExtension && DoAnyExtMatch(directory, fileNameWithoutExt, out string anyExtMatchPath) && !IsDuplicate(anyExtMatchPath))
                        {
                            if (Settings.Current.fullLogging && Path.GetExtension(entry.location) != Path.GetExtension(filePath))
                                Debug.Log(string.Format("\t-> File with the original extension \"" + Path.GetExtension(filePath) + "\" not found, picked file with the extension \"" + Path.GetExtension(anyExtMatchPath) + "\""));

                            anyExtMatches++;
                            filePath = anyExtMatchPath;
                            foundReplacement = true;
                        }

                        // Try to fuzzy match the file name to other files in the same folder (using levenshtein algorithm), don't allow dupes
                        else if (Settings.Current.matchingFuzzy && DoFuzzyMatch(directory, fileNameWithoutExt, out string fuzzyMatchPath) && !IsDuplicate(fuzzyMatchPath))
                        {
                            if (Settings.Current.fullLogging)
                                Debug.Log("\t-> DoFuzzyMatch : Fuzzily matched to: " + fuzzyMatchPath);

                            // Auto-accept if we shouldn't prompt for fuzzy match
                            if (alwaysAcceptFuzzyMatch || Settings.Current.matchingFuzzyDontPrompt)
                            {
                                fuzzyMatches++;
                                filePath = fuzzyMatchPath;
                                foundReplacement = true;

                                if (Settings.Current.fullLogging)
                                    Debug.Log("\t-> DoFuzzyMatch : Fuzzy match automatically accepted");
                            }
                            else if (alwaysIgnoreFuzzyMatch)
                            {
                                Debug.Log("\t-> DoFuzzyMatch : Fuzzy match automatically ignored");
                                continue;
                            }
                            else
                            {
                                sw.Stop();

                                ReportProgress();

                                // Set the taskbar icon state to paused (yellow)
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                                // Ask the user to confirm the fuzzyMatch, since there's no guarantee that it isn't wrong
                                var matchConfirmPrompt = new MatchConfirmPrompt(filePath, fuzzyMatchPath);

                                // Show dialogue in context of main thread
                                mainForm.Invoke(() => matchConfirmPrompt.ShowDialog());
                                
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

                                sw.Start();

                                // User checked "Always do this", so don't prompt to fuzzy match in the future
                                if (matchConfirmPrompt.alwaysAcceptFuzzyMatch)
                                    alwaysAcceptFuzzyMatch = true;
                                else if (matchConfirmPrompt.alwaysIgnoreFuzzyMatch)
                                    alwaysIgnoreFuzzyMatch = true;

                                // Use the fuzzy (or fixed) path
                                if (matchConfirmPrompt.DialogResult == DialogResult.OK)
                                {
                                    fuzzyMatches++;
                                    filePath = fuzzyMatchPath;
                                    foundReplacement = true;

                                    if (Settings.Current.fullLogging)
                                        Debug.Log("\t-> DoFuzzyMatch : Fuzzy match accepted");
                                }

                                else if (matchConfirmPrompt.DialogResult == DialogResult.Abort)
                                {
                                    worker.CancelAsync();
                                    return;
                                }

                                // Do not match this entry
                                else if (matchConfirmPrompt.DialogResult == DialogResult.Ignore)
                                {
                                    if (Settings.Current.fullLogging)
                                        Debug.Log("\t-> DoFuzzyMatch : User ignored the fuzzy-matched path");

                                    continue;
                                }
                            }
                        }

                        // Try to match the file name with a file in any of the referred paths, don't allow dupes
                        else if (DoReferredPathMatch(referredPaths, Path.GetFileName(filePath), out string referredMatchPath) && !IsDuplicate(referredMatchPath))
                        {
                            if (Settings.Current.fullLogging)
                                Debug.Log("\t-> DoReferredPathMatch : Found match at referred path " + referredMatchPath);

                            referredMatches++;
                            filePath = referredMatchPath;
                            foundReplacement = true;
                        }

                        // If we didn't find a replacement
                        else
                        {
                            if (!System.IO.File.Exists(filePath))
                                Debug.LogError("\t-> File doesn't exist, and we were unable to find a replacement!");
                            else if (IsDuplicate(filePath))
                                Debug.LogError("\t-> File path already mapped to another entry!");

                            // Halt the thread for workerErrorWaitTime
                            if (Settings.Current.workerErrorAction == WorkerPauseAction.Wait)
                            {
                                sw.Stop();
                                System.Threading.Thread.Sleep(Settings.Current.workerErrorWaitTime);
                                sw.Start();
                            }

                            // Prompt the user regarding the error
                            // But only if doNotPrompt is false, and the path isn't in the ignored list
                            else if (Settings.Current.workerErrorAction == WorkerPauseAction.Prompt && !doNotPrompt && 
                                     !ignoreMissingPaths.Contains(Path.GetDirectoryName(filePath)))
                            {
                                sw.Stop();
                                ReportProgress(true);
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                                if (Settings.Current.fullLogging)
                                    Debug.Log("\t-> DoManualMatch : Asking user for manual match");

                                // If the user manually matched the path
                                if (DoManualMatch(filePath, out ManualMatchResult result))
                                {
                                    // Ensure the manual match isn't a duplicate
                                    if (IsDuplicate(result.manualMatchPath))
                                        Debug.Log("\t-> DoManualMatch : Manually matched, but match was already mapped to another entry!");
                                    else
                                    {
                                        manualMatches++;
                                        filePath = result.manualMatchPath;
                                        foundReplacement = true;

                                        if (result.addToReferredPaths)
                                        {
                                            string dir = Path.GetDirectoryName(filePath);

                                            if (!referredPaths.Contains(dir))
                                                referredPaths.Add(dir);
                                        }
                                    }
                                }

                                // Add to ignored paths
                                if (result.addToIgnoredPaths)
                                    ignoreMissingPaths.Add(Path.GetDirectoryName(filePath));
                                    
                                // Don't prompt in the future
                                if (result.ignoreAll)
                                    doNotPrompt = true;
                                
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                                sw.Start();
                            }
                        }

                        // Give up, and move to the next entry
                        if (!foundReplacement)
                        {
                            continue;
                        }
                        else
                        {
                            if (Settings.Current.fullLogging)
                                Debug.Log("\t-> Found replacement using the above method");
                        }
                    }

                    // If we're here, then a file was found
                    if (Settings.Current.fullLogging)
                        Debug.Log(string.Format("\t-> {0} to: {1}", (args.mappingMode == MappingMode.Mapped ? "Remapped" : "Directly mapped"), filePath));

                    numMatches++;

                    // Save the filePath
                    entry.mappedFilePath = filePath;
                }

                // --- LOOKUP MODE --

                // In MappingMode.Lookup, try to cross-reference the XML entry with an entry in the foobar-sourced JSON file
                else if (mappingMode == MappingMode.Lookup)
                {
                    LookupEntry matchedLookupEntry = null;

                    // Candidates, keyed by LookupEntry where the value is the score they got
                    var candidates = new Dictionary<LookupEntry, float>();

                    // For every lookup entry, calculate the percentage of present tags are equal to the present tags in the XML entry.
                    // So if every tag except for one is missing, and that one is equal - that would be a 100% match
                    foreach (LookupEntry lookupEntry in lookup.tracks)
                    {
                        // Do not continue if all values that will be compared are null
                        if (string.IsNullOrEmpty(entry.trackTitle) && string.IsNullOrEmpty(entry.artist) && string.IsNullOrEmpty(entry.album) && entry.trackNumber == null && string.IsNullOrEmpty(entry.fileName))
                            break;

                        // Check if we should cancel
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            Debug.LogError("Cancelled mapping!");
                            return;
                        }
                        
                        // Do not match to already matched entry (storing this value makes MappingMode.Lookup inherantly duplicate proof)
                        if (lookupEntry.isMatched)
                            continue;

                        /*
                        
                        // For ease of use, generate a Tuple list where the left hand value is the value of the tag in the lookupEntry, and the right hand value is the value of a tag in the XML entry
                        var valueTable = new List<Tuple<string, string>>
                        {
                            new Tuple<string, string>(lookupEntry.title, entry.trackTitle),
                            new Tuple<string, string>(lookupEntry.artist, entry.artist),
                            new Tuple<string, string>(lookupEntry.album, entry.album),
                            new Tuple<string, string>(lookupEntry.trackNumber, entry.trackNumber),
                            new Tuple<string, string>(lookupEntry.fileName, entry.fileName)
                        }

                        // Filter the valueTable by tags that are present in both, not just one or neither
                        .Where(x => !IsEitherNull(x.Item1, x.Item2));

                        // If we couldn't get any comparable values, i.e. there was a null value on either side for every value (this actually should never happen unless one of the input files are seriously broken), continue
                        if (valueTable.Count() == 0)
                            continue;

                        float score = 0.0f;

                        // Score is the percentage of values that are equal
                        score = (float)valueTable.Count(x => StringEquality(x.Item1, x.Item2)) / (float)valueTable.Count();

                        */

                        // Faster alternative to above
                        int nonNullValues = 0;
                        int equalValues = 0;

                        if (!IsEitherNull(lookupEntry.title, entry.trackTitle))
                        {
                            nonNullValues++;
                            if (StringEquality(lookupEntry.title, entry.trackTitle))
                                equalValues++;
                        }

                        if (!IsEitherNull(lookupEntry.artist, entry.artist))
                        {
                            nonNullValues++;
                            if (StringEquality(lookupEntry.artist, entry.artist))
                                equalValues++;
                        }

                        if (!IsEitherNull(lookupEntry.album, entry.album))
                        {
                            nonNullValues++;
                            if (StringEquality(lookupEntry.album, entry.album))
                                equalValues++;
                        }

                        if (!IsEitherNull(lookupEntry.trackNumber, entry.trackNumber.ToString()))
                        {
                            nonNullValues++;
                            if (StringEquality(lookupEntry.trackNumber, entry.trackNumber.ToString()))
                                equalValues++;
                        }

                        if (!IsEitherNull(lookupEntry.fileName, entry.fileName))
                        {
                            nonNullValues++;
                            if (StringEquality(lookupEntry.fileName, entry.fileName))
                                equalValues++;
                        }


                        // If we couldn't get any comparable values, i.e. there was a null value on either side for every value (this actually should never happen unless one of the input files are seriously broken), continue
                        if (nonNullValues == 0)
                            continue;

                        // Score is the percentage of values that are equal of values that are not null
                        float score = (float)equalValues / nonNullValues;

                        // Add this to the candidates if the score is >= 50% match (default)
                        if (score >= Settings.Current.lookupMinMatchingPercent)
                        {
                            if (Settings.Current.fullLogging)
                            {
                                Debug.Log("\t-> Candidate (score = " + score + ", " + equalValues + "/" + nonNullValues + " values equal) - " + lookupEntry.displayTitle + "\n" + GenerateEntryComparisonString(entry, lookupEntry));
                            }

                            candidates.Add(lookupEntry, score);

                            // Consider it a perfect match if the score is 100%, stop looking for scores
                            if (score >= 1.0f)
                                break;
                        }
                    }

                    // End foreach

                    // Pick the only score
                    if (candidates.Count == 1)
                    {
                        if (Settings.Current.fullLogging)
                            Debug.Log("\t-> Picked the only candidate");

                        matchedLookupEntry = candidates.Keys.First();
                    }

                    // Pick the best score, or let the user know that there were multiple matches with the same score
                    else if (candidates.Count > 1)
                    {
                        if (Settings.Current.fullLogging)
                            Debug.Log("\t-> " + candidates.Count + " candidates available...");

                        // If there were multiple with the highest score
                        if (candidates.Count(x => x.Value == candidates.Values.Max()) > 1)
                        {
                            sw.Stop();
                            ReportProgress();
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                            Debug.LogError("\t-> Found multiple candidates with the same score");

                            // Prompt the user to select the candidate manually
                            var matchLookupMultiple = new MatchLookupMultiple(entry, candidates.Keys.ToList(), MatchLookupMultiple.Mode.SelectCandidates);

                            DialogResult result = matchLookupMultiple.ShowDialog();

                            if (result == DialogResult.OK)
                                matchedLookupEntry = matchLookupMultiple.matchedLookupEntry;
                            else if (result == DialogResult.Abort)
                            {
                                worker.CancelAsync();
                                return;
                            }

                            sw.Start();
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        }

                        // Pick the highest
                        else
                        {
                            matchedLookupEntry = candidates.First(x => x.Value == candidates.Values.Max()).Key;

                            if (Settings.Current.fullLogging)
                                Debug.Log("\t-> Picked candidate with highest score (" + candidates[matchedLookupEntry] + ")\n\t" + matchedLookupEntry.displayTitle);
                        }
                    }

                    // If we didn't find a match
                    if (matchedLookupEntry == null)
                    {
                        Debug.LogError("\tCould not find any matches in the lookup JSON");

                        // Check if there are actually any lookup entries left (so we aren't looping over nothing)
                        if (lookup.tracks.All(x => x.isMatched))
                        {
                            Debug.Log("All lookup entries are matched, stopping mapping process");
                            break;
                        }

                        // If we should prompt or wait when an error occurs
                        if (Settings.Current.workerErrorAction != WorkerPauseAction.None)
                        {
                            sw.Stop();
                            ReportProgress(true);
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);

                            if (Settings.Current.workerErrorAction == WorkerPauseAction.Wait)
                                System.Threading.Thread.Sleep(Settings.Current.workerErrorWaitTime);
                            else if (Settings.Current.workerErrorAction == WorkerPauseAction.Prompt && !doNotPrompt && !ignoreEntries.Contains(entry.id))
                            {
                                // Prompt the user to select the lookup entry manually
                                var matchLookupMultiple = new MatchLookupMultiple(entry, lookup.tracks.ToList(), MatchLookupMultiple.Mode.SelectAll);
                                mainForm.Invoke(() => matchLookupMultiple.ShowDialog());
                                var result = matchLookupMultiple.DialogResult;

                                // The user clicked "OK"
                                if (result == DialogResult.OK)
                                {
                                    matchedLookupEntry = matchLookupMultiple.matchedLookupEntry;

                                    // If the selected lookup entry was already matched to another XML entry, remove the mapping on that entry
                                    if (matchedLookupEntry.isMatched)
                                    {
                                        Entry alreadyMatchedEntry = mainForm.GetEntries(EntryFilter.AllEntries).FirstOrDefault(x => x.lookupIndex == matchedLookupEntry.index);
                                        alreadyMatchedEntry.mappedFilePath = string.Empty;
                                        alreadyMatchedEntry.lookupIndex = -1;
                                    }
                                }

                                // The user clicked "Ignore"
                                else if (result == DialogResult.Ignore)
                                {
                                    // Ignore all of this album (this ignores the album + album artist on the entry side)
                                    if (matchLookupMultiple.ignoreGuids != null && matchLookupMultiple.ignoreGuids.Length > 0)
                                        ignoreEntries.UnionWith(matchLookupMultiple.ignoreGuids);
                                    else if (matchLookupMultiple.ignoreAllUnmatched)
                                        doNotPrompt = true;
                                }

                                // The user clicked "Abort"
                                else if (result == DialogResult.Abort)
                                {
                                    worker.CancelAsync();
                                    return;
                                }
                            }

                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                            sw.Start();
                        }
                    }

                    // If we did find a match, set some values based on the matched LookupEntry
                    if (matchedLookupEntry != null)
                    {
                        // Set isMatched on the matching LookupEntry
                        matchedLookupEntry.isMatched = true;

                        // Set the mappedFilePath on the entry
                        entry.mappedFilePath = matchedLookupEntry.path;
                        entry.lookupIndex = matchedLookupEntry.index;

                        numMatches++;

                        // Warn if imperfectly matched
                        if (Settings.Current.lookupWarnOnImperfect && candidates != null && candidates.Count > 0 &&
                            candidates.ContainsKey(matchedLookupEntry) && candidates[matchedLookupEntry] != 1.0f)
                        {
                            Debug.LogWarning("\t-> Not perfectly matched!");
                            string comparisonStr = GenerateEntryComparisonString(entry, matchedLookupEntry);
                            Debug.LogWarning(comparisonStr);
                            
                            if (Settings.Current.workerWarningAction == WorkerPauseAction.Wait)
                                System.Threading.Thread.Sleep(Settings.Current.workerWarningWaitTime);
                            else if (Settings.Current.workerWarningAction == WorkerPauseAction.Prompt)
                                MessageBox.Show("iTunes Library XML entry:\n\t" + entry.fileName + "\nwas not perfectly matched to the lookup entry:\n\t" + matchedLookupEntry.fileName + "\n\n" + comparisonStr, "Not perfectly matched", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }

            sw.Stop();

            // Do final UpdateProgress
            ReportProgress();

            // Inform the user of lookup entries that haven't been matched
            if (mappingMode == MappingMode.Lookup)
            {
                var unmatchedEntries = lookup.tracks.Where(x => x.isMatched == false).ToList();

                if (unmatchedEntries != null && unmatchedEntries.Count > 0)
                {
                    var listLookupEntriesForm = new ListLookupEntries(unmatchedEntries);
                    listLookupEntriesForm.ShowDialog();
                }
            }

            // Count the amount of remaining unmapped entries (this can happen if we called break from the entry loop)
            unmapped = count - numMatches;
            
            string timeHr = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).ToString("m\\:ss\\.ff");
            string resultStr = string.Format("Processed {12} entries in {0}ms ({1})\n {2} matches ({3})\n   {4} normalized matches\n   {5} anyExtension matches\n   {6} fuzzy matches\n   {7} referred matches\n   {8} manual matches\n {9} removed mappings\n {10} removed lookup indexes \n {11} unmapped", sw.ElapsedMilliseconds, timeHr, numMatches, mappingMode.ToString().ToLower(), normalizedMatches, anyExtMatches, fuzzyMatches, referredMatches, manualMatches, removedMappings, removedLookupIndexes, unmapped, count);
            Debug.Log("Done! " + resultStr);

            mainForm.Invoke(() => mainForm.Flash(false));
            System.Media.SystemSounds.Exclamation.Play();
            MessageBox.Show(resultStr, "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Utility Methods ---

        // Returns true if both values are null or empty
        static bool AreBothNull(string a, string b)
        {
            return string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b);
        }

        public static bool IsEitherNull(string a, string b)
        {
            return string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b);
        }

        // Checks if string a and b are equal. 
        public static bool StringEquality(string a, string b)
        {
            // Data matches
            if (a == b || a?.Normalize() == b?.Normalize())
                return true;

            // Data doesn't match
            else
                return false;
        }

        static string GenerateEntryComparisonString(Entry entry, LookupEntry lookupEntry)
        {
            var sb = new System.Text.StringBuilder();

            void Append(string key, string iTunesValue, string foobarValue)
            {
                bool eitherNull = IsEitherNull(iTunesValue, foobarValue);
                bool valueEqual = StringEquality(iTunesValue, foobarValue);

                sb.AppendLine("\t" + key + " - " + (valueEqual ? "EQUAL" : "NOT EQUAL") + (eitherNull ? "(IGNORED)" : ""));
                sb.AppendLine("\t  iTunes: \"" + iTunesValue + "\"");
                sb.AppendLine("\t  foobar: \"" + foobarValue + "\"");
            }

            Append("Title", entry.trackTitle, lookupEntry.title);
            Append("Artist", entry.artist, lookupEntry.artist);
            Append("Album", entry.album, lookupEntry.album);
            Append("Track Num", entry.trackNumber.ToString(), lookupEntry.trackNumber);
            Append("File Name", entry.fileName, lookupEntry.fileName);

            sb = sb.Remove(sb.Length - 1, 1);

            /*
            if (!IsEitherNull(entry.trackTitle, lookupEntry.title) &&
                !StringEquality(entry.trackTitle, lookupEntry.title))
            {
                sb.AppendLine("  Title (XML):\n\t\"" + entry.trackTitle + "\"\n\t\"" + lookupEntry.title + "\"");
            }
            if (!IsEitherNull(entry.artist, lookupEntry.artist) &&
                !StringEquality(entry.artist, lookupEntry.artist))
            {
                sb.AppendLine("  Artist (XML):\n\t\"" + entry.artist + "\"\n\t\"" + lookupEntry.artist + "\"");
            }
            if (!IsEitherNull(entry.album, lookupEntry.album) &&
                !StringEquality(entry.album, lookupEntry.album))
            {
                sb.AppendLine("  Album (XML):\n\t\"" + entry.album + "\"\n\t\"" + lookupEntry.album + "\"");
            }
            if (!IsEitherNull(entry.trackNumber, lookupEntry.trackNumber) &&
                !StringEquality(entry.trackNumber, lookupEntry.trackNumber))
            {
                sb.AppendLine("  Track # (XML):\n\t\"" + entry.trackNumber + "\"\n\t\"" + lookupEntry.trackNumber + "\"");
            }
            if (!IsEitherNull(entry.fileName, lookupEntry.fileName) &&
                !StringEquality(entry.fileName, lookupEntry.fileName))
            {
                sb.AppendLine("  File (XML):\n\t\"" + entry.fileName + "\"\n\t\"" + lookupEntry.fileName + "\"");
            }
            */

            return sb.ToString();
        }

        static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
                return m;

            if (m == 0)
                return n;

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) { }

            for (int j = 0; j <= m; d[0, j] = j++) { }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[n, m];
        }

        // --- Matching Methods --

        // Try to match path without combining diacritic characters, converting the combining characters to their single character equivalents. Since it's unlikely a path will contain both combining and single characters at different levels, we do this on the entire path
        // See https://docs.microsoft.com/en-us/dotnet/api/system.string.normalize
        // Returns true if the normalized match found a file
        static bool DoNormalizedMatch(string sourcePath, out string matchPath, bool returnFalseIfIdentical = true)
        {
            if (Settings.Current.fullLogging)
                Debug.Log("\t-> DoNormalizedMatch : Doing normalized match...");

            string normalizedPath = sourcePath.Normalize();

            // Return false if the normalizedPath is the same as the sourcePath, and if returnFalseIfIdentical is true
            // If the path is the exact same as it was before, a file isn't going to magically exist if it didn't before
            if (returnFalseIfIdentical && normalizedPath == sourcePath)
            {
                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> DoNormalizedMatch : Normalized path is identical to input path");

                matchPath = string.Empty;
                return false;
            }

            if (System.IO.File.Exists(normalizedPath))
            {
                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> DoNormalizedMatch : Found file at normalized path");

                matchPath = normalizedPath;
                return true;
            }
            else
            {
                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> DoNormalizedMatch : No file found at normalized path");

                matchPath = string.Empty;
                return false;
            }
        }

        // Try to match the files in the directory with other files with the same name as fileName, but a different extension. This assumes that the directory is a valid and existing absolute directory path (to save having to check), and that fileName does NOT include an extension
        static bool DoAnyExtMatch(string directory, string fileName, out string matchPath)
        {
             Debug.LogFull("\t-> DoAnyExtMatch : Doing any extension match");

            if (Directory.Exists(directory) == false)
            {
                Debug.LogFull("\t-> DoAnyExtMatch : Base directory doesn't exist");

                matchPath = null;
                return false;
            }

            // Get a list of files in the mapped directory that match the name of our mappedFilePath
            var filesWithSameName = Directory.GetFiles(directory, fileName + ".*");

            Debug.LogFull("\t-> DoAnyExtMatch : Found " + filesWithSameName.Length + " files with same name");

            // Multiple files found, select first valid one
            if (filesWithSameName.Length > 0)
            {
                for (int i = 0; i < filesWithSameName.Length; i++)
                {
                    try
                    {
                        TagLib.File.Create(filesWithSameName[i], ReadStyle.PictureLazy);

                        // Process hits here if we didn't catch an exception
                        matchPath = filesWithSameName[i];
                        Debug.LogFull("\t-> DoAnyExtMatch : Matched to file with extension " + Path.GetExtension(matchPath));
                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogFull("\t-> DoAnyExtMatch : File with extension " + Path.GetExtension(filesWithSameName[i]) + " isn't valid");
                        Debug.LogFull("\t-> DoAnyExtMatch : " + e.GetType().Name.ToString() + " - " + e.Message);
                    }
                }
            }

            matchPath = string.Empty;
            return false;
        }

        // Try to match the files in the directory with other files where their names loosely match, but a different extension. This assumes that the directory is a valid and existing directory (to save having to check), and that fileName does NOT include an extension
        static bool DoFuzzyMatch(string directory, string fileName, out string fuzzyMatchPath)
        {
            if (Directory.Exists(directory) == false)
            {
                fuzzyMatchPath = null;
                return false;
            }

            // Return if fuzzyDist is <= 0
            if (Settings.Current.matchingFuzzyDistance <= 0)
            {
                Debug.LogError("\t-> DoFuzzyMatch : fuzzyDist cannot be <= 0");
                fuzzyMatchPath = string.Empty;
                return false;
            }

            var filesInDirectory = Directory.GetFiles(directory);

            // Return if there are no files in the directory
            if (filesInDirectory == null || filesInDirectory.Length == 0)
            {
                fuzzyMatchPath = string.Empty;
                return false;
            }

            int lowestDist = int.MaxValue;
            int lowestDistIndex = -1;

            // Loop over files in the directory
            for (int i = 0; i < filesInDirectory.Length; i++)
            {
                // Calculate the distance between the original fileName and this file name
                int dist = ComputeLevenshteinDistance(fileName, Path.GetFileNameWithoutExtension(filesInDirectory[i]));

                // Check that the levenschtein distance is lower than the last
                if (dist < lowestDist)
                {
                    lowestDist = dist;
                    lowestDistIndex = i;
                }
            }

            if (Settings.Current.fullLogging)
            {
                Debug.Log("\t-> DoFuzzyMatch : Lowest fuzzy dist was " + lowestDist + (lowestDistIndex != -1 ? " for the name \"" + filesInDirectory[lowestDistIndex] + "\"" : ""));
                Debug.Log("\t-> DoFuzzyMatch : (lowestDist (" + lowestDist + ") <= fuzzyDist (" + Settings.Current.matchingFuzzyDistance + ")) == " + (lowestDist <= Settings.Current.matchingFuzzyDistance).ToString());
            }

            // Check if we found a match that is at most fuzzyDist from the best match
            if (lowestDist <= Settings.Current.matchingFuzzyDistance)
            {
                fuzzyMatchPath = filesInDirectory[lowestDistIndex];
                return true;
            }
            else
            {
                fuzzyMatchPath = string.Empty;
                return false;
            }
        }
        
        // Try to match to a file name to a file in any of the directories that were provided by the user manually matching (and checking the topmost checkbox of MatchManualPrompt). We don't do any checks on these files to ensure that they are in fact the correct files, therefore this method should be considered unreliable.
        // This also takes into account matchingAnyExtension and matchingNormalize
        static bool DoReferredPathMatch(List<string> referredPaths, string fileName, out string referredPathMatchPath)
        {
            if (referredPaths == null || referredPaths.Count == 0)
            {
                referredPathMatchPath = null;
                return false;
            }

            // Cache these two variables, as they will be used repeatedly
            string srcFileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string srcFileNameWithoutExtNormalized = srcFileNameWithoutExt.Normalize();

            // Do not filter by source file extension if matchingAnyExtension is true
            string matchingPattern = Settings.Current.matchingAnyExtension ? "*" : "*" + Path.GetExtension(fileName);

            // Enumerate files in all directories (top directories only)
            foreach (string dir in referredPaths)
            {
                if (!Directory.Exists(dir)) continue;

                foreach (string path in Directory.EnumerateFiles(dir, matchingPattern))
                {
                    // Get the target file name
                    string targetFileName = Path.GetFileNameWithoutExtension(path);

                    // Check if the targetFileName matches the sourceFileName
                    if (targetFileName == srcFileNameWithoutExt || targetFileName == srcFileNameWithoutExtNormalized)
                    {
                        referredPathMatchPath = path;
                        return true;
                    }
                }
            }

            // If we're here, ReferredPathMatch found no results
            referredPathMatchPath = null;
            return false;
        }

        // Other values that may have been returned as a result of the manual match
        private struct ManualMatchResult
        {
            public string manualMatchPath;

            public bool ignoreAll;
            public bool addToIgnoredPaths;
            public bool addToReferredPaths;
        }

        // Prompts the user to manually match the entry. Returns true if the user fixed the path, otherwise returns false
        // Also returns a ManualMatchResult, which sets:
        // ignoreAll: If we should never ask the user again in this session
        // addToReferredPaths: If this directory should be used to refer other files in this folder
        static bool DoManualMatch(string filePath, out ManualMatchResult result)
        {
            var matchManualPrompt = new MatchManualPrompt(filePath);
            mainForm.Invoke(() => matchManualPrompt.ShowDialog());

            result = new ManualMatchResult();

            if (matchManualPrompt.DialogResult == DialogResult.Abort)
            {
                worker.CancelAsync();
                return false;
            }

            // Do not manually match in the future
            result.ignoreAll = matchManualPrompt.ignoreAll;

            // Do not prompt the user for other missing files at this path
            result.addToIgnoredPaths = matchManualPrompt.ignoreAtPath;

            // If the user fixed the path
            if (matchManualPrompt.fixedPath != null)
            {
                result.manualMatchPath = matchManualPrompt.fixedPath;

                // Add the fixed path to manuallyMatchedPaths for future reference
                result.addToReferredPaths = matchManualPrompt.lookOther;

                return true;
            }
            
            return false;
        }
    }
}
