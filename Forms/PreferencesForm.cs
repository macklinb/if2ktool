using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class PreferencesForm : Form
    {
        Main mainForm;
        
        public PreferencesForm()
        {
            mainForm = (Main)Application.OpenForms["Main"];
            InitializeComponent();

            EnumUtils.InitComboBoxWithEnum(cbErrorAction, typeof(WorkerPauseAction));
            EnumUtils.InitComboBoxWithEnum(cbWarningAction, typeof(WorkerPauseAction));
            EnumUtils.InitComboBoxWithEnum(cbForceID3v2Version, typeof(ID3v2Version));
            EnumUtils.InitComboBoxWithEnum(cbForceID3v2Encoding, typeof(TagLib.StringType));

            toolTip.SetToolTip(chkFullLogging, Consts.TOOLTIP_FULL_LOGGING);
            toolTip.SetToolTip(chkLogToFile, Consts.TOOLTIP_LOG_TO_FILE);
            toolTip.SetToolTip(chkThreadedLogging, Consts.TOOLTIP_THREADED_LOGGING);
            toolTip.SetToolTip(chkDelayStart, Consts.TOOLTIP_DELAY_START);
            toolTip.SetToolTip(lblOnWorkerError, Consts.TOOLTIP_WORKER_ON_ERROR);
            toolTip.SetToolTip(lblOnWorkerWarning, Consts.TOOLTIP_WORKER_ON_ERROR.Replace("error", "warning"));
            toolTip.SetToolTip(chkPropagateCheckEdits, Consts.TOOLTIP_PROPAGATE_CHECK_EDITS);
            toolTip.SetToolTip(chkAllowEditRowHeight, Consts.TOOLTIP_ALLOW_EDIT_ROW_HEIGHT);
            toolTip.SetToolTip(chkAnyExt, Consts.TOOLTIP_ANY_EXTENSION);
            toolTip.SetToolTip(chkFuzzy, Consts.TOOLTIP_FUZZY);
            toolTip.SetToolTip(chkNormalizeStrings, Consts.TOOLTIP_NORMALIZE_STRINGS);
            toolTip.SetToolTip(lblLookupMinMatchPercentHeader, Consts.TOOLTIP_LOOKUP_MIN_MATCH_PERCENTAGE);
            toolTip.SetToolTip(chkForceId3v2Version, Consts.TOOLTIP_FORCE_ID3v2_VERSION);
            toolTip.SetToolTip(chkForceId3v2Encoding, Consts.TOOLTIP_FORCE_ID3v2_ENCODING);
            toolTip.SetToolTip(chkRemoveID3v1, Consts.TOOLTIP_FORCE_REMOVE_ID3v1);
            toolTip.SetToolTip(chkUseNumericGenresID3v2, Consts.TOOLTIP_USE_NUMERIC_GENRES_ID3v2);
            toolTip.SetToolTip(lblMaxParallelThreads, Consts.TOOLTIP_MAX_PARALLEL_THREADS);
            toolTip.SetToolTip(chkDryRun, Consts.TOOLTIP_DRY_RUN);
            toolTip.SetToolTip(chkWriteInfoToWavFiles, Consts.TOOLTIP_WRITE_INFO_TO_WAV_FILES);

            errorProvider.SetIconPadding(txtReportProgressInterval, 25);
            errorProvider.SetIconPadding(txtErrorWaitTime, 25);
            errorProvider.SetIconPadding(txtWarningWaitTime, 25);

            InitControlsFromSettings(Settings.Current);

            // Load version number into lblTagLibVersion
            try
            {
                string version = System.Diagnostics.FileVersionInfo.GetVersionInfo("taglib-sharp.dll").FileVersion;
                lblTagLibVersion.Text = "TagLib-Sharp v" + version;
            }
            catch
            {
                lblTagLibVersion.Text = "";
            }
        }

        void InitControlsFromSettings(Settings settings)
        {
            chkShowConsole.Checked              = settings.showConsole;
            chkFullLogging.Checked              = settings.fullLogging;
            chkLogToFile.Checked                = settings.logToFile;
            chkThreadedLogging.Checked          = settings.threadedLogging;

            chkDelayStart.Checked               = settings.workerDelayStart;
            cbErrorAction.SelectedValue         = settings.workerErrorAction;
            cbWarningAction.SelectedValue       = settings.workerWarningAction;
            txtErrorWaitTime.Text               = settings.workerErrorWaitTime.ToString();
            txtWarningWaitTime.Text             = settings.workerWarningWaitTime.ToString();
            chkReportProgress.Checked           = settings.workerReportsProgress;
            txtReportProgressInterval.Text      = settings.workerReportProgressInterval.ToString();
            chkScrollWithSelection.Checked      = settings.workerScrollWithSelection;
            chkLockViewToSelection.Enabled      = chkScrollWithSelection.Checked;
            chkLockViewToSelection.Checked      = settings.workerLockViewToSelection;

            chkPropagateCheckEdits.Checked      = settings.mainPropagateCheckEdits;
            chkAllowEditRowHeight.Checked       = settings.mainAllowEditRowHeight;

            chkAnyExt.Checked                   = settings.matchingAnyExtension;
            chkFuzzy.Checked                    = settings.matchingFuzzy;
            numFuzzyDistance.Value              = settings.matchingFuzzyDistance;
            chkDontPromptFuzzy.Checked          = settings.matchingFuzzyDontPrompt;
            chkNormalizeStrings.Checked         = settings.matchingNormalize;
            chkCheckForDuplicates.Checked       = settings.matchingCheckForDupes;

            sldrLookupMinScore.Value            = (int)(10 * settings.lookupMinMatchingPercent);
            lblLookupMinMatchPercent.Text       = (sldrLookupMinScore.Value * 10) + "%";
            chkWarnImperfect.Checked            = settings.lookupWarnOnImperfect;

            chkForceId3v2Version.Checked        = settings.forceID3v2Version;
            cbForceID3v2Version.Enabled         = chkForceId3v2Version.Checked;
            cbForceID3v2Version.SelectedValue   = settings.forceID3v2VersionValue;
            chkForceId3v2Encoding.Checked       = settings.forceID3v2Encoding;
            cbForceID3v2Encoding.Enabled        = chkForceId3v2Encoding.Checked;
            cbForceID3v2Encoding.SelectedValue  = settings.forceID3v2EncodingValue;
            chkRemoveID3v1.Checked              = settings.removeID3v1;
            chkUseNumericGenresID3v2.Checked    = settings.useNumericGenresID3v2;

            numMaxParallelThreads.Value         = settings.maxParallelThreads;
            chkDryRun.Checked                   = settings.dryRun;
            chkWriteInfoToWavFiles.Checked      = settings.writeInfoToWavFiles;
        }

        // Writes the settings from the controls to Settings.Current
        bool SavePrefs()
        {
            Settings.Current.showConsole = chkShowConsole.Checked;
            Settings.Current.fullLogging = chkFullLogging.Checked;
            Settings.Current.logToFile = chkLogToFile.Checked;
            Settings.Current.threadedLogging = chkThreadedLogging.Checked;

            Settings.Current.workerDelayStart = chkDelayStart.Checked;
            Settings.Current.workerErrorAction = (WorkerPauseAction)cbErrorAction.SelectedValue;
            Settings.Current.workerWarningAction = (WorkerPauseAction)cbWarningAction.SelectedValue;

            if (!Int32.TryParse(txtErrorWaitTime.Text, out int errorWaitTime))
            {
                MessageBox.Show("\"" + txtErrorWaitTime.Text + "\" is not a number!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtErrorWaitTime.Clear();
                txtErrorWaitTime.Focus();
                return false;
            }
            else
                Settings.Current.workerErrorWaitTime = errorWaitTime;

            if (!Int32.TryParse(txtWarningWaitTime.Text, out int warningWaitTime))
            {
                MessageBox.Show("\"" + txtWarningWaitTime.Text + "\" is not a number!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWarningWaitTime.Clear();
                txtWarningWaitTime.Focus();
                return false;
            }
            else
                Settings.Current.workerWarningWaitTime = warningWaitTime;

            Settings.Current.workerReportsProgress = chkReportProgress.Checked;

            if (!Int32.TryParse(txtReportProgressInterval.Text, out int reportProgressInterval))
            {
                MessageBox.Show("\"" + txtReportProgressInterval.Text + "\" is not a number!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReportProgressInterval.Clear();
                txtReportProgressInterval.Focus();
                return false;
            }

            Settings.Current.workerReportProgressInterval = reportProgressInterval;

            Settings.Current.workerScrollWithSelection = chkScrollWithSelection.Checked;
            Settings.Current.workerLockViewToSelection = chkLockViewToSelection.Checked;

            Settings.Current.mainPropagateCheckEdits = chkPropagateCheckEdits.Checked;
            Settings.Current.mainAllowEditRowHeight = chkAllowEditRowHeight.Checked;

            Settings.Current.matchingAnyExtension = chkAnyExt.Checked;
            Settings.Current.matchingFuzzy = chkFuzzy.Checked;
            Settings.Current.matchingFuzzyDistance = Convert.ToInt32(numFuzzyDistance.Value);
            Settings.Current.matchingFuzzyDontPrompt = chkDontPromptFuzzy.Checked;
            Settings.Current.matchingNormalize = chkNormalizeStrings.Checked;
            Settings.Current.matchingCheckForDupes = chkCheckForDuplicates.Checked;

            Settings.Current.lookupMinMatchingPercent = sldrLookupMinScore.Value / 10f;
            Settings.Current.lookupWarnOnImperfect = chkWarnImperfect.Checked;

            Settings.Current.forceID3v2Version = chkForceId3v2Version.Checked;
            Settings.Current.forceID3v2VersionValue = (ID3v2Version)cbForceID3v2Version.SelectedValue;
            Settings.Current.forceID3v2Encoding = chkForceId3v2Encoding.Checked;
            Settings.Current.forceID3v2EncodingValue = (TagLib.StringType)cbForceID3v2Encoding.SelectedValue;
            Settings.Current.removeID3v1 = chkRemoveID3v1.Checked;
            Settings.Current.useNumericGenresID3v2 = chkUseNumericGenresID3v2.Checked;

            Settings.Current.maxParallelThreads = (int)numMaxParallelThreads.Value;
            Settings.Current.dryRun = chkDryRun.Checked;
            Settings.Current.writeInfoToWavFiles = chkWriteInfoToWavFiles.Checked;

            return true;
        }

        void ApplyPrefs()
        {
            // Apply settings changes
            ConsoleHelper.ToggleConsole(Settings.Current.showConsole);
            Debug.ToggleLogToFile(Settings.Current.logToFile);
            Debug.ToggleThreadedLogging(Settings.Current.threadedLogging);
            mainForm.LoadStatesFromSettings();
        }

        private void sldrLookupMinScore_Scroll(object sender, EventArgs e)
        {
            lblLookupMinMatchPercent.Text = (sldrLookupMinScore.Value * 10) + "%";
        }

        private void chkScrollWithSelection_CheckedChanged(object sender, EventArgs e)
        {
            chkLockViewToSelection.Enabled = chkScrollWithSelection.Checked;
        }
        
        private void chkForceId3v2Version_CheckedChanged(object sender, EventArgs e)
        {
            cbForceID3v2Version.Enabled = chkForceId3v2Version.Checked;
        }

        private void chkForceId3v2Encoding_CheckedChanged(object sender, EventArgs e)
        {
            cbForceID3v2Encoding.Enabled = chkForceId3v2Encoding.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SavePrefs())
            {
                ApplyPrefs();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // This doesn't immediately reset the settings, but instead just loads the default values into the form controls. The user can then choose to save or cancel normally
            InitControlsFromSettings(new Settings());
        }

        private void txtErrorWaitTime_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtErrorWaitTime.Text, out int errorWaitTime))
                errorProvider.SetError(txtErrorWaitTime, "Value entered is not a number! Please enter a valid integer number");
            else
                errorProvider.SetError(txtErrorWaitTime, string.Empty);
        }

        private void txtWarningWaitTime_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtWarningWaitTime.Text, out int warningWaitTime))
                errorProvider.SetError(txtWarningWaitTime, "Value entered is not a number! Please enter a valid integer number");
            else
                errorProvider.SetError(txtWarningWaitTime, string.Empty);
        }

        private void txtReportProgressInterval_TextChanged(object sender, EventArgs e)
        {
            // Ensure that the value entered is a number, and that it is less than 50
            if (!Int32.TryParse(txtReportProgressInterval.Text, out int reportProgressInterval))
                errorProvider.SetError(txtReportProgressInterval, "Value entered is not a number! Please enter a valid integer number");
            else if (reportProgressInterval < 50)
                errorProvider.SetError(txtReportProgressInterval, "This value should be at least 50 ms, any lower would introduce lag or even lock the main interface while processing items.");
            else
                errorProvider.SetError(txtReportProgressInterval, string.Empty);
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                string text = toolTip.GetToolTip(e.AssociatedControl);
                
                var sizef = graphics.MeasureString(text, SystemFonts.DefaultFont, new SizeF(350f, 0), StringFormat.GenericDefault, out int charactersFitted, out int linesFitted);
                int width = (int)sizef.Width;
                var size = new Size(width, 3 + (15 * linesFitted));
                e.ToolTipSize = size;

                Debug.Log(sizef.ToString() + ", " + size.ToString() + ", charsFitted = " + charactersFitted + ", linesFitted = " + linesFitted);
            }
        }
    }
}
