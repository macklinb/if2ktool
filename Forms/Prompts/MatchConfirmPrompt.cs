using System;
using System.IO;
using System.Windows.Forms;

namespace if2ktool
{
    // This form is used so that the user can manually match an entry to a file. It also appears when the matchingworker needs to confirm a fuzzily-matched file.
    public partial class MatchConfirmPrompt : Form
    {
        // Set to true if the user requested to always do this action for fuzzy matches
        public bool alwaysAcceptFuzzyMatch { get; private set; }
        public bool alwaysIgnoreFuzzyMatch { get; private set; }

        public MatchConfirmPrompt(string missingFilePath, string fuzzyMatchPath)
        {
            InitializeComponent();

            lblFileName.Text = Path.GetFileName(missingFilePath);
            lblMissingPath.Text = missingFilePath;
            lblFuzzyPath.Text = fuzzyMatchPath;
            
            cbAutomaticFuzzy.SelectedIndex = 0;
            cbAutomaticFuzzy.Enabled = false;
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
            this.Close();
        }

        private void MatchManualPrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.alwaysAcceptFuzzyMatch = chkAutomaticFuzzy.Checked && cbAutomaticFuzzy.SelectedItem.ToString() == "Accept";
            this.alwaysIgnoreFuzzyMatch = chkAutomaticFuzzy.Checked && cbAutomaticFuzzy.SelectedItem.ToString() == "Ignore";
        }

        private void chkAutomaticFuzzy_CheckedChanged(object sender, EventArgs e)
        {
            cbAutomaticFuzzy.Enabled = chkAutomaticFuzzy.Checked;
        }
    }
}
