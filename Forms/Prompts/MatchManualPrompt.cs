using System;
using System.IO;
using System.Windows.Forms;

namespace if2ktool
{
    // This form is used so that the user can manually match an entry to a file. It also appears when the matchingworker needs to confirm a fuzzily-matched file.
    public partial class MatchManualPrompt : Form
    {
        // The initial directory to open the file picker to
        public static string initialDirectory;

        // Set to the path that either was unchanged from the automated match, or fixed manually by the user
        public string fixedPath { get; private set; }

        // Set to true if the user allows the matching worker to look in this directory for other unmapped files that have the same endpoint directory
        public bool lookOther { get; private set; }

        // Set to true to not prompt for missing files in this session
        public bool ignoreAll { get; private set; }

        private string missingFilePath;

        public MatchManualPrompt(string missingFilePath, string initialFilePath = "")
        {
            InitializeComponent();

            this.missingFilePath = missingFilePath;

            if (MatchManualPrompt.initialDirectory != null)
                chkPersistPath.CheckState = CheckState.Indeterminate;

            // Set some labels
            lblFileName.Text = Path.GetFileName(missingFilePath);
            lblMissingPath.Text = missingFilePath;
            lblMissingPath.Visible = true;

            // Disable the confirm button until the user has matched the file
            btnConfirm.Enabled = false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string initialDirectory = MatchManualPrompt.initialDirectory;

            // If the text box isn't empty, set the initial directory to whatever it contains
            if (!string.IsNullOrEmpty(txtFixedPath.Text))
                initialDirectory = txtFixedPath.Text;

            // Otherwise, if is IS empty, and we have a saved initialDirectory, use that
            else if (!string.IsNullOrEmpty(MatchManualPrompt.initialDirectory))
                initialDirectory = MatchManualPrompt.initialDirectory;

            // Otherwise, get the initial directory from the missing file path
            else
                initialDirectory = missingFilePath;
            
            // Get the existing endpoint directory furthest to the end
            while (!Directory.Exists(initialDirectory) && !string.IsNullOrEmpty(initialDirectory))
            {
                initialDirectory = Path.GetDirectoryName(initialDirectory);
            }

            // Open a file picker to the sourceMatchedPath
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Select \"" + lblFileName.Text + "\""
            };

            // Set the initial directory if we found one
            if (!string.IsNullOrEmpty(initialDirectory))
                openFileDialog.InitialDirectory = initialDirectory;

            // Set the fixedPath variable
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fixedPath = txtFixedPath.Text = openFileDialog.FileName;
                btnConfirm.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            fixedPath = null;
            txtFixedPath.Text = "";
            btnConfirm.Enabled = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            fixedPath = null;
            this.Close();
        }

        private void MatchManualPrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.lookOther = chkLookOther.Checked;
            this.ignoreAll = chkIgnoreAll.Checked;

            // Set initialDirectory if checked
            if (chkPersistPath.CheckState == CheckState.Checked && fixedPath != null)
                initialDirectory = Path.GetDirectoryName(fixedPath);

            // Clear initialDirectory if unchecked
            else if (chkPersistPath.CheckState == CheckState.Unchecked)
                initialDirectory = null;
        }
    }
}
