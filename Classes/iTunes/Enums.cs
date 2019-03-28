using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// All information regarding the format of smart playlists is courtesy of user cvzi on GitHub (https://github.com/cvzi/itunes_smartplaylist)
namespace if2ktool.iTunes
{
    public enum MatchOperator
    {
        All = 0x00,
        Any = 0x01,
    }

    public enum MediaKind : int
    {
        Music           = 0x01,
        Movie           = 0x02,
        Podcast         = 0x04,
        Audiobook       = 0x08,
        Music_Video     = 0x20,
        TV_Show         = 0x40,
        Home_Video      = 0x400,
        iTunes_Extras   = 0x10000,
        Voice_Memo      = 0x100000,
        iTunes_U        = 0x200000,
        Book            = 0xC00000,
    }

    public enum iCloudStatus
    {
        Purchased   = 0x01,
        Matched     = 0x02,
        Uploaded    = 0x03,
        Ineligible  = 0x04,
        LocalOnly   = 0x05,
        Error       = 0x06,
        Duplicate   = 0x07,
    }

    public enum LoveKind
    {
        None = 0x00,
        Loved = 0x02,
        Disliked = 0x03,
    }

    public enum LocationKind
    {
        Computer = 0x01,
        iCloud   = 0x10
    }

    // The methods by which the number of songs in a playlist are limited
    public enum LimitMethod
    {
        Minutes = 0x01,
        MB      = 0x02,
        Items   = 0x03,
        Hours   = 0x04,
        GB      = 0x05
    }

    // The signs which apply to different kinds of logic
    public enum LogicSign
    {
        None            = -1,
        IntPositive     = 0x00,
        StringPositive  = 0x01,
        IntNegative     = 0x02,
        StringNegative  = 0x03
    }

    // The logical rules
    public enum LogicRule
    {
        Other       = 0x00,
        Is          = 0x01,
        Contains    = 0x02,
        Starts      = 0x04,
        Ends        = 0x08,
        Greater     = 0x10,
        Less        = 0x40
    }

    // The methods by which songs are selected for inclusion in a limited playlist
    // Refers to the dropdown next to "Limit to x y selected by..."
    public enum SelectionMethod
    {
        Random          = 0x02,
        Name            = 0x05,
        Album           = 0x06,
        Artist          = 0x07,
        Genre           = 0x09,
        HighestRating   = 0x1c,
        LowestRating    = 0x01,
        RecentlyPlayed  = 0x1a,
        OftenPlayed     = 0x19,
        RecentlyAdded   = 0x15
    }

    // Type of the number specified in a Date field.
    // Refers to the dropdown next to "<Date field> in the last <number>..."
    public enum DateFieldType
    {
        Days,
        Weeks,
        Months,
    }

    // Field type
    public enum FieldType
    {
        String,
        Integer,
        Date,
        Boolean,
        MediaKind,
        Playlist
    }

    // Field identifier
    public enum Field
    {
        Name = 0x02,
        Album = 0x03,
        Artist = 0x04,
        Bitrate = 0x05,
        SampleRate = 0x06,
        Year = 0x07,
        Genre = 0x08,
        Kind = 0x09,
        DateModified = 0x0a,
        TrackNumber = 0x0b,
        Size = 0x0c,
        Duration = 0x0d,    // <- AKA "Time"
        Comments = 0x0e,
        DateAdded = 0x10,
        Composer = 0x12,
        Plays = 0x16,
        LastPlayed = 0x17,
        DiscNumber = 0x18,
        Rating = 0x19,
        Checked = 0x1d,
        Compilation = 0x1f,
        BPM = 0x23,
        HasArtwork = 0x25,
        Grouping = 0x27,
        PlaylistPersistentID = 0x28,
        Purchased = 0x29,
        Description = 0x36,
        Category = 0x37,
        Podcast = 0x39,
        MediaKind = 0x3c,
        Show = 0x3e,
        Season = 0x3f,
        Skips = 0x44,
        LastSkipped = 0x45,
        AlbumArtist = 0x47,
        SortName = 0x4e,
        SortAlbum = 0x4f,
        SortAlbumArtist = 0x51,
        SortComposer = 0x52,
        SortShow = 0x53,
        VideoRating = 0x59,         // <- Changed to "Content Rating" in later versions of iTunes
        AlbumRating = 0x5a,
        Location = 0x85,
        iCloudStatus = 0x86,

        // Added in 12.2.0.145
        Love = 0x9a,
        AlbumLove = 0x9c,

        // Added in 12.3.2.35
        Work = 0x9f,
        MovementName = 0xa0,
        MovementNumber = 0xa1,

        // ?
    }

    // Note that bytes are big endian!
    // (though this only matters for multi-byte values)

    public enum InfoOffset
    {
        // INFO OFFSETS
        // Offsets for bytes which...
        LIVEUPDATE = 0,             // Determines whether live updating is enabled - Absolute offset
        MATCHBOOL = 1,              // Determines whether logical matching is to be performed - Absolute offset
        LIMITBOOL = 2,              // Determines whether results are limited - Absolute offset
        LIMITMETHOD = 3,            // Determines by what criteria the results are limited - Absolute offset
        SELECTIONMETHOD = 7,        // Determines by what criteria limited playlists are populated - Absolute offset
        LIMITINT = 8,               // Determines the limit amount- Absolute offset
        LIMITCHECKED = 12,          // Determines whether to exclude unchecked items - Absolute offset
        SELECTIONMETHODSIGN = 13,   // Determines whether certain selection methods are "most" or "least" - Absolute offset
    }

    public enum CriteriaOffset
    {
        // CRITERIA OFFSETS
        // Offsets for bytes which...
        RULECOUNT = 11,             // The amount of rules (1 byte Int8)
        LOGICTYPE = 15,             // MatchOperator - Determine whether all or any criteria must match - Absolute offset
        FIELD = 139,                // Determine what is being matched (Artist, Album, &c) - Absolute offset

        // NOTE: The following offsets are relative from the start of each FIELD
        LOGICSIGN = 1,              // determine whether the matching rule is positive or negative (e.g., is vs. is not)
        LOGICRULE = 4,              // The kind of logic used (LogicRule - is, contains, begins, &c)
        STRINGLEN  = 51,            // The length of the string data (2 byte Int16, big endian)
        STRING = 53,                // Begin string data
        INTA = 57,                  // Begin the first int (offset assumes string data is zero)
        INTB = 24,                  // Begin the second int - Relative offset from INTA
        TIMEMULTIPLE = 73,          // Begin the int with the multiple of time
        TIMEVALUE = 65,             // Begin the inverse int with the value of time
        SUBLOGICTYPE = 68,          // Determine whether all or any criteria must match
        SUBINT = 61,                // begin the first int - Relative offset from FIELD

        // Byte offsets for the fields
        INTLENGTH = 67,             // The length of the int criteria starting at the first int
        SUBEXPRESSIONLENGTH = 192,  // The length of a subexpression starting from FIELD
    }
}
