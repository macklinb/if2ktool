using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace if2ktool
{
    public static class Consts
    {
        // Plist element name
        public const string ELEMENT_PLIST = "plist";

        // "Key" element values in the XML/plist
        public const string PLIST_KEY_MUSIC_LIBRARY = "Music Folder";
        public const string PLIST_KEY_TRACKS = "Tracks";
        public const string PLIST_KEY_PLAYLISTS = "Playlists";

        // Main tags
        public const string PLIST_KEY_TRACK_ID = "Track ID";
        public const string PLIST_KEY_NAME = "Name";
        public const string PLIST_KEY_ARTIST = "Artist";
        public const string PLIST_KEY_ALBUM_ARTIST = "Album Artist";
        public const string PLIST_KEY_ALBUM = "Album";
        public const string PLIST_KEY_TRACK_NUMBER = "Track Number";
        public const string PLIST_KEY_YEAR = "Year";

        // Misc tags
        public const string PLIST_KEY_TOTAL_TIME = "Total Time";
        public const string PLIST_KEY_SIZE = "Size";
        public const string PLIST_KEY_LOCATION = "Location";
        public const string PLIST_KEY_COMPILATION = "Compilation";
        public const string PLIST_KEY_HAS_VIDEO = "Has Video";
        public const string PLIST_KEY_PODCAST = "Podcast";
        public const string PLIST_KEY_TRACK_TYPE = "Track Type";

        // Tags for writing
        public const string PLIST_KEY_DATE_ADDED = "Date Added";
        public const string PLIST_KEY_LAST_PLAYED = "Play Date UTC";
        public const string PLIST_KEY_PLAY_COUNT = "Play Count";
        public const string PLIST_KEY_RATING = "Rating";

        // Playlist tags
        public const string PLIST_KEY_PLAYLIST_ID = "Playlist ID";
        public const string PLIST_KEY_PLAYLIST_PERSISTENT_ID = "Playlist Persistent ID";
        public const string PLIST_KEY_PLAYLIST_ITEMS = "Playlist Items";
        public const string PLIST_KEY_PLAYLIST_SMART_INFO = "Smart Info";
        public const string PLIST_KEY_PLAYLIST_SMART_CRITERIA = "Smart Criteria";

        public const string VALID_TRACK_TYPE = "File";

        // Valid command line arguments to pass to if2ktool
        public const string ARG_DRY_RUN = "dryrun";
        public const string ARG_UNATTENDED = "unattended";
        public const string ARG_ANY_EXTENSION = "anyext";
        public const string ARG_SKIP_DATE_ADDED = "skip_dateadded";
        public const string ARG_SKIP_LAST_PLAYED = "skip_lastplayed";
        public const string ARG_SKIP_PLAY_COUNT = "skip_playcount";
        public const string ARG_SKIP_RATING = "skip_rating";

        // Tag key titles to write for foo_playcount
        public const string TAG_DATE_ADDED = "added_timestamp";
        public const string TAG_LAST_PLAYED = "last_played_timestamp";
        public const string TAG_PLAY_COUNT = "play_count";
        public const string TAG_RATING = "rating";

        // Misc constants
        public const string APPLE_TAG_PREFIX = "\0\0\0\0";
        public const string APPLE_TAG_MEANSTRING = "com.apple.iTunes";
        public const string ID3_POPM_USER = "Windows Media Player 9 Series";
        public const byte ID3_POPM_RATING_1 = 1;
        public const byte ID3_POPM_RATING_2 = 64;
        public const byte ID3_POPM_RATING_3 = 128;
        public const byte ID3_POPM_RATING_4 = 196;
        public const byte ID3_POPM_RATING_5 = 255;

        public const string DEFAULT_ARTIST_STR = "Unknown Artist";
        public const string DEFAULT_ALBUM_STR = "Unknown Album";
        public const string DATE_TIME_FORMAT = "MM'/'dd'/'yyyy hh':'mm tt";
        public const string NOT_EQUAL_CHAR = "≠";

        // Tooltip strings
        public const string TOOLTIP_FULL_LOGGING =
@"Logs much more information (including some debug information) to the console.
Enabling this will slow down processes, and will consume much more memory.";
        public const string TOOLTIP_LOG_TO_FILE =
@"Logs all console output to a file, in the same folder as the executable.

Note that this may have a significant performance impact when logging in the same thread,
especially when ""Full logging"" is enabled. Consider enabling ""Log in separate thread"".";
        public const string TOOLTIP_THREADED_LOGGING =
@"Performs all logging in a separate thread. This greatly speeds up processing time, but
means that the logging is no longer in sync with the process, which has a side-effect of
not being able to pause the application by selecting the log.";
        public const string TOOLTIP_DELAY_START =
@"(Default: true) Delay worker processes by a few seconds before starting.";
        public const string TOOLTIP_WORKER_ON_ERROR =
@"(Default: true) Whenever an error occurs while matching or writing tags, pause the worker
temporarily and take the action below before continuing. This can help identify issues as
they come up, especially with a fast-scrolling console. Note that this doesn't affect pre
or post process errors.

None - Do nothing.
Wait - Pause the worker for a few moments, enough to make it obvious, then continue.
Prompt - Print the error to a message box, will wait until the message box is dismissed
before continuing the worker.";
        public const string TOOLTIP_PROGRESS_REPORT_INTERVAL =
@"(Default: 20) The interval, in milliseconds, between updating the main form UI
(e.g. the progress bar and DataGridView selection) from the worker thread.
Note that setting this to very low values can and will have a performance impact,
as the UI will be redrawn more often.";
        public const string TOOLTIP_PROPAGATE_CHECK_EDITS =
@"(Default: false) When true, changing the ""checked"" state of an entry while multiple
entries are selected will propagate the checked state to the other rows.";
        public const string TOOLTIP_ALLOW_EDIT_ROW_HEIGHT =
@"(Default: false) When true, allows the editing of row heights in the entries panel.
Having this disabled prevents accidentally changing the row height when selecting rows,
which is easy to do with a narrow row height.";
        public const string TOOLTIP_ANY_EXTENSION =
@"(Default: false) If you wish to map to files that may have different extensions.
Firstly attempts to match files with the original XML entry extension, then looks in
the same directory, choosing the first supported file with the same name.";
        public const string TOOLTIP_FUZZY =
@"(Default: false) If a file at the original/mapped path was not found, this tries to
fuzzily match the original file name with other files in the same directory.
See the readme for details.";
        public const string TOOLTIP_NORMALIZE_STRINGS =
@"(Default: true) If a file at the original/mapped path was not found, we check for a
file at the same path, but with the path string ""normalized"".
This converts strings to ""unicode-normalized form C"", which strips complex unicode
characters such as combining diacritic marks (among other things), converting them to
their original single-character form. See readme for more info.";
        public const string TOOLTIP_FORCE_ID3v2_VERSION =
@"(Default: false) If checked, when writing tags, the ID3v2 version will be forced
instead of writing the tags in the same version as the inputted file.

Setting the forced version to None is the same as not forcing a version.";
        public const string TOOLTIP_DONT_ADD_ID3V1 =
@"(Default: true) Prevent TagLib from adding an ID3v1 tag to files without one.
If this is false, an ID3v1 tag will be created on every single file processed by TagLib.";
        public const string TOOLTIP_FORCE_REMOVE_ID3v1 =
@"(Default: false) Forcibly remove all ID3v1 tags from files processed.";
        public const string TOOLTIP_USE_NUMERIC_GENRES_ID3v2 =
@"(Default: true) Disable to prevent genres from being written as indices in ID3v2 tags.";

        // Matching form match mode - direct
        public const string TOOLTIP_MATCH_MODE_DIRECT =
@"For tagging an iTunes library in-place. Generally most people should use this.
Assumes that the xml refers to an existing library that is accessible on the machine
(or on the network), and that the files referenced by iTunes are the same exact ones
to be loaded by foobar2000. Note that matching is only based on the file path, so if
the path varies slightly, the file will not be matched.";

        // Matching form match mode - mapped
        public const string TOOLTIP_MATCH_MODE_MAPPED =
@"Used to map the paths in the XML to a different library directory.
This substitutes the ""Music Folder"" portion (plus ""Music"") of the track path in
the XML with a path of your choosing. Assumes that the structure of the original
library folder is the same as the one provided.";

        // Matching form match mode - lookup
        public const string TOOLTIP_MATCH_MODE_LOOKUP =
@"Cross reference the XML entries with a json-formatted listing from foobar2000.
In this mode, the tags from the iTunes library XML are compared with the tags in
the ""lookup"" json in order to match them to a file.";

        // Matching form match mode - search
        public const string TOOLTIP_MATCH_MODE_SEARCH =
@"Performs a search for each and every file in the library folder provided, matching
it not based on file path and name - but instead the how the tags contained within
the file match with the tags present in the XML. This mode takes the longest amount
of time, and uses the most amount of memory - caching the tags of every file present
at every depth to reduce repeated lookups.";

        // Matching form search modes
        public const string TOOLTIP_SEARCHMODE =
@"Search All: Recursively looks through all directories below the library folder,
front-to-back. The most extensive search, but will take a lot of time.

Search Endpoint: Searches for files in the endpoint directory ONLY - assuming that
it exists.

Search Front-to-Back: Looks for files in the library directory, stepping down to a
specific directory until the next point in the path doesn't exist, or if we get to the
endpoint.

Search Back-to-Front: Look for files at the endpoint directory, stepping up a directory
if no matches were found, or if the directory doesn't exist. This stops when we get to
the topmost library directory.";

        // Shown in preferences form, on lblLookupMinMatchPercentHeader
        public const string TOOLTIP_LOOKUP_MIN_MATCH_PERCENTAGE =
@"This is the minimum percentage of present and matching tags that an entry needs to have in order
to be matched to a track in the JSON file, during Lookup matching. This is 50% by default.
Low percentages can yield incorrect matches, while higher percentages will limit the margin for error
- and can yield less matches. Note that a 100% match where both entries only have one present tag
is still a perfect match!";

        // Generate JSON lookup form button tooltip
        public const string TOOLTIP_GENERATE_LOOKUP_BUTTON =
@"This will generate the JSON lookup using the AutoHotkey script ""CreateLookupJSON.exe"".

Prerequisites:
- foobar2000 installed
- foo_texttools component installed
- foo_albumlist component installed
- CreateLookupJSON.exe in if2ktool directory
- foo_patterns.text in if2ktool directory

Note that generating a JSON file with this script means that you will need to use the album
list as the context for an exported masstagger script.";

        // Shown in the export playlist form, on chkExtendedM3u
        public const string TOOLTIP_EXTENDED_M3U =
@"If checked, the exported playlist will also contain extended M3U data, which prepends each track
with the track number, artist and title.

This has no effect on the playlist generated in foobar2000, since it will only use the track paths.";

        // Messages
        // Shown after tags have been written
        public const string WRITE_TAGS_FINISHED = "You can now import the tags into foobar2000's foo_playcount by selecting\n\"Playback Statistics -> Import statistics from file tags\" from the context menu.\n\nWhen it has finished importing, you can remove the tags either with \"Tools ->\nRemove stats tags\", or by using foobar2000's \"Masstagger\" component, and removing\nthe following fields: added_timestamp, last_played_timestamp, play_count, rating";

        // Shown after tags have been removed
        public const string REMOVE_TAGS_FINISHED = "Playback Statistic tags should no longer be present in files, but do double check that this is the case...";

        // Shown before tags are written or removed
        public const string WRITE_TAGS_WARNING = "By clicking \"Write tags\", you accept that this tool is experimental, and you take full responsibility for whatever may happen to your library, including corruption of files, loss of data and general unpleasantness (though for the most part, this should not happen).\n\nPlease make sure to back up your files before writing data!\n\nYou should probably also close foobar2000 before doing this, but leaving it open should have no adverse affects.\n\nNote that this operation may take a while depending on the size, tag complexity, and file format of your library (about 1 minute per 1000 tracks on a modern HDD, and likely faster on an SSD). It will also heavily affect (and be affected by) disk I/O in other processes.\n\nYou can cancel this process by pressing Ctrl-C while in the console window, or with Action -> Cancel tag writer. \n\nClick \"OK\" to continue, and \"Cancel\" to abort.";

        public const string UNMATCHED_JSON = "The following entries in the JSON were not matched to an entry in the XML. This means that they exist in foobar2000, but not in the iTunes Library, which may be the case for:\n- Files that are unsupported by iTunes and supported by foobar\n- If you have deleted tracks in iTunes and forgot to delete them on disk (leaving remnants in the iTunes Library folder)\n- If you have files that exist in the iTunes Library folder, but were not added to iTunes..\n\nUnmatched entries will result in gaps in playback statistics, but otherwise have no effect on the function of this tool. Consider removing them from your foobar2000 library if this is an issue (click the file path below)";

        // Header used for masstagger files. I have no clue what most of these bytes store, but the last 4 null characters are used to store a 4-byte integer representing the amount of characters in the data (including the scheme). 
        public static readonly byte[] MASSTAGGER_HEADER = new byte[]
        {
            // Random garbage portion. Seems to be persistent across all scripts
            0xec, 0xb0, 0x58, 0x0c, 0x9f, 0x7a, 0x92, 0x48, 0xbe, 0xb0, 0x52, 0x84, 0x5b, 0xeb, 0x0c, 0x4b, 0x00, 0x00, 0x00, 0x00,

            // 4 byte integer (Int32) - The number of actions to perform (in thic case 1, a singular Input data)
            0x01, 0x00, 0x00, 0x00,

            // 16 byte hex base / GUID representing the action type to perform (in this case "Input data (one line per track)")
            0xd8, 0x36, 0x23, 0x5a, 0xe4, 0x45, 0x5a, 0x4e, 0x97, 0xef, 0xdf, 0x28, 0xcb, 0x11, 0x3b, 0xe3,

            // 4 byte integer (Int32) - Number of characters in the scheme + data, prefixes the data for each action
            0x00, 0x00, 0x00, 0x00,

            // The above two repeats for every action (with the scheme and data following each)
        };

        // The tag name portion of the masstagger header, (takes up 65 characters including one new line character)
        public static readonly byte[] MASSTAGGER_SCHEME = new byte[]
        {
            // Tag name portion
            // "%added_timestamp%;"
            0x25, 0x61, 0x64, 0x64, 0x65, 0x64, 0x5f, 0x74, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x25, 0x3b,

            // "%last_played_timestamp%;"
            0x25, 0x6c, 0x61, 0x73, 0x74, 0x5f, 0x70, 0x6c, 0x61, 0x79, 0x65, 0x64, 0x5f, 0x74, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x25, 0x3b,

            // "%play_count%;"
            0x25, 0x70, 0x6c, 0x61, 0x79, 0x5f, 0x63, 0x6f, 0x75, 0x6e, 0x74, 0x25, 0x3b,

            // "%rating%\n"
            0x25, 0x72, 0x61, 0x74, 0x69, 0x6e, 0x67, 0x25, 0x0a
        };

        // Used to debug the sort order of the exported file
        public static readonly byte[] MASSTAGGER_SCHEME_DEBUG = new byte[]
        {
            // "%title%\n"
            0x25, 0x74, 0x69, 0x74, 0x6c, 0x65, 0x25, 0x0a
        };
    }
}
