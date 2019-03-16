using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace if2ktool
{
    public enum SearchMode
    {
        [Description("Search all")]
        SearchAll,

        [Description("Search endpoint")]
        SearchEndpoint,

        [Description("Search front-to-back")]
        SearchFrontToBack,

        [Description("Search back-to-front")]
        SearchBackToFront
    }

    public partial class MappingForm : Form
    {
        // These static values are used to save the form state for this session
        public static EntryFilter lastEntryFilter;
        public static bool lastEntryFilterNot;
        public static MappingMode lastMappingMode;
        
        MappingMode mode;

        Main mainForm;

        public MappingForm()
        {
            mainForm = (Main)Application.OpenForms["Main"];

            InitializeComponent();

            // Subscribe to dgvEntries.SelectionChanged
            mainForm.dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;
            mainForm.MarkedChanged += dgvEntries_MarkedChanged;

            // Set up tooltips
            toolTip.SetToolTip(rbDirect, Consts.TOOLTIP_MATCH_MODE_DIRECT);
            toolTip.SetToolTip(rbMapped, Consts.TOOLTIP_MATCH_MODE_MAPPED);
            toolTip.SetToolTip(rbLookup, Consts.TOOLTIP_MATCH_MODE_LOOKUP);
            toolTip.SetToolTip(rbSearch, Consts.TOOLTIP_MATCH_MODE_SEARCH);
            toolTip.SetToolTip(cbSearchMode, Consts.TOOLTIP_SEARCHMODE);

            // Init combo boxes
            EnumUtils.InitComboBoxWithEnum(cbFilter, typeof(EntryFilter));
            EnumUtils.InitComboBoxWithEnum(cbSearchMode, typeof(SearchMode));

            // Restore last state (only persistent over a single session)
            switch (lastMappingMode)
            {
                case MappingMode.Direct: rbDirect.Checked = true; break;
                case MappingMode.Mapped: rbMapped.Checked = true; break;
                case MappingMode.Lookup: rbLookup.Checked = true; break;
                case MappingMode.Search: rbSearch.Checked = true; break;
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

        private void MappingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveState();
        }

        // Save current state (only persistent over a single session)
        void SaveState()
        {
            lastMappingMode = mode;
            lastEntryFilter = (EntryFilter)cbFilter.SelectedValue;
            lastEntryFilterNot = chkEntryFilterNot.Checked;
        }

        private void dgvEntries_MarkedChanged(object sender, Main.MarkedChangedEventArgs e)
        {
            if ((EntryFilter)cbFilter.SelectedValue == EntryFilter.Marked)
                UpdateEntriesLabel();
        }

        // Update the selection count (by calling this cbSelectionMode)
        private void dgvEntries_SelectionChanged(object sender, EventArgs e)
        {
            if (Main.shouldInhibitDataGridViewSelectionEvent)
                return;

            cbEntryFilter_SelectedIndexChanged(this, e);
        }

        private void rbDirect_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDirect.Checked == true)
            {
                ResetFields(true);

                mode = MappingMode.Direct;
                lblPath.Text = "Library Path";
                txtLibraryPath.Enabled = false;
                btnBrowse.Enabled = false;
                cbSearchMode.Enabled = false;

                txtLibraryPath.Text = Main.sourceLibraryFolderPath;
            }
        }

        private void rbMapped_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMapped.Checked == true)
            {
                ResetFields(true);

                mode = MappingMode.Mapped;
                lblPath.Text = "Library Path";
                cbSearchMode.Enabled = false;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.lastLibraryFolderPath))
                    txtLibraryPath.Text = Properties.Settings.Default.lastLibraryFolderPath;
            }
        }

        private void rbLookup_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLookup.Checked == true)
            {
                ResetFields(false);

                mode = MappingMode.Lookup;
                lblPath.Text = "f2k JSON file";
                txtLibraryPath.Enabled = true;
                btnBrowse.Enabled = true;
                lblAboutLookup.Visible = true;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.lastLookupJsonPath))
                    txtLibraryPath.Text = Properties.Settings.Default.lastLookupJsonPath;
            }
            else
                lblAboutLookup.Visible = false;
        }

        private void rbSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSearch.Checked == true)
            {
                rbMapped_CheckedChanged(sender, e);
                cbSearchMode.Enabled = true;
                mode = MappingMode.Search;
            }
        }

        private void cbEntryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEntriesLabel();
        }

        private void chkEntryFilterNot_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntriesLabel();
        }

        private void UpdateEntriesLabel()
        {
            int count = mainForm.GetEntriesCount((EntryFilter)cbFilter.SelectedValue, chkEntryFilterNot.Checked);
            lblEntryCount.Text = count + " entries";
        }

        private void ResetFields(bool state)
        {
            mode = MappingMode.Lookup;
            lblPath.Text = string.Empty;
            txtLibraryPath.Enabled = state;
            txtLibraryPath.Clear();
            btnBrowse.Enabled = state;
            cbSearchMode.Enabled = state;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (mode == MappingMode.Lookup)
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON files|*.json|All files|*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtLibraryPath.Text = openFileDialog.FileName;
                    Properties.Settings.Default.lastLookupJsonPath = openFileDialog.FileName;
                }
            }
            else if (mode == MappingMode.Mapped)
            {
                // Create new CommonOpenFileDialog (the default FolderBrowserDialog is poop)
                var openFolderDialog = new CommonOpenFileDialog();
                openFolderDialog.IsFolderPicker = true;

                string initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

                if (!string.IsNullOrEmpty(Properties.Settings.Default.lastLibraryFolderPath))
                    initialDir = Properties.Settings.Default.lastLibraryFolderPath;

                if (Directory.Exists(initialDir))
                    openFolderDialog.InitialDirectory = initialDir;

                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    // Return focus to this window (as the dialog returns focus to Main)
                    this.Focus();

                    // Set source directory
                    txtLibraryPath.Text = openFolderDialog.FileName;

                    // Set lastPath in settings
                    Properties.Settings.Default.lastLibraryFolderPath = openFolderDialog.FileName;
                }
            }
        }

        // Button to go to preferences window
        private void btnOpenPreferences_Click(object sender, EventArgs e)
        {
            new PreferencesForm().ShowDialog();
        }

        // Save all form controls to variables for the Main form to access
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Ensure that other mapping or writing progress aren't already under way
            if (MappingWorker.InProgress || TagWriterWorker.InProgress)
            {
                Debug.LogError("Cannot start mapping while another operation is in progress", true);
                return;
            }

            if (mode == MappingMode.Mapped && (string.IsNullOrEmpty(txtLibraryPath.Text) || !Directory.Exists(txtLibraryPath.Text)))
            {
                MessageBox.Show("Path empty or invalid! Please enter a valid directory path!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (mode == MappingMode.Lookup && (string.IsNullOrEmpty(txtLibraryPath.Text) || !File.Exists(txtLibraryPath.Text)))
            {
                MessageBox.Show("Path empty or invalid! Please enter a valid file path!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var entryFilter = (EntryFilter)cbFilter.SelectedValue;
            bool entryFilterNot = chkEntryFilterNot.Checked;

            // Get subset of entries based on the selectionMode (instead of individually discounting every entry that doesn't meet criteria)
            var rows = mainForm.GetRows(entryFilter, entryFilterNot);
            
            var args = new MappingWorker.MatchingWorkerArgs
            {
                mappingMode         = this.mode,
                filter              = entryFilter,
                libraryPath         = txtLibraryPath.Text,
                rows                = rows
            };

            // Call MatchEntries from here, then close form
            MappingWorker.StartWorker(args);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lblAboutLookup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new GenerateLookupJson().ShowDialog();
        }

        private void txtLibraryPath_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtLibraryPath, txtLibraryPath.Text);
        }
    }
}
