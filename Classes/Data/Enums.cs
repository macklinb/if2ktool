using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace if2ktool
{
    public enum EntryFilter
    {
        [Description("All entries")]
        AllEntries,

        [Description("Selected rows")]
        Selection,

        [Description("Marked rows")]
        Marked,

        [Description("Mapped entries")]
        Mapped,

        [Description("Saved entries")]
        Saved
    }

    public enum MappingMode
    {
        Direct,
        Mapped,
        Lookup,
        Search
    }

    public enum ExportSortOrder
    {
        [Description("Lookup sort order")]
        LookupSortOrder,

        [Description("Track title")]
        TrackTitle,

        [Description("File path")]
        FilePath,

        [Description("File name (with ext)")]
        FileName,

        [Description("File size (bytes)")]
        FileSizeBytes
    }

    public enum Rating : byte
    {
        [Description("-")] Unrated = 0,
        [Description("1")] Rating_1 = 1,
        [Description("2")] Rating_2 = 2,
        [Description("3")] Rating_3 = 3,
        [Description("4")] Rating_4 = 4,
        [Description("5")] Rating_5 = 5
    }
    
    public enum WorkerPauseAction
    {
        None,
        Wait,
        Prompt
    }

    public enum ID3v2Version
    {
        // Don't force
        [Description("None")]
        None,

        [Description("ID3v2.2")]
        ID3v2_2 = 2,

        [Description("ID3v2.3")]
        ID3v2_3 = 3,

        [Description("ID3v2.3")]
        ID3v2_4 = 4,
    }

    public static class EnumUtils
    {
        // Set DataSource on a ComboBox to be the values of an enumeration, with the DescriptionAttribute as the DisplayMember
        public static void InitComboBoxWithEnum(ComboBox comboBox, Type type)
        {
            // If any of the enums don't have a DescriptionAttribute, fall back to using ToString()
            if (Enum.GetValues(type).Cast<Enum>().Any(e => Attribute.GetCustomAttribute(e.GetType().GetField(e.ToString()), typeof(DescriptionAttribute)) == null))
            {
                comboBox.DisplayMember = "Text";
                comboBox.ValueMember = "Value";
                comboBox.DataSource = Enum.GetValues(type)
                .Cast<Enum>()
                .Select(e => new
                {
                    Text = e.ToString(),
                    Value = e
                })
                .OrderBy(e => e.Value)
                .ToList();
            }
            else
            {
                comboBox.DisplayMember = "Description";
                comboBox.ValueMember = "Value";
                comboBox.DataSource = Enum.GetValues(type)
                .Cast<Enum>()
                .Select(value => new
                {
                    Description = ((DescriptionAttribute)Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))).Description,
                    Value = value
                })
                .OrderBy(item => item.Value)
                .ToList();
            }
        }
    }
}
