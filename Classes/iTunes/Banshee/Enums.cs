namespace Banshee.Plugins.iTunesImporter
{
	internal struct Kind
	{
		public string Name, Extension;
		public Kind(string name, string extension)
		{
			Name = name;
			Extension = extension;
		}
	}
			
	internal partial class SmartPlaylistParser
    {
    	private static Kind[] Kinds = {
    		new Kind("Protected AAC audio file", ".m4p"),
    		new Kind("MPEG audio file", ".mp3"),
    		new Kind("AIFF audio file", ".aiff"),
    		new Kind("WAV audio file", ".wav"),
    		new Kind("QuickTime movie file", ".mov"),
    		new Kind("MPEG-4 video file", ".mp4"),
    		new Kind("AAC audio file", ".m4a")
    	};
    	
    	/// <summary>
        /// The methods by which the number of songs in a playlist are limited
        /// </summary>
        private enum LimitMethods
        {
            Minutes = 0x01,
            MB = 0x02,
            Items = 0x03,
            Hours = 0x04,
            GB = 0x05,
        }
        /// <summary>
        /// The methods by which songs are selected for inclusion in a limited playlist
        /// </summary>
        private enum SelectionMethods
        {
            Random = 0x02,
            Title = 0x05,
            AlbumTitle = 0x06,
            Artist = 0x07,
            Genre = 0x09,
            HighestRating = 0x1c,
            LowestRating = 0x01,
            RecentlyPlayed = 0x1a,
            OftenPlayed = 0x19,
            RecentlyAdded = 0x15
        }
        /// <summary>
        /// The matching criteria which take string data
        /// </summary>
        private enum StringFields
        {
            AlbumTitle = 0x03,
            AlbumArtist = 0x47,
            Artist = 0x04,
            Category = 0x37,
            Comments = 0x0e,
            Composer = 0x12,
            Description = 0x36,
            Genre = 0x08,
            Grouping = 0x27,
            Kind = 0x09,
            Title = 0x02,
            Show = 0x3e
        }
        /// <summary>
        /// The matching criteria which take integer data
        /// </summary>
        private enum IntFields
        {
            BPM = 0x23,
            BitRate = 0x05,
            Compilation = 0x1f,
            DiskNumber = 0x18,
            NumberOfPlays = 0x16,
            Rating = 0x19,
            Playlist = 0x28,    // FIXME Move this?
            Podcast = 0x39,
            SampleRate = 0x06,
            Season = 0x3f,
            Size = 0x0c,
            SkipCount = 0x44,
            Duration = 0x0d,
            TrackNumber = 0x0b,
            VideoKind = 0x3c,
            Year = 0x07
        }
        /// <summary>
        /// The matching criteria which take date data
        /// </summary>
        private enum DateFields
        {
            DateAdded = 0x10,
            DateModified = 0x0a,
            LastPlayed = 0x17,
            LastSkipped = 0x45
        }
        /// <summary>
        /// The matching criteria which we do no handle
        /// </summary>
        private enum IgnoreStringFields
        {
            AlbumArtist = 0x47,
            Category = 0x37,
            Comments = 0x0e,
            Composer = 0x12,
            Description = 0x36,
            Grouping = 0x27,
            Show = 0x3e
        }
        /// <summary>
        /// The matching criteria which we do no handle
        /// </summary>
        private enum IgnoreIntFields
        {
            BPM = 0x23,
            BitRate = 0x05,
            Compilation = 0x1f,
            DiskNumber = 0x18,
            Playlist = 0x28,
            Podcast = 0x39,
            SampleRate = 0x06,
            Season = 0x3f,
            Size = 0x0c,
            SkipCount = 0x44,
            TrackNumber = 0x0b,
            VideoKind = 0x3c
        }
        private enum IgnoreDateFields
        {
            DateModified = 0x0a,
            LastSkipped = 0x45
        }
        /// <summary>
        /// The signs which apply to different kinds of logic (is vs. is not, contains vs. doesn't contain, etc.)
        /// </summary>
        private enum LogicSign
        {
            IntPositive = 0x00,
            StringPositive = 0x01,
            IntNegative = 0x02,
            StringNegative = 0x03
        }
        /// <summary>
        /// The logical rules
        /// </summary>
        private enum LogicRule
        {
            Other = 0x00,
            Is = 0x01,
            Contains = 0x02,
            Starts = 0x04,
            Ends = 0x08,
            Greater = 0x10,
            Less = 0x40
        }
    }
}