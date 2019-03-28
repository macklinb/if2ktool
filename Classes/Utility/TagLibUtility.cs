using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace if2ktool
{
    // Helper class for TagWriterWorker, providing an interface with TagLibSharp
    public static class TagLibUtility
    {
        // --- TagLib Methods ---

        // This dictionary defines the preferred tagtypes to use for writing playback statistics data
        // These will usually only be TagTypes that support custom, non-fixed data
        // In CreateTagIfRequired, the file will be checked against the preferred tagtypes, and will return the first present tagtypes.
        // If none of the preferred tagtypes exist, the a new tag with the TagType at index 0 will be created on the file
        static readonly Dictionary<Type, TagTypes[]> PREFERRED_TAGTYPES = new Dictionary<Type, TagTypes[]>()
        {
            // AAC - Advanced Audio Codec - Apple/ID3v2
            { typeof(TagLib.Aac.File), new TagTypes[] { TagTypes.Apple, TagTypes.Id3v2 } },

            // APE - Monkey's Audio - APE tag / 
            { typeof(TagLib.Ape.File), new TagTypes[] { TagTypes.Ape, TagTypes.Id3v2 } },
            
            // AIFF - Audio Interchange File Format - ID3v2/XMP
            { typeof(TagLib.Aiff.File), new TagTypes[] { TagTypes.Id3v2 } },

            // FLAC - Free Lossless Audio Codec - Xiph only
            { typeof(TagLib.Flac.File), new TagTypes[] { TagTypes.Xiph } },

            // MPEG4 - Covers both ALAC (Apple Lossless Audio Codec) and MP4 (Motion Picture Experts Group Layer 4) files - Aple/ID3v2
            { typeof(TagLib.Mpeg4.File), new TagTypes[] { TagTypes.Apple, TagTypes.Id3v2 } },

            // MP3 - MPEG-2 Audio Layer III - ID3v2 / APE
            { typeof(TagLib.Mpeg.AudioFile), new TagTypes[] { TagTypes.Id3v2, TagTypes.Ape } },

            // OGG - Xiph only
            { typeof(TagLib.Ogg.File), new TagTypes[] { TagTypes.Xiph } },

            // RIFF - Resource Interchange File Format, covers WAV files
            { typeof(TagLib.Riff.File), new TagTypes[] { TagTypes.Id3v2 } }
        };

        // Creates a new tag type on the file if needed, depending on the input file type. Returns true if a new tag was created. The out variable usedTagType is set to the tag that should be used
        public static bool CreateTagIfRequired(TagLib.File file, out TagTypes usedTagType)
        {
            usedTagType = TagTypes.None;

            if (!PREFERRED_TAGTYPES.TryGetValue(file.GetType(), out TagTypes[] tagTypes))
                return false;

            // Get any preferred TagType on the file
            for (int i = 0; i < tagTypes.Length; i++)
            {
                if (file.TagTypesOnDisk.HasFlag(tagTypes[i]))
                {
                    usedTagType = tagTypes[i];
                    return false;
                }
            }

            // If we're here, the file has no preferred TagTypes on it - so we have to make one
            file.GetTag(tagTypes[0], true);
            usedTagType = tagTypes[0];
            return true;
        }

        // Write custom, non-standard key-value pair to file tags
        // The tag type selected is the first one present, rather than having to specify
        public static void WriteCustomTag(TagLib.File file, string key, string value, DebugStack ds = null)
        {
            // Write ID3v2 TXXX Frame for MP3/AAC/AIFF
            // Note that we are writing in extended ASCII equivalent, as this method should only be called when writing statistics tags
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);
                var textframe = TagLib.Id3v2.UserTextInformationFrame.Get(tag, key, StringType.Latin1, true);
                textframe.Text = new string[] { value };

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote ID3v2 TXXX Frame: key=\"" + key + "\", value=\"" + value + "\"", ds);
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
                    Debug.Log("\t-> Wrote AppleAnnotationBox tag: key=\"" + key + "\", value=\"" + value + "\"", ds);
            }

            // Write XiphComment field for FLAC
            else if (file.TagTypes.HasFlag(TagTypes.Xiph))
            {
                var tag = (TagLib.Ogg.XiphComment)file.GetTag(TagTypes.Xiph);
                tag.SetField(key, new string[] { value });

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote Ogg.XiphComment: key=\"" + key + "\", value=\"" + value + "\"", ds);
            }

            // Write APE, should it exist
            else if (file.TagTypes.HasFlag(TagTypes.Ape))
            {
                var tag = (TagLib.Ape.Tag)file.GetTag(TagTypes.Ape);
                tag.SetValue(key, value);


                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote Ogg.XiphComment: key=\"" + key + "\", value=\"" + value + "\"", ds);
            }

            else
            {
                Debug.LogWarning("\t-> Could not write data. Unsupported or missing tag type!", ds);
            }
        }

        // Removes a custom tag with <key> from <file>. Returns true if a tag was found and removed
        public static bool RemoveCustomTag(TagLib.File file, string key, DebugStack ds = null)
        {
            // Remove ID3v2 TXXX Frame for MP3/AAC/AIFF
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, false);
                var textFrame = TagLib.Id3v2.UserTextInformationFrame.Get(tag, key, StringType.UTF16, false);

                if (textFrame != null)
                {
                    if (Settings.Current.fullLogging)
                    {
                        Debug.Log("\t-> Removed ID3v2 TXXX Frame with the key \"" + key + "\"", ds);
                    }

                    tag.RemoveFrame(textFrame);
                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An ID3v2 TXXX Frame with the key \"" + key + "\" was not present", ds);

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
                            Debug.Log("\t-> Removed AppleAnnotationBox with the key \"" + key + "\"", ds);

                        // Setting the DashBox to an empty string will remove it. For some reason we don't need to include 4 null characters when removing
                        tag.SetDashBox(Consts.APPLE_TAG_MEANSTRING, key, string.Empty);
                    }

                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An AppleAnnotationBox with the key \"" + key + "\" was not present", ds);

                    return false;
                }
            }

            // Remove XiphComment field for FLAC
            else if (file.TagTypes.HasFlag(TagTypes.Xiph))
            {
                var tag = (TagLib.Ogg.XiphComment)file.GetTag(TagTypes.Xiph);

                if (tag.GetField(key) != null)
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed XiphComment with the key \"" + key + "\"", ds);

                    tag.RemoveField(key);
                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> A XiphComment with the key \"" + key + "\" was not present", ds);

                    return false;
                }
            }


            // Write APE, should it exist
            else if (file.TagTypes.HasFlag(TagTypes.Ape))
            {
                var tag = (TagLib.Ape.Tag)file.GetTag(TagTypes.Ape);

                if (tag.GetItem(key) != null)
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed APE item with the key \"" + key + "\"", ds);

                    tag.RemoveItem(key);
                    return true;
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An APE item with the key \"" + key + "\" was not present", ds);

                    return false;
                }
            }

            else
            {
                Debug.LogWarning("\t-> Could not remove data. Unsupported or missing tag type!", ds);
            }

            return false;
        }

        // Write rating to tags using Rating enum
        public static void WriteRating(TagLib.File file, Rating rating, DebugStack ds = null)
        {
            // Do not write if rating is unrated
            if (rating == Rating.Unrated)
            {
                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Rating not written, since the entry was unrated", ds);

                return;
            }
            else
                WriteRating(file, (int)rating, ds);
        }

        // Write rating to tags using a star rating
        public static void WriteRating(TagLib.File file, int starRating, DebugStack ds = null)
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
                        Debug.LogWarning("\t-> Invalid rating of " + starRating + " (" + (starRating * 20) + "). Skipping write...", ds);
                        popmFrame = null;
                        break;
                    }
                }

                if (Settings.Current.fullLogging && popmFrame != null)
                    Debug.Log("\t-> Wrote ID3v2 Popularimeter (POMP) frame: value: " + popmFrame.Rating + " (" + starRating + " stars)", ds);
            }

            // Apple uses a regular text field, with a 1-5 value
            // FLAC uses a regular vorbis comment, with a 1-5 value
            else if (file.TagTypes.HasFlag(TagTypes.Apple) || file.TagTypes.HasFlag(TagTypes.Xiph) || file.TagTypes.HasFlag(TagTypes.Ape))
            {
                WriteCustomTag(file, Consts.TAG_RATING, starRating.ToString(), ds);
            }
        }

        public static void RemoveRating(TagLib.File file, DebugStack ds = null)
        {
            // Remove Id3v2 POPM frame
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, false);
                var popmFrame = TagLib.Id3v2.PopularimeterFrame.Get(tag, Consts.ID3_POPM_USER, false);

                if (popmFrame != null)
                {
                    tag.RemoveFrame(popmFrame);

                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> Removed ID3v2 Popularimeter (POMP) frame", ds);
                }
                else
                {
                    if (Settings.Current.fullLogging)
                        Debug.Log("\t-> An ID3v2 Popularimeter (POMP) frame was not present, so it was not removed", ds);
                }
            }

            // For all other fields, use custom rating field
            else if (file.TagTypes.HasFlag(TagTypes.Apple) || file.TagTypes.HasFlag(TagTypes.Xiph))
            {
                RemoveCustomTag(file, Consts.TAG_RATING, ds);
            }
        }

        // writes a predefined tag to a TagLib tag
        public static void WriteTextTag(TagLib.Tag tag, string key, string value, DebugStack ds = null)
        {
            Type tagType = tag.GetType();

            // Write TextInformationFrame to ID3v2 (see http://id3.org/id3v2.4.0-frames for valid keys)
            if (tagType == typeof(TagLib.Id3v2.Tag))
            {
                // Cast tag to Id3v2 tag
                var id3tag = (TagLib.Id3v2.Tag)tag;
                var frame = TagLib.Id3v2.TextInformationFrame.Get(id3tag, key, true);
                frame.Text = new string[] { value };
                frame.TextEncoding = Settings.Current.forceID3v2EncodingValue;

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote ID3v2 " + key + " Frame: value=\"" + value + "\"", ds);
            }

            // Write RIFF INFO for WAV files
            if (tagType == typeof(TagLib.Riff.InfoTag))
            {
                var riffInfoTag = (TagLib.Riff.InfoTag)tag;
                riffInfoTag.SetValue(key, new string[] { value });

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote RIFF INFO " + key + ": value=\"" + value + "\"", ds);
            }
        }

        // Writes a predefined tag to a file
        // Currently only implemented for ID3v2 and RIFF
        // Key has to be a valid 4-character ID3v2 text frame identifier (starting with 'T', but excluding 'TXXX')
        public static void WriteTextTag(TagLib.File file, string key, string value, DebugStack ds = null, TagTypes forceTagType = TagTypes.None)
        {
            // Override the "presented" tag types if forceTagType is anything other than none
            TagTypes tagTypes = (forceTagType == TagTypes.None) ? file.TagTypes : forceTagType;

            TagLib.Tag tag = null;

            // Write TextInformationFrame to ID3v2 (see http://id3.org/id3v2.4.0-frames for valid keys)
            if (tagTypes.HasFlag(TagTypes.Id3v2))
                tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2, true);

            // Write RIFF INFO for WAV files
            else if (tagTypes.HasFlag(TagTypes.RiffInfo))
               tag = (TagLib.Riff.InfoTag)file.GetTag(TagTypes.RiffInfo, true);

            if (tag == null)
                return;
            else
                WriteTextTag(tag, key, value, ds);
        }

        // Currently only implemented for ID3v2 and RIFF
        // Right now there's no intermediary layer between the key and the general name of the value we want to get (i.e. Artist is TPE1 in ID3v2 and IART on RIFF INFO)
        public static string GetTextTag(TagLib.File file, string key)
        {
            // Write TextInformationFrame to ID3v2 (see http://id3.org/id3v2.4.0-frames for valid keys)
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
                var frame = TagLib.Id3v2.TextInformationFrame.Get(tag, key, false);

                if (frame == null || frame.Text == null || frame.Text.Count() == 0)
                    return null;
                else
                    return frame.Text.First();
            }

            // Write RIFF INFO for WAV files
            else if (file.TagTypes.HasFlag(TagTypes.RiffInfo))
            {
                var tag = (TagLib.Riff.InfoTag)file.GetTag(TagTypes.RiffInfo, false);
                var values = tag.GetValuesAsStrings(key);

                if (values == null || values.Count() == 0)
                    return null;
                else
                    return values[0];
            }

            return null;
        }

        public static void WriteCommentsTag(TagLib.File file, string text, DebugStack ds = null)
        {
            // Write CommentsFrame for ID3v2.
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);

                // Although there isn't a defined specification for the language string, some programs use ISO 639-2 - so might as well use that
                // And description is commonly left empty
                var commentsFrame = TagLib.Id3v2.CommentsFrame.Get(tag, string.Empty, System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName, true);
                commentsFrame.Text = text;

                if (Settings.Current.fullLogging)
                    Debug.Log("\t-> Wrote ID3v2 COMM Frame: language=\"" + commentsFrame.Language + "\", value=\"" + text + "\"", ds);
            }
        }
        
        public static string GetCommentsTag(TagLib.File file)
        {
            // Write CommentsFrame for ID3v2.
            if (file.TagTypes.HasFlag(TagTypes.Id3v2))
            {
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
                var commentsFrame = TagLib.Id3v2.CommentsFrame.GetPreferred(tag, string.Empty, System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);

                if (commentsFrame != null)
                    return commentsFrame.Text;
            }
            
            if (file.TagTypes.HasFlag(TagTypes.RiffInfo))
            {
                string text = GetTextTag(file, Consts.RIFF_ID_COMMENTS);

                if (!string.IsNullOrEmpty(text))
                    return text;
            }

            return null;
        }
    }
}
