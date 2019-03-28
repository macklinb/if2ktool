namespace if2ktool
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblErrorWaitTimeMs = new System.Windows.Forms.Label();
            this.txtErrorWaitTime = new System.Windows.Forms.TextBox();
            this.chkDelayStart = new System.Windows.Forms.CheckBox();
            this.cbErrorAction = new System.Windows.Forms.ComboBox();
            this.txtWarningWaitTime = new System.Windows.Forms.TextBox();
            this.cbWarningAction = new System.Windows.Forms.ComboBox();
            this.lblWarningWaitTimeMs = new System.Windows.Forms.Label();
            this.chkAllowEditRowHeight = new System.Windows.Forms.CheckBox();
            this.chkPropagateCheckEdits = new System.Windows.Forms.CheckBox();
            this.grpWorkers = new System.Windows.Forms.GroupBox();
            this.chkLockViewToSelection = new System.Windows.Forms.CheckBox();
            this.chkScrollWithSelection = new System.Windows.Forms.CheckBox();
            this.txtReportProgressInterval = new System.Windows.Forms.TextBox();
            this.lblProgressInterval = new System.Windows.Forms.Label();
            this.chkReportProgress = new System.Windows.Forms.CheckBox();
            this.lblOnWorkerWarning = new System.Windows.Forms.Label();
            this.lblOnWorkerError = new System.Windows.Forms.Label();
            this.lblLookupMinMatchPercent = new System.Windows.Forms.Label();
            this.lblLookupMinMatchPercentHeader = new System.Windows.Forms.Label();
            this.numFuzzyDistance = new System.Windows.Forms.NumericUpDown();
            this.chkAnyExt = new System.Windows.Forms.CheckBox();
            this.chkFuzzy = new System.Windows.Forms.CheckBox();
            this.sldrLookupMinScore = new System.Windows.Forms.TrackBar();
            this.grpMapping = new System.Windows.Forms.GroupBox();
            this.chkNormalizeStrings = new System.Windows.Forms.CheckBox();
            this.chkCheckForDuplicates = new System.Windows.Forms.CheckBox();
            this.grpLookup = new System.Windows.Forms.GroupBox();
            this.chkWarnImperfect = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkShowConsole = new System.Windows.Forms.CheckBox();
            this.chkFullLogging = new System.Windows.Forms.CheckBox();
            this.chkLogToFile = new System.Windows.Forms.CheckBox();
            this.grpLogging = new System.Windows.Forms.GroupBox();
            this.chkThreadedLogging = new System.Windows.Forms.CheckBox();
            this.grpForm = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.Tagging = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTagLibVersion = new System.Windows.Forms.Label();
            this.cbForceID3v2Encoding = new System.Windows.Forms.ComboBox();
            this.chkForceId3v2Encoding = new System.Windows.Forms.CheckBox();
            this.chkUseNumericGenresID3v2 = new System.Windows.Forms.CheckBox();
            this.chkRemoveID3v1 = new System.Windows.Forms.CheckBox();
            this.cbForceID3v2Version = new System.Windows.Forms.ComboBox();
            this.chkForceId3v2Version = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageWorkers = new System.Windows.Forms.TabPage();
            this.tabPageMapping = new System.Windows.Forms.TabPage();
            this.tabPageTagging = new System.Windows.Forms.TabPage();
            this.numMaxParallelThreads = new System.Windows.Forms.NumericUpDown();
            this.lblMaxParallelThreads = new System.Windows.Forms.Label();
            this.chkDryRun = new System.Windows.Forms.CheckBox();
            this.chkWriteInfoToWavFiles = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkDontPromptFuzzy = new System.Windows.Forms.CheckBox();
            this.grpWorkers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFuzzyDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sldrLookupMinScore)).BeginInit();
            this.grpMapping.SuspendLayout();
            this.grpLookup.SuspendLayout();
            this.grpLogging.SuspendLayout();
            this.grpForm.SuspendLayout();
            this.Tagging.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageWorkers.SuspendLayout();
            this.tabPageMapping.SuspendLayout();
            this.tabPageTagging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxParallelThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lblErrorWaitTimeMs
            // 
            this.lblErrorWaitTimeMs.AutoSize = true;
            this.lblErrorWaitTimeMs.Location = new System.Drawing.Point(157, 72);
            this.lblErrorWaitTimeMs.Name = "lblErrorWaitTimeMs";
            this.lblErrorWaitTimeMs.Size = new System.Drawing.Size(20, 13);
            this.lblErrorWaitTimeMs.TabIndex = 7;
            this.lblErrorWaitTimeMs.Text = "ms";
            // 
            // txtErrorWaitTime
            // 
            this.txtErrorWaitTime.Location = new System.Drawing.Point(115, 69);
            this.txtErrorWaitTime.Name = "txtErrorWaitTime";
            this.txtErrorWaitTime.Size = new System.Drawing.Size(39, 20);
            this.txtErrorWaitTime.TabIndex = 8;
            this.txtErrorWaitTime.TextChanged += new System.EventHandler(this.txtErrorWaitTime_TextChanged);
            // 
            // chkDelayStart
            // 
            this.chkDelayStart.AutoSize = true;
            this.chkDelayStart.Location = new System.Drawing.Point(13, 22);
            this.chkDelayStart.Name = "chkDelayStart";
            this.chkDelayStart.Size = new System.Drawing.Size(76, 17);
            this.chkDelayStart.TabIndex = 7;
            this.chkDelayStart.Text = "Delay start";
            this.chkDelayStart.UseVisualStyleBackColor = true;
            // 
            // cbErrorAction
            // 
            this.cbErrorAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbErrorAction.FormattingEnabled = true;
            this.cbErrorAction.Location = new System.Drawing.Point(37, 68);
            this.cbErrorAction.Name = "cbErrorAction";
            this.cbErrorAction.Size = new System.Drawing.Size(72, 21);
            this.cbErrorAction.TabIndex = 8;
            // 
            // txtWarningWaitTime
            // 
            this.txtWarningWaitTime.Location = new System.Drawing.Point(115, 119);
            this.txtWarningWaitTime.Name = "txtWarningWaitTime";
            this.txtWarningWaitTime.Size = new System.Drawing.Size(39, 20);
            this.txtWarningWaitTime.TabIndex = 11;
            this.txtWarningWaitTime.TextChanged += new System.EventHandler(this.txtWarningWaitTime_TextChanged);
            // 
            // cbWarningAction
            // 
            this.cbWarningAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWarningAction.FormattingEnabled = true;
            this.cbWarningAction.Location = new System.Drawing.Point(37, 119);
            this.cbWarningAction.Name = "cbWarningAction";
            this.cbWarningAction.Size = new System.Drawing.Size(72, 21);
            this.cbWarningAction.TabIndex = 10;
            // 
            // lblWarningWaitTimeMs
            // 
            this.lblWarningWaitTimeMs.AutoSize = true;
            this.lblWarningWaitTimeMs.Location = new System.Drawing.Point(157, 122);
            this.lblWarningWaitTimeMs.Name = "lblWarningWaitTimeMs";
            this.lblWarningWaitTimeMs.Size = new System.Drawing.Size(20, 13);
            this.lblWarningWaitTimeMs.TabIndex = 9;
            this.lblWarningWaitTimeMs.Text = "ms";
            // 
            // chkAllowEditRowHeight
            // 
            this.chkAllowEditRowHeight.AutoSize = true;
            this.chkAllowEditRowHeight.Location = new System.Drawing.Point(11, 46);
            this.chkAllowEditRowHeight.Name = "chkAllowEditRowHeight";
            this.chkAllowEditRowHeight.Size = new System.Drawing.Size(137, 17);
            this.chkAllowEditRowHeight.TabIndex = 1;
            this.chkAllowEditRowHeight.Text = "Allow editing row height";
            this.chkAllowEditRowHeight.UseVisualStyleBackColor = true;
            // 
            // chkPropagateCheckEdits
            // 
            this.chkPropagateCheckEdits.AutoSize = true;
            this.chkPropagateCheckEdits.Location = new System.Drawing.Point(11, 23);
            this.chkPropagateCheckEdits.Name = "chkPropagateCheckEdits";
            this.chkPropagateCheckEdits.Size = new System.Drawing.Size(196, 17);
            this.chkPropagateCheckEdits.TabIndex = 0;
            this.chkPropagateCheckEdits.Text = "Propagate check states to selection";
            this.chkPropagateCheckEdits.UseVisualStyleBackColor = true;
            // 
            // grpWorkers
            // 
            this.grpWorkers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpWorkers.Controls.Add(this.chkLockViewToSelection);
            this.grpWorkers.Controls.Add(this.chkScrollWithSelection);
            this.grpWorkers.Controls.Add(this.txtReportProgressInterval);
            this.grpWorkers.Controls.Add(this.lblProgressInterval);
            this.grpWorkers.Controls.Add(this.chkReportProgress);
            this.grpWorkers.Controls.Add(this.lblOnWorkerWarning);
            this.grpWorkers.Controls.Add(this.lblOnWorkerError);
            this.grpWorkers.Controls.Add(this.txtErrorWaitTime);
            this.grpWorkers.Controls.Add(this.txtWarningWaitTime);
            this.grpWorkers.Controls.Add(this.cbErrorAction);
            this.grpWorkers.Controls.Add(this.chkDelayStart);
            this.grpWorkers.Controls.Add(this.lblErrorWaitTimeMs);
            this.grpWorkers.Controls.Add(this.cbWarningAction);
            this.grpWorkers.Controls.Add(this.lblWarningWaitTimeMs);
            this.grpWorkers.Location = new System.Drawing.Point(6, 6);
            this.grpWorkers.Name = "grpWorkers";
            this.grpWorkers.Size = new System.Drawing.Size(279, 248);
            this.grpWorkers.TabIndex = 13;
            this.grpWorkers.TabStop = false;
            this.grpWorkers.Text = " Workers ";
            // 
            // chkLockViewToSelection
            // 
            this.chkLockViewToSelection.AutoSize = true;
            this.chkLockViewToSelection.Location = new System.Drawing.Point(13, 221);
            this.chkLockViewToSelection.Name = "chkLockViewToSelection";
            this.chkLockViewToSelection.Size = new System.Drawing.Size(132, 17);
            this.chkLockViewToSelection.TabIndex = 20;
            this.chkLockViewToSelection.Text = "Lock view to selection";
            this.chkLockViewToSelection.UseVisualStyleBackColor = true;
            // 
            // chkScrollWithSelection
            // 
            this.chkScrollWithSelection.AutoSize = true;
            this.chkScrollWithSelection.Location = new System.Drawing.Point(13, 198);
            this.chkScrollWithSelection.Name = "chkScrollWithSelection";
            this.chkScrollWithSelection.Size = new System.Drawing.Size(119, 17);
            this.chkScrollWithSelection.TabIndex = 19;
            this.chkScrollWithSelection.Text = "Scroll with selection";
            this.chkScrollWithSelection.UseVisualStyleBackColor = true;
            this.chkScrollWithSelection.CheckedChanged += new System.EventHandler(this.chkScrollWithSelection_CheckedChanged);
            // 
            // txtReportProgressInterval
            // 
            this.txtReportProgressInterval.Location = new System.Drawing.Point(111, 170);
            this.txtReportProgressInterval.Name = "txtReportProgressInterval";
            this.txtReportProgressInterval.Size = new System.Drawing.Size(39, 20);
            this.txtReportProgressInterval.TabIndex = 15;
            this.txtReportProgressInterval.TextChanged += new System.EventHandler(this.txtReportProgressInterval_TextChanged);
            // 
            // lblProgressInterval
            // 
            this.lblProgressInterval.AutoSize = true;
            this.lblProgressInterval.Location = new System.Drawing.Point(34, 173);
            this.lblProgressInterval.Name = "lblProgressInterval";
            this.lblProgressInterval.Size = new System.Drawing.Size(140, 13);
            this.lblProgressInterval.TabIndex = 18;
            this.lblProgressInterval.Text = "Report interval                 ms";
            // 
            // chkReportProgress
            // 
            this.chkReportProgress.AutoSize = true;
            this.chkReportProgress.Location = new System.Drawing.Point(13, 148);
            this.chkReportProgress.Name = "chkReportProgress";
            this.chkReportProgress.Size = new System.Drawing.Size(145, 17);
            this.chkReportProgress.TabIndex = 17;
            this.chkReportProgress.Text = "Worker reports progress?";
            this.chkReportProgress.UseVisualStyleBackColor = true;
            // 
            // lblOnWorkerWarning
            // 
            this.lblOnWorkerWarning.AutoSize = true;
            this.lblOnWorkerWarning.Location = new System.Drawing.Point(14, 97);
            this.lblOnWorkerWarning.Name = "lblOnWorkerWarning";
            this.lblOnWorkerWarning.Size = new System.Drawing.Size(99, 13);
            this.lblOnWorkerWarning.TabIndex = 13;
            this.lblOnWorkerWarning.Text = "On worker warning:";
            // 
            // lblOnWorkerError
            // 
            this.lblOnWorkerError.AutoSize = true;
            this.lblOnWorkerError.Location = new System.Drawing.Point(14, 46);
            this.lblOnWorkerError.Name = "lblOnWorkerError";
            this.lblOnWorkerError.Size = new System.Drawing.Size(83, 13);
            this.lblOnWorkerError.TabIndex = 12;
            this.lblOnWorkerError.Text = "On worker error:";
            // 
            // lblLookupMinMatchPercent
            // 
            this.lblLookupMinMatchPercent.Location = new System.Drawing.Point(169, 26);
            this.lblLookupMinMatchPercent.Name = "lblLookupMinMatchPercent";
            this.lblLookupMinMatchPercent.Size = new System.Drawing.Size(39, 13);
            this.lblLookupMinMatchPercent.TabIndex = 24;
            this.lblLookupMinMatchPercent.Text = "50%";
            this.lblLookupMinMatchPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLookupMinMatchPercentHeader
            // 
            this.lblLookupMinMatchPercentHeader.AutoSize = true;
            this.lblLookupMinMatchPercentHeader.Location = new System.Drawing.Point(17, 26);
            this.lblLookupMinMatchPercentHeader.Name = "lblLookupMinMatchPercentHeader";
            this.lblLookupMinMatchPercentHeader.Size = new System.Drawing.Size(146, 13);
            this.lblLookupMinMatchPercentHeader.TabIndex = 22;
            this.lblLookupMinMatchPercentHeader.Text = "Lookup minimum matching %:";
            // 
            // numFuzzyDistance
            // 
            this.numFuzzyDistance.Cursor = System.Windows.Forms.Cursors.Default;
            this.numFuzzyDistance.Location = new System.Drawing.Point(120, 44);
            this.numFuzzyDistance.Name = "numFuzzyDistance";
            this.numFuzzyDistance.Size = new System.Drawing.Size(40, 20);
            this.numFuzzyDistance.TabIndex = 20;
            this.numFuzzyDistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAnyExt
            // 
            this.chkAnyExt.AutoSize = true;
            this.chkAnyExt.Location = new System.Drawing.Point(13, 23);
            this.chkAnyExt.Name = "chkAnyExt";
            this.chkAnyExt.Size = new System.Drawing.Size(93, 17);
            this.chkAnyExt.TabIndex = 19;
            this.chkAnyExt.Text = "Any Extension";
            this.chkAnyExt.UseVisualStyleBackColor = true;
            // 
            // chkFuzzy
            // 
            this.chkFuzzy.AutoSize = true;
            this.chkFuzzy.Location = new System.Drawing.Point(13, 46);
            this.chkFuzzy.Name = "chkFuzzy";
            this.chkFuzzy.Size = new System.Drawing.Size(101, 17);
            this.chkFuzzy.TabIndex = 18;
            this.chkFuzzy.Text = "Fuzzy Distance:";
            this.chkFuzzy.UseVisualStyleBackColor = true;
            // 
            // sldrLookupMinScore
            // 
            this.sldrLookupMinScore.AutoSize = false;
            this.sldrLookupMinScore.BackColor = System.Drawing.SystemColors.Window;
            this.sldrLookupMinScore.LargeChange = 1;
            this.sldrLookupMinScore.Location = new System.Drawing.Point(17, 46);
            this.sldrLookupMinScore.Name = "sldrLookupMinScore";
            this.sldrLookupMinScore.Size = new System.Drawing.Size(191, 40);
            this.sldrLookupMinScore.TabIndex = 23;
            this.sldrLookupMinScore.Value = 5;
            this.sldrLookupMinScore.Scroll += new System.EventHandler(this.sldrLookupMinScore_Scroll);
            // 
            // grpMapping
            // 
            this.grpMapping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMapping.Controls.Add(this.chkDontPromptFuzzy);
            this.grpMapping.Controls.Add(this.chkNormalizeStrings);
            this.grpMapping.Controls.Add(this.chkCheckForDuplicates);
            this.grpMapping.Controls.Add(this.grpLookup);
            this.grpMapping.Controls.Add(this.chkAnyExt);
            this.grpMapping.Controls.Add(this.chkFuzzy);
            this.grpMapping.Controls.Add(this.numFuzzyDistance);
            this.grpMapping.Location = new System.Drawing.Point(6, 6);
            this.grpMapping.Name = "grpMapping";
            this.grpMapping.Size = new System.Drawing.Size(281, 240);
            this.grpMapping.TabIndex = 25;
            this.grpMapping.TabStop = false;
            this.grpMapping.Text = "Mapping";
            // 
            // chkNormalizeStrings
            // 
            this.chkNormalizeStrings.AutoSize = true;
            this.chkNormalizeStrings.Location = new System.Drawing.Point(13, 69);
            this.chkNormalizeStrings.Name = "chkNormalizeStrings";
            this.chkNormalizeStrings.Size = new System.Drawing.Size(105, 17);
            this.chkNormalizeStrings.TabIndex = 31;
            this.chkNormalizeStrings.Text = "Normalize strings";
            this.chkNormalizeStrings.UseVisualStyleBackColor = true;
            // 
            // chkCheckForDuplicates
            // 
            this.chkCheckForDuplicates.AutoSize = true;
            this.chkCheckForDuplicates.Location = new System.Drawing.Point(13, 92);
            this.chkCheckForDuplicates.Name = "chkCheckForDuplicates";
            this.chkCheckForDuplicates.Size = new System.Drawing.Size(123, 17);
            this.chkCheckForDuplicates.TabIndex = 30;
            this.chkCheckForDuplicates.Text = "Check for duplicates";
            this.chkCheckForDuplicates.UseVisualStyleBackColor = true;
            // 
            // grpLookup
            // 
            this.grpLookup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLookup.Controls.Add(this.chkWarnImperfect);
            this.grpLookup.Controls.Add(this.lblLookupMinMatchPercent);
            this.grpLookup.Controls.Add(this.sldrLookupMinScore);
            this.grpLookup.Controls.Add(this.lblLookupMinMatchPercentHeader);
            this.grpLookup.Location = new System.Drawing.Point(6, 115);
            this.grpLookup.Name = "grpLookup";
            this.grpLookup.Size = new System.Drawing.Size(269, 118);
            this.grpLookup.TabIndex = 29;
            this.grpLookup.TabStop = false;
            this.grpLookup.Text = "Lookup";
            // 
            // chkWarnImperfect
            // 
            this.chkWarnImperfect.AutoSize = true;
            this.chkWarnImperfect.Location = new System.Drawing.Point(20, 85);
            this.chkWarnImperfect.Name = "chkWarnImperfect";
            this.chkWarnImperfect.Size = new System.Drawing.Size(145, 17);
            this.chkWarnImperfect.TabIndex = 25;
            this.chkWarnImperfect.Text = "Warn on imperfect match";
            this.chkWarnImperfect.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(156, 299);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(67, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(229, 299);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 32000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // chkShowConsole
            // 
            this.chkShowConsole.AutoSize = true;
            this.chkShowConsole.Location = new System.Drawing.Point(13, 23);
            this.chkShowConsole.Name = "chkShowConsole";
            this.chkShowConsole.Size = new System.Drawing.Size(93, 17);
            this.chkShowConsole.TabIndex = 1;
            this.chkShowConsole.Text = "Show console";
            this.chkShowConsole.UseVisualStyleBackColor = true;
            // 
            // chkFullLogging
            // 
            this.chkFullLogging.AutoSize = true;
            this.chkFullLogging.Location = new System.Drawing.Point(112, 23);
            this.chkFullLogging.Name = "chkFullLogging";
            this.chkFullLogging.Size = new System.Drawing.Size(79, 17);
            this.chkFullLogging.TabIndex = 2;
            this.chkFullLogging.Text = "Full logging";
            this.chkFullLogging.UseVisualStyleBackColor = true;
            // 
            // chkLogToFile
            // 
            this.chkLogToFile.AutoSize = true;
            this.chkLogToFile.Location = new System.Drawing.Point(13, 46);
            this.chkLogToFile.Name = "chkLogToFile";
            this.chkLogToFile.Size = new System.Drawing.Size(72, 17);
            this.chkLogToFile.TabIndex = 3;
            this.chkLogToFile.Text = "Log to file";
            this.chkLogToFile.UseVisualStyleBackColor = true;
            // 
            // grpLogging
            // 
            this.grpLogging.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLogging.Controls.Add(this.chkThreadedLogging);
            this.grpLogging.Controls.Add(this.chkLogToFile);
            this.grpLogging.Controls.Add(this.chkFullLogging);
            this.grpLogging.Controls.Add(this.chkShowConsole);
            this.grpLogging.Location = new System.Drawing.Point(6, 6);
            this.grpLogging.Name = "grpLogging";
            this.grpLogging.Size = new System.Drawing.Size(281, 98);
            this.grpLogging.TabIndex = 12;
            this.grpLogging.TabStop = false;
            this.grpLogging.Text = "Logging";
            // 
            // chkThreadedLogging
            // 
            this.chkThreadedLogging.AutoSize = true;
            this.chkThreadedLogging.Location = new System.Drawing.Point(13, 69);
            this.chkThreadedLogging.Name = "chkThreadedLogging";
            this.chkThreadedLogging.Size = new System.Drawing.Size(132, 17);
            this.chkThreadedLogging.TabIndex = 4;
            this.chkThreadedLogging.Text = "Log in separate thread";
            this.chkThreadedLogging.UseVisualStyleBackColor = true;
            // 
            // grpForm
            // 
            this.grpForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpForm.Controls.Add(this.chkPropagateCheckEdits);
            this.grpForm.Controls.Add(this.chkAllowEditRowHeight);
            this.grpForm.Location = new System.Drawing.Point(6, 110);
            this.grpForm.Name = "grpForm";
            this.grpForm.Size = new System.Drawing.Size(281, 72);
            this.grpForm.TabIndex = 28;
            this.grpForm.TabStop = false;
            this.grpForm.Text = "Form";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(11, 299);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(106, 23);
            this.btnReset.TabIndex = 29;
            this.btnReset.Text = "Reset to defaults";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Tagging
            // 
            this.Tagging.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tagging.Controls.Add(this.label1);
            this.Tagging.Controls.Add(this.lblTagLibVersion);
            this.Tagging.Controls.Add(this.cbForceID3v2Encoding);
            this.Tagging.Controls.Add(this.chkForceId3v2Encoding);
            this.Tagging.Controls.Add(this.chkUseNumericGenresID3v2);
            this.Tagging.Controls.Add(this.chkRemoveID3v1);
            this.Tagging.Controls.Add(this.cbForceID3v2Version);
            this.Tagging.Controls.Add(this.chkForceId3v2Version);
            this.Tagging.Location = new System.Drawing.Point(6, 6);
            this.Tagging.Name = "Tagging";
            this.Tagging.Size = new System.Drawing.Size(281, 171);
            this.Tagging.TabIndex = 30;
            this.Tagging.TabStop = false;
            this.Tagging.Text = "TagLib";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "*Note that this only applies to new frames";
            // 
            // lblTagLibVersion
            // 
            this.lblTagLibVersion.AutoSize = true;
            this.lblTagLibVersion.Location = new System.Drawing.Point(10, 144);
            this.lblTagLibVersion.Name = "lblTagLibVersion";
            this.lblTagLibVersion.Size = new System.Drawing.Size(104, 13);
            this.lblTagLibVersion.TabIndex = 31;
            this.lblTagLibVersion.Text = "TagLib-Sharp v0.0.0";
            // 
            // cbForceID3v2Encoding
            // 
            this.cbForceID3v2Encoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForceID3v2Encoding.FormattingEnabled = true;
            this.cbForceID3v2Encoding.Location = new System.Drawing.Point(184, 45);
            this.cbForceID3v2Encoding.Name = "cbForceID3v2Encoding";
            this.cbForceID3v2Encoding.Size = new System.Drawing.Size(77, 21);
            this.cbForceID3v2Encoding.TabIndex = 34;
            // 
            // chkForceId3v2Encoding
            // 
            this.chkForceId3v2Encoding.AutoSize = true;
            this.chkForceId3v2Encoding.Location = new System.Drawing.Point(13, 49);
            this.chkForceId3v2Encoding.Name = "chkForceId3v2Encoding";
            this.chkForceId3v2Encoding.Size = new System.Drawing.Size(132, 17);
            this.chkForceId3v2Encoding.TabIndex = 33;
            this.chkForceId3v2Encoding.Text = "Force ID3v2 encoding";
            this.chkForceId3v2Encoding.UseVisualStyleBackColor = true;
            this.chkForceId3v2Encoding.CheckedChanged += new System.EventHandler(this.chkForceId3v2Encoding_CheckedChanged);
            // 
            // chkUseNumericGenresID3v2
            // 
            this.chkUseNumericGenresID3v2.AutoSize = true;
            this.chkUseNumericGenresID3v2.Location = new System.Drawing.Point(13, 115);
            this.chkUseNumericGenresID3v2.Name = "chkUseNumericGenresID3v2";
            this.chkUseNumericGenresID3v2.Size = new System.Drawing.Size(151, 17);
            this.chkUseNumericGenresID3v2.TabIndex = 32;
            this.chkUseNumericGenresID3v2.Text = "Use numeric genres in ID3";
            this.chkUseNumericGenresID3v2.UseVisualStyleBackColor = true;
            // 
            // chkRemoveID3v1
            // 
            this.chkRemoveID3v1.AutoSize = true;
            this.chkRemoveID3v1.Location = new System.Drawing.Point(13, 92);
            this.chkRemoveID3v1.Name = "chkRemoveID3v1";
            this.chkRemoveID3v1.Size = new System.Drawing.Size(134, 17);
            this.chkRemoveID3v1.TabIndex = 3;
            this.chkRemoveID3v1.Text = "Remove all ID3v1 tags";
            this.chkRemoveID3v1.UseVisualStyleBackColor = true;
            // 
            // cbForceID3v2Version
            // 
            this.cbForceID3v2Version.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForceID3v2Version.FormattingEnabled = true;
            this.cbForceID3v2Version.Location = new System.Drawing.Point(184, 19);
            this.cbForceID3v2Version.Name = "cbForceID3v2Version";
            this.cbForceID3v2Version.Size = new System.Drawing.Size(77, 21);
            this.cbForceID3v2Version.TabIndex = 1;
            // 
            // chkForceId3v2Version
            // 
            this.chkForceId3v2Version.AutoSize = true;
            this.chkForceId3v2Version.Location = new System.Drawing.Point(13, 23);
            this.chkForceId3v2Version.Name = "chkForceId3v2Version";
            this.chkForceId3v2Version.Size = new System.Drawing.Size(122, 17);
            this.chkForceId3v2Version.TabIndex = 0;
            this.chkForceId3v2Version.Text = "Force ID3v2 version";
            this.chkForceId3v2Version.UseVisualStyleBackColor = true;
            this.chkForceId3v2Version.CheckedChanged += new System.EventHandler(this.chkForceId3v2Version_CheckedChanged);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageWorkers);
            this.tabControl.Controls.Add(this.tabPageMapping);
            this.tabControl.Controls.Add(this.tabPageTagging);
            this.tabControl.Location = new System.Drawing.Point(5, 5);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(301, 287);
            this.tabControl.TabIndex = 31;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.grpLogging);
            this.tabPageGeneral.Controls.Add(this.grpForm);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(293, 261);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageWorkers
            // 
            this.tabPageWorkers.Controls.Add(this.grpWorkers);
            this.tabPageWorkers.Location = new System.Drawing.Point(4, 22);
            this.tabPageWorkers.Name = "tabPageWorkers";
            this.tabPageWorkers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWorkers.Size = new System.Drawing.Size(293, 261);
            this.tabPageWorkers.TabIndex = 1;
            this.tabPageWorkers.Text = "Workers";
            this.tabPageWorkers.UseVisualStyleBackColor = true;
            // 
            // tabPageMapping
            // 
            this.tabPageMapping.Controls.Add(this.grpMapping);
            this.tabPageMapping.Location = new System.Drawing.Point(4, 22);
            this.tabPageMapping.Name = "tabPageMapping";
            this.tabPageMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMapping.Size = new System.Drawing.Size(293, 261);
            this.tabPageMapping.TabIndex = 2;
            this.tabPageMapping.Text = "Mapping";
            this.tabPageMapping.UseVisualStyleBackColor = true;
            // 
            // tabPageTagging
            // 
            this.tabPageTagging.Controls.Add(this.numMaxParallelThreads);
            this.tabPageTagging.Controls.Add(this.lblMaxParallelThreads);
            this.tabPageTagging.Controls.Add(this.chkDryRun);
            this.tabPageTagging.Controls.Add(this.chkWriteInfoToWavFiles);
            this.tabPageTagging.Controls.Add(this.Tagging);
            this.tabPageTagging.Location = new System.Drawing.Point(4, 22);
            this.tabPageTagging.Name = "tabPageTagging";
            this.tabPageTagging.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTagging.Size = new System.Drawing.Size(293, 261);
            this.tabPageTagging.TabIndex = 3;
            this.tabPageTagging.Text = "Tagging";
            this.tabPageTagging.UseVisualStyleBackColor = true;
            // 
            // numMaxParallelThreads
            // 
            this.numMaxParallelThreads.Location = new System.Drawing.Point(190, 186);
            this.numMaxParallelThreads.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numMaxParallelThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxParallelThreads.Name = "numMaxParallelThreads";
            this.numMaxParallelThreads.Size = new System.Drawing.Size(44, 20);
            this.numMaxParallelThreads.TabIndex = 39;
            this.numMaxParallelThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblMaxParallelThreads
            // 
            this.lblMaxParallelThreads.AutoSize = true;
            this.lblMaxParallelThreads.Location = new System.Drawing.Point(16, 188);
            this.lblMaxParallelThreads.Name = "lblMaxParallelThreads";
            this.lblMaxParallelThreads.Size = new System.Drawing.Size(104, 13);
            this.lblMaxParallelThreads.TabIndex = 38;
            this.lblMaxParallelThreads.Text = "Max parallel threads:";
            // 
            // chkDryRun
            // 
            this.chkDryRun.AutoSize = true;
            this.chkDryRun.Location = new System.Drawing.Point(19, 210);
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.Size = new System.Drawing.Size(60, 17);
            this.chkDryRun.TabIndex = 32;
            this.chkDryRun.Text = "Dry run";
            this.chkDryRun.UseVisualStyleBackColor = true;
            // 
            // chkWriteInfoToWavFiles
            // 
            this.chkWriteInfoToWavFiles.AutoSize = true;
            this.chkWriteInfoToWavFiles.Location = new System.Drawing.Point(19, 233);
            this.chkWriteInfoToWavFiles.Name = "chkWriteInfoToWavFiles";
            this.chkWriteInfoToWavFiles.Size = new System.Drawing.Size(155, 17);
            this.chkWriteInfoToWavFiles.TabIndex = 31;
            this.chkWriteInfoToWavFiles.Text = "Write info tags to WAV files";
            this.chkWriteInfoToWavFiles.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // chkDontPromptFuzzy
            // 
            this.chkDontPromptFuzzy.AutoSize = true;
            this.chkDontPromptFuzzy.Location = new System.Drawing.Point(166, 46);
            this.chkDontPromptFuzzy.Name = "chkDontPromptFuzzy";
            this.chkDontPromptFuzzy.Size = new System.Drawing.Size(86, 17);
            this.chkDontPromptFuzzy.TabIndex = 32;
            this.chkDontPromptFuzzy.Text = "Don\'t prompt";
            this.chkDontPromptFuzzy.UseVisualStyleBackColor = true;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 333);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(283, 370);
            this.Name = "PreferencesForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.grpWorkers.ResumeLayout(false);
            this.grpWorkers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFuzzyDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sldrLookupMinScore)).EndInit();
            this.grpMapping.ResumeLayout(false);
            this.grpMapping.PerformLayout();
            this.grpLookup.ResumeLayout(false);
            this.grpLookup.PerformLayout();
            this.grpLogging.ResumeLayout(false);
            this.grpLogging.PerformLayout();
            this.grpForm.ResumeLayout(false);
            this.grpForm.PerformLayout();
            this.Tagging.ResumeLayout(false);
            this.Tagging.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageWorkers.ResumeLayout(false);
            this.tabPageMapping.ResumeLayout(false);
            this.tabPageTagging.ResumeLayout(false);
            this.tabPageTagging.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxParallelThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblErrorWaitTimeMs;
        private System.Windows.Forms.TextBox txtErrorWaitTime;
        private System.Windows.Forms.CheckBox chkDelayStart;
        private System.Windows.Forms.ComboBox cbErrorAction;
        private System.Windows.Forms.TextBox txtWarningWaitTime;
        private System.Windows.Forms.ComboBox cbWarningAction;
        private System.Windows.Forms.Label lblWarningWaitTimeMs;
        private System.Windows.Forms.GroupBox grpWorkers;
        private System.Windows.Forms.CheckBox chkPropagateCheckEdits;
        private System.Windows.Forms.CheckBox chkAllowEditRowHeight;
        private System.Windows.Forms.Label lblLookupMinMatchPercent;
        private System.Windows.Forms.Label lblLookupMinMatchPercentHeader;
        private System.Windows.Forms.NumericUpDown numFuzzyDistance;
        private System.Windows.Forms.CheckBox chkAnyExt;
        private System.Windows.Forms.CheckBox chkFuzzy;
        private System.Windows.Forms.TrackBar sldrLookupMinScore;
        private System.Windows.Forms.GroupBox grpMapping;
        private System.Windows.Forms.CheckBox chkWarnImperfect;
        private System.Windows.Forms.GroupBox grpLookup;
        private System.Windows.Forms.CheckBox chkCheckForDuplicates;
        private System.Windows.Forms.CheckBox chkNormalizeStrings;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblOnWorkerWarning;
        private System.Windows.Forms.Label lblOnWorkerError;
        private System.Windows.Forms.TextBox txtReportProgressInterval;
        private System.Windows.Forms.Label lblProgressInterval;
        private System.Windows.Forms.CheckBox chkReportProgress;
        private System.Windows.Forms.CheckBox chkScrollWithSelection;
        private System.Windows.Forms.CheckBox chkLockViewToSelection;
        private System.Windows.Forms.CheckBox chkShowConsole;
        private System.Windows.Forms.CheckBox chkFullLogging;
        private System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.GroupBox grpLogging;
        private System.Windows.Forms.GroupBox grpForm;
        private System.Windows.Forms.CheckBox chkThreadedLogging;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox Tagging;
        private System.Windows.Forms.ComboBox cbForceID3v2Version;
        private System.Windows.Forms.CheckBox chkForceId3v2Version;
        private System.Windows.Forms.CheckBox chkRemoveID3v1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageWorkers;
        private System.Windows.Forms.TabPage tabPageMapping;
        private System.Windows.Forms.TabPage tabPageTagging;
        private System.Windows.Forms.Label lblTagLibVersion;
        private System.Windows.Forms.CheckBox chkUseNumericGenresID3v2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox chkWriteInfoToWavFiles;
        private System.Windows.Forms.ComboBox cbForceID3v2Encoding;
        private System.Windows.Forms.CheckBox chkForceId3v2Encoding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDryRun;
        private System.Windows.Forms.NumericUpDown numMaxParallelThreads;
        private System.Windows.Forms.Label lblMaxParallelThreads;
        private System.Windows.Forms.CheckBox chkDontPromptFuzzy;
    }
}