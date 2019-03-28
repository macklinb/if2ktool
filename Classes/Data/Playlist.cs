using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace if2ktool
{
    // This class represents playlist data loaded from the iTunes XML
    public class Playlist
    {
        public string name { get; set; }
        public int playlistId;
        public string playlistPersistentId;
        public string smartInfo;
        public string smartCriteria;

        public int[] playlistItems;

        public bool IsSmartPlaylist
        {
            get { return !string.IsNullOrEmpty(smartInfo); }
        }

        public int Count
        {
            get { return (playlistItems == null) ? 0 : playlistItems.Length; }
        }

        public Playlist(XElement element)
        {
            name                 = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_NAME);
            playlistId           = Convert.ToInt32(PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_PLAYLIST_ID));
            playlistPersistentId = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_PLAYLIST_PERSISTENT_ID);
            smartInfo            = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_PLAYLIST_SMART_INFO);
            smartCriteria        = PlistHelper.GetValueOfKeyInDict(element, Consts.PLIST_KEY_PLAYLIST_SMART_CRITERIA);

            // Replace line breaks in smartInfo and smartCriteria
            if (smartInfo != null)
            {
                smartInfo = smartInfo.Replace("\n", string.Empty)
                                     .Replace("\t", string.Empty);
            }
            if (smartCriteria != null)
            {
                smartCriteria = smartCriteria.Replace("\n", string.Empty)
                                             .Replace("\t", string.Empty);
            }

            if (IsSmartPlaylist)
            {
                //iTunes.SmartPlaylistParser.Parse(smartInfo, smartCriteria);

                //byte[] info = Convert.FromBase64String(smartInfo);
                //byte[] criteria = Convert.FromBase64String(smartCriteria);
                //Banshee.Plugins.iTunesImporter.SmartPlaylistParser.Parse(info, criteria);
            }

            // Get array element under playlist dict
            var array = element.Element("array");

            if (array != null)
            {
                var items = new List<int>();

                foreach (var trackDicts in array.Descendants("dict"))
                    items.Add(Convert.ToInt32(PlistHelper.GetValueOfKeyInDict(trackDicts, Consts.PLIST_KEY_TRACK_ID)));

                playlistItems = items.ToArray();
            }
        }
    }
}
