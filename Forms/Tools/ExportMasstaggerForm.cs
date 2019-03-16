using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class ExportMasstaggerForm : Form
    {
        Main mainForm;

        public ExportMasstaggerForm()
        {
            mainForm = (Main)Application.OpenForms["Main"];

            InitializeComponent();

            EnumUtils.InitComboBoxWithEnum(cbFilter, typeof(EntryFilter));
            EnumUtils.InitComboBoxWithEnum(cbSortOrder, typeof(ExportSortOrder));
            mainForm.dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;
        }

        // UpdateFilesLabel when the dgvEntries selection changes
        private void dgvEntries_SelectionChanged(object sender, EventArgs e)
        {
            if (Main.shouldInhibitDataGridViewSelectionEvent)
                return;

            UpdateEntriesLabel();
        }

        // UpdateFilesLabel when the cbFilter changes
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEntriesLabel();
        }

        private void chkFilterNot_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntriesLabel();
        }

        private void UpdateEntriesLabel()
        {
            int count = mainForm.GetEntriesCount((EntryFilter)cbFilter.SelectedValue, chkFilterNot.Checked);
            lblCount.Text = count + " entries";
        }

        private void cbSortOrder_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbSortOrder.SelectedValue != null && (ExportSortOrder)cbSortOrder.SelectedValue == ExportSortOrder.LookupSortOrder &&
                !mainForm.GetEntries(EntryFilter.AllEntries).Any(entry => entry.lookupIndex != -1))
            {
                MessageBox.Show("You have picked the sort order \"Custom\", but no sort order has been loaded!\n\nPlease generate a JSON export from foo_texttools, then map the iTunes XML to it via the \"Lookup\" mode of the mapping tool. See the readme for more information.", "No sort order loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.Filter = "Masstagger scripts|*.mts|Text files|*.txt|All files|*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;

                if (File.Exists(path))
                {
                    if (MessageBox.Show(Path.GetFileName(path) + "already exists.\nDo you want to replace it?", "Confirm export", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                       return;
                }

                CreateMasstaggerScript(saveFileDialog.FileName);
            }
        }

        private void CreateMasstaggerScript(string filePath)
        {
            bool filterNot = chkFilterNot.Checked;

            // Get skip bools
            bool skipDateAdded = chkSkipDateAdded.Checked;
            bool skipLastPlayed = chkSkipLastPlayed.Checked;
            bool skipPlayCount = chkSkipPlayCount.Checked;
            bool skipRating = chkSkipRating.Checked;

            // Debugging
            bool noHeader = chkNoHeader.Checked;
            bool addSortField = chkAddSortField.Checked;
            bool debugSortOrder = chkDebugSortOrder.Checked;
            bool appendUnderscore = chkAppendUnderscore.Checked;
            
            // Get entries from rows, ordering by the track title
            var sortOrder = (ExportSortOrder)cbSortOrder.SelectedValue;
            var entries = mainForm.GetRows((EntryFilter)cbFilter.SelectedValue, filterNot).Select(x => (Entry)x.DataBoundItem);

            // Sort the entries
            switch (sortOrder)
            {
                case ExportSortOrder.TrackTitle:
                    entries = entries.OrderBy(e => e.trackTitle); break;
                case ExportSortOrder.FilePath:
                    entries = entries.OrderBy(e => e.location.Substring(Main.libraryXmlIsMacForm ? 16 : 17).Replace('/', '\\')); break;
                case ExportSortOrder.FileName:
                    entries = entries.OrderBy(e => e.fileName); break;
                case ExportSortOrder.FileSizeBytes:
                    entries = entries.OrderBy(e => e.fileSize); break;
                case ExportSortOrder.LookupSortOrder:
                {
                    // Warn the user that a sort order hasn't been loaded
                    if (!entries.Any(e => e.lookupIndex != -1))
                    {
                        MessageBox.Show("No sort order has been loaded! Please generate a foobar2000 export from foo_texttools, and load it using the Mapping tool's \"Lookup\" mode. See the GitHub page for more information).", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Order by lookupIndex and discard entries that do not have a lookupIndex (or the lookupIndex is negative and invalid)
                    entries = entries.OrderBy(e => e.lookupIndex).SkipWhile(e => e.lookupIndex < 0);

                    break;
                }
            }

            var sb = new StringBuilder();

            // Append a string of the data for this entry, in the form "dateadded;lastplayed;playcount;rating\r\n"
            void AppendEntryData(Entry e)
            {
                if (addSortField)
                {
                    switch (sortOrder)
                    {
                        case ExportSortOrder.TrackTitle:
                            sb.AppendLine("Track Title: " + e.trackTitle); break;
                        case ExportSortOrder.FilePath:
                            sb.AppendLine("File Path: " + e.location.Substring(Main.libraryXmlIsMacForm ? 16 : 17).Replace('/', '\\')); break;
                        case ExportSortOrder.FileName:
                            sb.AppendLine("File Name: " + e.fileName); break;
                        case ExportSortOrder.FileSizeBytes:
                            sb.AppendLine("File Size: " + e.fileSize.ToString()); break;
                        case ExportSortOrder.LookupSortOrder:
                            sb.AppendLine("Index: " + e.lookupIndex); break;
                    }
                }


                if (!debugSortOrder)
                {
                    // Date Played
                    if (!skipDateAdded && e.dateAdded != DateTime.MinValue)
                        sb.Append(e.dateAdded.ToFileTime());
                    sb.Append(";");

                    // Last Played
                    if (!skipLastPlayed && e.lastPlayed != DateTime.MinValue)
                        sb.Append(e.lastPlayed.ToFileTime());
                    sb.Append(";");

                    // Play count
                    if (!skipPlayCount && e.playCount != 0)
                        sb.Append(e.playCount);
                    sb.Append(";");

                    // Rating
                    if (!skipRating && e.rating != Rating.Unrated)
                        sb.Append((int)e.rating);
                }

                // debugSortOrder writes the track title instead of any relevent data, can be used to compare the track name in foobar with the exported track name. This is used to ensure that the sort order is valid
                else
                {
                    sb.Append(e.trackTitle + (appendUnderscore ? "_" : ""));
                }

                sb.Append("\r\n");
            }


            int offset = 0;

            // Loop over every (sorted) entry, writing a line containing the associated data for this track
            foreach (var e in entries)
            {
                // For ExportSortOrder.Custom, we have to add empty lines as to offset the data so that each line coincides with the lookupIndex - which is the expected position of each row of data in masstagger
                if (sortOrder == ExportSortOrder.LookupSortOrder)
                {
                    // The entries should be sorted by lookupIndex, so we should be okay just incrementing the offset to match, adding empty lines as we go
                    if (offset < e.lookupIndex)
                    {
                        while (offset != e.lookupIndex)
                        {
                            sb.AppendLine(addSortField ? "Index: " + offset : null);
                            offset++;
                        }
                    }

                    // This should never happen, but just check in case
                    else if (offset > e.lookupIndex)
                    {
                        Debug.LogError("Entry " + e.fileName + " is out of sequence! Cancelling operation", true);
                        return;
                    }
                }

                // Append the entry data at the current line
                AppendEntryData(e);

                offset++;
            }
            
            byte[] header = Consts.MASSTAGGER_HEADER;
            byte[] scheme = debugSortOrder ? Consts.MASSTAGGER_SCHEME_DEBUG : Consts.MASSTAGGER_SCHEME;
            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());

            // Resulting bytes which will be written to the file
            byte[] bytes = new byte[header.Length + scheme.Length + data.Length];

            // Length of scheme + data in bytes. We insert this into the header's last 4 bytes
            byte[] dataLength = BitConverter.GetBytes(scheme.Length + data.Length);
            System.Buffer.BlockCopy(dataLength, 0, header, header.Length - 4, 4);

            // BlockCopy the header, scheme and data into the final bytes array
            System.Buffer.BlockCopy(header, 0, bytes, 0, header.Length);
            System.Buffer.BlockCopy(scheme, 0, bytes, header.Length, scheme.Length);
            System.Buffer.BlockCopy(data, 0, bytes, header.Length + scheme.Length, data.Length);

            try
            {
                // Write the bytes to file
                File.WriteAllBytes(filePath, noHeader ? data : bytes);

                Debug.Log("Wrote " + bytes.Length + " bytes to " + filePath);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("An error occurred\n\n" + e.Message, true);
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }

            // Open in explorer
            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + filePath + "\"");
        }
    }
}
