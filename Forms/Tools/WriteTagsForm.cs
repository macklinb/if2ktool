using System;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class WriteTagsForm : Form
    {
        Main mainForm;
        bool removeTags = false;

        static EntryFilter lastEntryFilter;
        static bool lastEntryFilterNot;

        public WriteTagsForm(bool removeTags = false)
        {
            mainForm = (Main)Application.OpenForms["Main"];

            InitializeComponent();
            this.removeTags = removeTags;

            EnumUtils.InitComboBoxWithEnum(cbFilter, typeof(EntryFilter));
            mainForm.dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;

            if (removeTags)
            {
                this.Text = "Remove tags";
            }

            // Show a message if the user has no mapped entries
            if (mainForm.GetEntriesCount(EntryFilter.Mapped) == 0)
            {
                MessageBox.Show("No entries have been mapped to their associated files! Following through with this operation will have no effect.\n\n See \"Tools -> Map entries to files\" for more", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Set EntryFilter to Selection if we have a selection of more than one
            if (mainForm.GetEntriesCount(EntryFilter.Selection) > 1)
                cbFilter.SelectedValue = EntryFilter.Selection;

            // Set EntryFilter to marker if we have a limited amount of marked rows
            else if (mainForm.GetEntriesCount(EntryFilter.Marked) != mainForm.GetEntriesCount(EntryFilter.AllEntries))
                cbFilter.SelectedValue = EntryFilter.Marked;

            // Use whatever was selected last
            else
            {
                cbFilter.SelectedValue = lastEntryFilter;
                chkEntryFilterNot.Checked = lastEntryFilterNot;
            }
        }
        
        // UpdateFilesLabel when the dgvEntries selection changes
        private void dgvEntries_SelectionChanged(object sender, EventArgs e)
        {
            if (Main.shouldInhibitDataGridViewSelectionEvent)
                return;

            UpdateFilesLabel();
        }

        // UpdateFilesLabel when the cbFilter changes
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilesLabel();
        }

        // UpdateFilesLabel when chkEntryFilterNot changes
        private void chkEntryFilterNot_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilesLabel();
        }

        private void UpdateFilesLabel()
        {
            int count = mainForm.GetEntriesCount((EntryFilter)cbFilter.SelectedValue, chkEntryFilterNot.Checked);
            lblFileCount.Text = count + " files";
        }

        private void btnWriteTags_Click(object sender, EventArgs e)
        {
            // Ensure that other mapping or writing progress aren't already under way
            if (MappingWorker.InProgress || TagWriterWorker.InProgress)
            {
                Debug.LogError("Cannot write tags while another operation is in progress", true);
                return;
            }

            var result = MessageBox.Show(Consts.WRITE_TAGS_WARNING, "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                var entryFilter = (EntryFilter)cbFilter.SelectedValue;
                bool filterNot = chkEntryFilterNot.Checked;

                var rows = mainForm.GetRows(entryFilter, filterNot);

                var args = new TagWriterWorker.TagWriterWorkerArgs
                {
                    removeTags = removeTags,
                    filter = entryFilter,
                    filterNot = filterNot,
                    skipDateAdded = chkSkipDateAdded.Checked,
                    skipLastPlayed = chkSkipLastPlayed.Checked,
                    skipPlayCount = chkSkipPlayCount.Checked,
                    skipRating = chkSkipRating.Checked,
                    rows = rows
                };

                TagWriterWorker.StartWorker(args);
                this.DialogResult = DialogResult.OK;
            }
            
            this.Close();
        }

        private void WriteTagsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            lastEntryFilter = (EntryFilter)cbFilter.SelectedValue;
            lastEntryFilterNot = chkEntryFilterNot.Checked;
        }
    }
}
