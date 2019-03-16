using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono;

namespace if2ktool.iTunes
{
    public static class SmartPlaylistParser
    {
        // Denotes which fields are strings
        static readonly Field[] STRING_FIELDS = new Field[] { Field.Album, Field.AlbumArtist, Field.Artist, Field.Category, Field.Comments, Field.Composer, Field.Description, Field.Genre, Field.Grouping, Field.Kind, Field.Name, Field.Show, Field.SortAlbum, Field.SortAlbumArtist, Field.SortComposer, Field.SortName, Field.SortShow, Field.VideoRating, Field.MovementNumber, Field.Work };

        // Denotes which fields are integers
        static readonly Field[] INT_FIELDS = new Field[] { Field.BPM, Field.Bitrate, Field.Compilation, Field.DiscNumber, Field.Plays, Field.Rating, Field.Podcast, Field.SampleRate, Field.Season, Field.Size, Field.Skips, Field.Duration, Field.TrackNumber, Field.Year, Field.MovementNumber };

        // Denotes which fields are boolean
        static readonly Field[] BOOL_FIELDS = new Field[] { Field.HasArtwork, Field.Purchased, Field.Checked };

        // Denotes which fields are dates
        static readonly Field[] DATE_FIELDS = new Field[] { Field.DateAdded, Field.DateModified, Field.LastPlayed, Field.LastSkipped };

        // Mapping iTunes Kind to extension
        // Note that rules using partial operators (contains) for iTunes Kind do not make sense when remapped in this context, and will be converted to boolean operators (is)
        static readonly Dictionary<string, string> ITUNES_KIND_TO_EXT = new Dictionary<string, string>()
        {
            { "Protected AAC audio file", ".m4p" },
            { "MPEG audio file", ".mp3" },
            { "AIFF audio file", ".aiff" },
            { "WAV audio file", ".wav" },
            { "QuickTime movie file", ".mov" },
            { "MPEG-4 video file", ".mp4" },
            { "AAC audio file", ".m4a" }
        };

        // iTunes field name to foobar2000 title-formatted field name mapping
        // Commented-out fields are unsupported
        static readonly Dictionary<Field, string> FIELD_MAPPING = new Dictionary<Field, string>()
        {
            { Field.Name, "%title%" },
            { Field.Album, "%album%" },
            { Field.Artist, "%artist" },
            { Field.Bitrate, "%bitrate%" },
            { Field.SampleRate, "%samplerate%" },
            { Field.Year, "[$year(%date%)]" },
            { Field.Genre, "%genre%" },
            { Field.Kind, "$ext(%filename_ext%)" },
            { Field.DateModified, "%last_modified%" },
            { Field.TrackNumber, "%track number%" },
            { Field.Size, "$div($div(%filesize%, 1024), 1024)" },
            { Field.Duration, "%length%" },
            { Field.Comments, "%comment%" },
            { Field.DateAdded, "$date(%added%)" },
            { Field.Composer, "$meta(composer)" },
            { Field.Plays, "%play_count%" },
            { Field.LastPlayed, "$date(%last_played%)" },
            { Field.DiscNumber, "%discnumber%" },
            { Field.Rating, "%rating%" },
            { Field.Compilation, "%itunescompilation%" },
            { Field.BPM, "%bpm%" },
            { Field.Grouping, "%grouping%" },
            { Field.AlbumArtist, "%album artist%" },

            // Sort values are remapped to the regular values
            { Field.SortName, "%title%" },
            { Field.SortAlbum, "%album%" },
            { Field.SortAlbumArtist, "%album artist%" },
            { Field.SortComposer, "%composer%" },

            // All other fields are not supported:
            //{ Field.Checked, "" },
            //{ Field.HasArtwork, "" },
            //{ Field.LastSkipped, "" }
            //{ Field.PlaylistPersistentID, "" }
            //{ Field.Purchased, "" }
            //{ Field.Description, "" }
            //{ Field.Category, "" }
            //{ Field.Podcast, "" }
            //{ Field.MediaKind, ""}
            //{ Field.Show, "" }
            //{ Field.Season, "" }
            //{ Field.Skips, "" }
            //{ Field.SortShow, "" }.
            //{ Field.VideoRating, "" },
            //{ Field.Location, ""},
            //{ Field.iCloudStatus, ""},
            //{ Field.Love, ""}
        };

        // The offset of the apple epoch from the unix epoch (should actually be negative)
        public static int APPLE_OFFSET_FROM_UNIX = 2082844800;

        public static void Parse(string infoStr, string criteriaStr)
        {
            byte[] info = Convert.FromBase64String(infoStr);
            byte[] criteria = Convert.FromBase64String(criteriaStr);

            int offset = (int)CriteriaOffset.FIELD;

            var playlist = new SmartPlaylist();

            // Get some global fields
            playlist.match               = Convert.ToBoolean(info[(int)InfoOffset.MATCHBOOL]);
            playlist.matchOperator       = info[(int)CriteriaOffset.LOGICTYPE] == 1 ? MatchOperator.Any : MatchOperator.All;
            playlist.limit               = Convert.ToBoolean(info[(int)InfoOffset.LIMITBOOL]);
            playlist.limitNumber         = DataConverter.Int32FromBE(info, (int)InfoOffset.LIMITINT);
            playlist.limitMethod         = (LimitMethod)info[(int)InfoOffset.LIMITMETHOD];
            playlist.selectionMethod     = (SelectionMethod)info[(int)InfoOffset.SELECTIONMETHOD];
            playlist.selectionMethodSign = (LogicSign)info[(int)InfoOffset.SELECTIONMETHODSIGN];
            playlist.matchOnlyChecked    = Convert.ToBoolean(info[(int)InfoOffset.LIMITCHECKED]);
            playlist.liveUpdating        = Convert.ToBoolean(info[(int)InfoOffset.LIVEUPDATE]);

            var rules = new List<SmartPlaylist.Rule>();

            while (offset < criteria.Length)
            {
                var rule = new SmartPlaylist.Rule();

                // Get the absolute offsets for this rule
                int logicRuleOffset     = offset + (int)CriteriaOffset.LOGICRULE;
                int logicSignOffset     = offset + (int)CriteriaOffset.LOGICSIGN;
                int stringLengthOffset  = offset + (int)CriteriaOffset.STRINGLEN;
                int stringOffset        = offset + (int)CriteriaOffset.STRING;
                int intAOffset          = offset + (int)CriteriaOffset.INTA;
                int intBOffset          = offset + (int)CriteriaOffset.INTA + (int)CriteriaOffset.INTB; // <- Offset from intAOffset
                int timeMultipleOffset  = offset + (int)CriteriaOffset.TIMEMULTIPLE;
                int timeValueOffset     = offset + (int)CriteriaOffset.TIMEVALUE;

                // Get the field type for this rule
                rule.field = (Field)criteria[offset];
                Debug.Log("Field is " + rule.field.ToString());

                // Get LogicRule
                rule.logicRule = (LogicRule)criteria[logicRuleOffset];

                // Get LogicSign
                rule.logicSign = (LogicSign)criteria[logicSignOffset];

                // If the field is a string
                if (STRING_FIELDS.Contains(rule.field))
                {
                    rule.fieldType = FieldType.String;

                    // Get LogicSign (only for Contains and is)
                    if (rule.logicRule == LogicRule.Contains || rule.logicRule == LogicRule.Is)
                        rule.logicSign = (LogicSign)criteria[logicSignOffset];
                    
                    // The length of the string is stored just before the string, as a 2 byte Int16, big endian
                    short stringLength = DataConverter.Int16FromBE(criteria, stringLengthOffset);
                    rule.stringData = "";

                    // Get data
                    for (int i = 0; i < stringLength; i += 2)
                    {
                        // Each char takes two bytes in order to support wide chars (hence the gaps between the characters)
                        // Not only that, but the byte order is big endian, so we can't just cast to a char
                        rule.stringData += (char)DataConverter.Int16FromBE(criteria, stringOffset + i);//BitConverter.ToChar(criteria, stringOffset + i);
                    }

                    // We're at the end of the string, shift the offset to the end of the string plus 3
                    offset = stringOffset + stringLength + 3;
                }

                // If the field is an integer
                if (INT_FIELDS.Contains(rule.field))
                {
                    rule.fieldType = FieldType.Integer;

                    // Get data (4 byte Int32, big endian)
                    if (rule.logicRule == LogicRule.Is || rule.logicRule == LogicRule.Greater || rule.logicRule == LogicRule.Less)
                        rule.integerA = DataConverter.Int32FromBE(criteria, intAOffset);

                    // Get data (for range)
                    else if (rule.logicRule == LogicRule.Other)
                    {
                        rule.integerA = DataConverter.Int32FromBE(criteria, intAOffset);
                        rule.integerB = DataConverter.Int32FromBE(criteria, intBOffset);
                    }

                    offset = intAOffset + (int)CriteriaOffset.INTLENGTH;
                }

                // If the field is a boolean
                if (BOOL_FIELDS.Contains(rule.field))
                {
                    rule.fieldType = FieldType.Boolean;

                    // Get data
                    rule.booleanData = criteria[logicSignOffset] == 2;

                    offset = intAOffset + (int)CriteriaOffset.INTLENGTH;
                }

                // If the field is a date
                if (DATE_FIELDS.Contains(rule.field))
                {
                    rule.fieldType = FieldType.Date;

                    // Notes:
                    // LogicRule.Greater: "<date> is after"
                    // LogicRule.Less: "<date> is before"
                    // LogicRule.Other:
                    //      LogicRuleOtherMode == 1: Date range
                    //          LogicSign.IntPositive: "is in the range"
                    //          LogicSign.IntNegative: "is not in the range"
                    //      LogicRuleOtherMode == 2: Date within
                    //          LogicSign.IntPositive: "is in the last"
                    //          LogicSign.IntNegative: "is not in the las"

                    // Get data (these are stored in the int fields, but as 4 byte UInt32's, big endian)
                    if (rule.logicRule == LogicRule.Is || rule.logicRule == LogicRule.Greater || rule.logicRule == LogicRule.Less)
                    {
                        rule.timestampA = AppleToUnixTimestamp(DataConverter.UInt32FromBE(criteria, intAOffset));
                    }
                    else if (rule.logicRule == LogicRule.Other)
                    {
                        rule.logicRuleOtherMode = criteria[logicSignOffset + 2];
                        rule.timestampA = AppleToUnixTimestamp(DataConverter.UInt32FromBE(criteria, intAOffset));
                        rule.timestampB = AppleToUnixTimestamp(DataConverter.UInt32FromBE(criteria, intBOffset));
                    }
                    
                    offset = intAOffset + (int)CriteriaOffset.INTLENGTH;
                }

                // If the field is a MediaKind
                if (rule.field == Field.MediaKind)
                {
                    rule.fieldType = FieldType.MediaKind;
                    rule.mediaKindData = (MediaKind)criteria[intAOffset + 4];

                    offset = intAOffset + (int)CriteriaOffset.INTLENGTH;
                }
            }
        }

        // An Apple/HFS timestamp is the number of seconds elapsed since 1st January 1904
        // A unix timestamp is the number of seconds elapsed since 1st January 1970
        // The offset between this the apple and unix epoch is 2082844800, so we have to subtract this value from an apple timestamp in order to get a unix timestamp
        public static uint AppleToUnixTimestamp(uint timestamp)
        {
            return (uint)(timestamp - APPLE_OFFSET_FROM_UNIX);
        }
    }
}
