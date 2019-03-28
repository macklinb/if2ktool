using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace if2ktool
{
    [Serializable]
    public class Settings
    {
        public static Settings Current { get; private set; }

        public bool showConsole = true;
        public bool fullLogging = false;
        public bool logToFile = false;
        public bool threadedLogging = true;

        // Worker settings
        public bool workerDelayStart = false;
        public WorkerPauseAction workerErrorAction = WorkerPauseAction.Prompt;
        public WorkerPauseAction workerWarningAction = WorkerPauseAction.None;
        public int workerErrorWaitTime = 500;
        public int workerWarningWaitTime = 500;
        public bool workerReportsProgress = true;
        public int workerReportProgressInterval = 50;
        public bool workerScrollWithSelection = true;
        public bool workerLockViewToSelection = true;

        // Main form settings
        public bool mainPropagateCheckEdits;
        public bool mainAllowEditRowHeight;

        // Matching settings
        public bool matchingAnyExtension;
        public bool matchingFuzzy = true;
        public int matchingFuzzyDistance = 3;
        public bool matchingFuzzyDontPrompt = false;
        public bool matchingNormalize = true;
        public bool matchingCheckForDupes = true;
        public float lookupMinMatchingPercent = 0.5f;
        public bool lookupWarnOnImperfect = true;

        // Tagging settings
        public bool forceID3v2Version = false;
        public ID3v2Version forceID3v2VersionValue = ID3v2Version.ID3v2_3;
        public bool forceID3v2Encoding = false;
        public TagLib.StringType forceID3v2EncodingValue = TagLib.StringType.UTF8;
        public bool removeID3v1;
        public bool useNumericGenresID3v2 = true;

        public int maxParallelThreads = 4;
        public bool dryRun = false;
        public bool writeInfoToWavFiles = false;

        // Loads the settings from file, otherwise loads the default
        public static void Init()
        {
            if (Properties.Settings.Default.savedSettings != null)
                Current = Properties.Settings.Default.savedSettings;
            else
                Current = new Settings();
        }

        // Resets the settings to default. Be sure to update all components that use the settings
        public static void Reset()
        {
            Current = new Settings();
            Properties.Settings.Default.Save();
        }
    }
}
