using System;
using System.Xml.Linq;
using System.ComponentModel;

namespace if2ktool
{
    // Represents (some) data retrieved from XML (or file, if loading file data)
    // This information remains consistant, and is NOT changed by this tool
    public class TrackInfo
    {
        // Track info
        public int trackId;
        public string trackTitle;
        public string artist;
        public string albumArtist;
        public string album;
        public string trackNumber;
        public string year;

        public bool compilation;
        public int fileSize;
        public int totalTime;
        public string location;
        public string fileName;

        // Tags to write. These exist only for original reference
        public string dateAdded;
        public string lastPlayed;
        public string playCount;
        public string rating;

        // Creates an TrackData object from an XElement, assumes that the XElement
        // passed is the portion of XML referring to a track entry including
        // and containing the element <dict>
        public TrackInfo(XElement element)
        {
            // This value should always be present, so it is safe to convert without checking
            trackId = Convert.ToInt32(PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_TRACK_ID));
            
            trackTitle  = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_NAME);
            artist      = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_ARTIST);
            albumArtist = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_ALBUM_ARTIST);
            album       = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_ALBUM);
            trackNumber = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_TRACK_NUMBER);
            year        = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_YEAR);
            compilation = PlistHelper.IsKeyInDict(element, Consts.PLIST_KEY_COMPILATION);

            dateAdded   = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_DATE_ADDED);
            lastPlayed  = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_LAST_PLAYED);
            playCount   = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_PLAY_COUNT);
            rating      = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_RATING);

            totalTime   = Convert.ToInt32(PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_TOTAL_TIME));
            fileSize    = Convert.ToInt32(PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_SIZE));
            location    = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_LOCATION);
            location    = Uri.UnescapeDataString(location);
            fileName    = System.IO.Path.GetFileName(location);
        }
    }

    // An entry is a combination of TrackInfo (fixed data loaded from the XML), and data that can change
    // It is used mainly to display information in the DataGridView
    public class Entry
    {
        public Entry(XElement element) : this(new TrackInfo(element)) { }

        // Creates an Entry from the XmlData
        public Entry(TrackInfo trackInfo)
        {
            this.trackInfo = trackInfo;

            // Pad track number to two zeroes
            if (!string.IsNullOrEmpty(trackInfo.trackNumber))
                trackInfo.trackNumber = trackInfo.trackNumber.PadLeft(2, '0');

            // Date Added - Parse ISO 8601 date to DateTime object
            if (!string.IsNullOrEmpty(trackInfo.dateAdded))
                dateAdded = DateTime.Parse(trackInfo.dateAdded, null, System.Globalization.DateTimeStyles.AssumeLocal);
            else
                dateAddedDisabled = true;

            // Last Played - Parse ISO 8601 date to DateTime object
            if (!string.IsNullOrEmpty(trackInfo.lastPlayed))
                lastPlayed = DateTime.Parse(trackInfo.lastPlayed, null, System.Globalization.DateTimeStyles.AssumeLocal);
            else
                lastPlayedDisabled = true;

            // Play Count
            if (!string.IsNullOrEmpty(trackInfo.playCount))
                playCount = Convert.ToInt32(trackInfo.playCount);

            // Rating
            if (!string.IsNullOrEmpty(trackInfo.rating))
                rating = (Rating)(Convert.ToInt32(trackInfo.rating) / 20);
            else
                rating = Rating.Unrated;

            // filePath and relativeFilePath
            if (!string.IsNullOrEmpty(Main.sourceLibraryFolderPath))
            {
                // Remove "file://localhost/" portion and replace '/' with '\\'
                filePath = location.Substring(17).Replace('/', '\\');

                // For the relative path, simply remove the music library path portion
                relativeFilePath = filePath.Replace(Main.sourceLibraryFolderPath, "");
            }
        }

        // --- Fields/Properties ---

        // Stores all the XML entry data
        TrackInfo trackInfo;

        // Can be used as a unique key for an entry
        // While trackId can be used, it may not always be present
        public Guid id { get; private set; } = Guid.NewGuid();

        [DisplayName("")]
        public bool isChecked { get; set; } = true;

        // --- Display Fields ---
        // Used to display the track data in our DataGridView

        [DisplayName("ID")]
        public int trackId { get { return trackInfo.trackId; } }

        [DisplayName("Title")]
        public string trackTitle { get { return trackInfo.trackTitle; } }

        [DisplayName("Artist")]
        public string artist
        {
            get
            {
                if (!string.IsNullOrEmpty(trackInfo.artist))
                    return trackInfo.artist;
                else if (!string.IsNullOrEmpty(trackInfo.albumArtist))
                    return trackInfo.albumArtist;
                else
                    return Consts.DEFAULT_ARTIST_STR;
            }
        }

        [DisplayName("Album Artist")]
        public string albumArtist
        {
            get
            {
                if (!string.IsNullOrEmpty(trackInfo.albumArtist))
                    return trackInfo.albumArtist;
                else if (!string.IsNullOrEmpty(trackInfo.artist))
                    return trackInfo.artist;
                else
                    return Consts.DEFAULT_ARTIST_STR;
            }
        }

        [DisplayName("Album")]
        public string album
        {
            get
            {
                if (!string.IsNullOrEmpty(trackInfo.album))
                    return trackInfo.album;
                else
                    return Consts.DEFAULT_ALBUM_STR;
                    
            }
        }

        [DisplayName("#")]
        public string trackNumber { get { return trackInfo.trackNumber; } }

        [DisplayName("Year")]
        public string year { get { return trackInfo.year; } }

        [Browsable(false)]
        public int fileSize { get { return trackInfo.fileSize; } }

        [Browsable(false)]
        public int totalTime { get { return trackInfo.totalTime;  } }

        [Browsable(false)]
        public bool compilation { get { return trackInfo.compilation; } }

        [DisplayName("Location")]
        public string location { get { return trackInfo.location; } }

        [DisplayName("File Name")]
        public string fileName { get { return trackInfo.fileName; } }

        // --- Writing Fields/Properties ---

        // File path that we've mapped to
        [DisplayName("Mapped Path")]
        public string mappedFilePath { get; set; } = "";

        private DateTime _dateAdded;
        private DateTime _lastPlayed;

        [DisplayName("Date Added")]
        public DateTime dateAdded
        {
            get { return _dateAdded; }
            set { _dateAdded = value; dateAddedDisabled = false;  }
        }

        [DisplayName("Last Played")]
        public DateTime lastPlayed
        {
            get { return _lastPlayed; }
            set { _lastPlayed = value; lastPlayedDisabled = false; }
        }

        [DisplayName("Play Count")]
        public int playCount { get; set; }

        [DisplayName("Rating")]
        public Rating rating { get; set; }

        // Misc, non display fields

        // trackInfo.location, without scope + localhost portion.
        public string filePath { get; private set; }

        // File path relative to the Music folder.
        public string relativeFilePath { get; private set; }

        // Instead of dealing with nullable DateTimePickers, we can let the user uncheck the dateAdded/lastPlayed fields, setting the below variables to true which has the same result as setting the field to null (but without losing the original value)
        public bool dateAddedDisabled;
        public bool lastPlayedDisabled;

        // IsMatched/IsMapped returns true if we have mapped this entry with a file
        [Browsable(false)]
        public bool isMapped
        {
            get { return !string.IsNullOrEmpty(mappedFilePath); }
        }

        // This is set to the index of the mapped entry in a JSON file, when mapped with MappingMode.Lookup
        // It should coincide with the order of the playlist view when the JSON was exported from foobar2000
        public int lookupIndex { get; set; } = -1;

        // True when we've written the iTunes tags to the file
        public bool wroteTags;

        // Gets set to true over a singular TagWrite session, to flag the file as processed. This gets reset after writing tags
        public bool processed;

        // Returns a checkmark if wroteTags is true
        public string wroteTagsDisplay
        {
            get { return wroteTags ? "\u2713" : ""; }
        }
    }
}
