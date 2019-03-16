namespace if2ktool
{
    partial class MappingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MappingForm));
            this.rbDirect = new System.Windows.Forms.RadioButton();
            this.rbMapped = new System.Windows.Forms.RadioButton();
            this.rbLookup = new System.Windows.Forms.RadioButton();
            this.rbSearch = new System.Windows.Forms.RadioButton();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.lblAboutLookup = new System.Windows.Forms.LinkLabel();
            this.txtLibraryPath = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOpenPreferences = new System.Windows.Forms.Button();
            this.cbSearchMode = new System.Windows.Forms.ComboBox();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.lblSearchMode = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblEntryCount = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkEntryFilterNot = new System.Windows.Forms.CheckBox();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.grpOptions.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbDirect
            // 
            this.rbDirect.AutoSize = true;
            this.rbDirect.Location = new System.Drawing.Point(32, 27);
            this.rbDirect.Name = "rbDirect";
            this.rbDirect.Size = new System.Drawing.Size(53, 17);
            this.rbDirect.TabIndex = 0;
            this.rbDirect.TabStop = true;
            this.rbDirect.Text = "Direct";
            this.rbDirect.UseVisualStyleBackColor = true;
            this.rbDirect.CheckedChanged += new System.EventHandler(this.rbDirect_CheckedChanged);
            // 
            // rbMapped
            // 
            this.rbMapped.AutoSize = true;
            this.rbMapped.Location = new System.Drawing.Point(91, 27);
            this.rbMapped.Name = "rbMapped";
            this.rbMapped.Size = new System.Drawing.Size(64, 17);
            this.rbMapped.TabIndex = 1;
            this.rbMapped.TabStop = true;
            this.rbMapped.Text = "Mapped";
            this.rbMapped.UseVisualStyleBackColor = true;
            this.rbMapped.CheckedChanged += new System.EventHandler(this.rbMapped_CheckedChanged);
            // 
            // rbLookup
            // 
            this.rbLookup.AutoSize = true;
            this.rbLookup.Location = new System.Drawing.Point(161, 27);
            this.rbLookup.Name = "rbLookup";
            this.rbLookup.Size = new System.Drawing.Size(61, 17);
            this.rbLookup.TabIndex = 2;
            this.rbLookup.TabStop = true;
            this.rbLookup.Text = "Lookup";
            this.rbLookup.UseVisualStyleBackColor = true;
            this.rbLookup.CheckedChanged += new System.EventHandler(this.rbLookup_CheckedChanged);
            // 
            // rbSearch
            // 
            this.rbSearch.AutoSize = true;
            this.rbSearch.Enabled = false;
            this.rbSearch.Location = new System.Drawing.Point(226, 27);
            this.rbSearch.Name = "rbSearch";
            this.rbSearch.Size = new System.Drawing.Size(59, 17);
            this.rbSearch.TabIndex = 3;
            this.rbSearch.TabStop = true;
            this.rbSearch.Text = "Search";
            this.rbSearch.UseVisualStyleBackColor = true;
            this.rbSearch.CheckedChanged += new System.EventHandler(this.rbSearch_CheckedChanged);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.lblAboutLookup);
            this.grpOptions.Controls.Add(this.txtLibraryPath);
            this.grpOptions.Controls.Add(this.lblPath);
            this.grpOptions.Controls.Add(this.btnBrowse);
            this.grpOptions.Location = new System.Drawing.Point(12, 183);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(313, 61);
            this.grpOptions.TabIndex = 4;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // lblAboutLookup
            // 
            this.lblAboutLookup.AutoSize = true;
            this.lblAboutLookup.Location = new System.Drawing.Point(294, 28);
            this.lblAboutLookup.Name = "lblAboutLookup";
            this.lblAboutLookup.Size = new System.Drawing.Size(13, 13);
            this.lblAboutLookup.TabIndex = 12;
            this.lblAboutLookup.TabStop = true;
            this.lblAboutLookup.Text = "?";
            this.lblAboutLookup.Visible = false;
            this.lblAboutLookup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAboutLookup_LinkClicked);
            // 
            // txtLibraryPath
            // 
            this.txtLibraryPath.Location = new System.Drawing.Point(83, 25);
            this.txtLibraryPath.Name = "txtLibraryPath";
            this.txtLibraryPath.Size = new System.Drawing.Size(179, 20);
            this.txtLibraryPath.TabIndex = 5;
            this.txtLibraryPath.MouseEnter += new System.EventHandler(this.txtLibraryPath_MouseEnter);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(11, 28);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(66, 13);
            this.lblPath.TabIndex = 6;
            this.lblPath.Text = "Library Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(268, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 24);
            this.btnBrowse.TabIndex = 11;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOpenPreferences
            // 
            this.btnOpenPreferences.Location = new System.Drawing.Point(12, 250);
            this.btnOpenPreferences.Name = "btnOpenPreferences";
            this.btnOpenPreferences.Size = new System.Drawing.Size(101, 23);
            this.btnOpenPreferences.TabIndex = 12;
            this.btnOpenPreferences.Text = "Open preferences";
            this.btnOpenPreferences.UseVisualStyleBackColor = true;
            this.btnOpenPreferences.Click += new System.EventHandler(this.btnOpenPreferences_Click);
            // 
            // cbSearchMode
            // 
            this.cbSearchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchMode.FormattingEnabled = true;
            this.cbSearchMode.Location = new System.Drawing.Point(126, 60);
            this.cbSearchMode.Name = "cbSearchMode";
            this.cbSearchMode.Size = new System.Drawing.Size(130, 21);
            this.cbSearchMode.TabIndex = 12;
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rbSearch);
            this.grpMode.Controls.Add(this.rbDirect);
            this.grpMode.Controls.Add(this.rbMapped);
            this.grpMode.Controls.Add(this.rbLookup);
            this.grpMode.Controls.Add(this.lblSearchMode);
            this.grpMode.Controls.Add(this.cbSearchMode);
            this.grpMode.Location = new System.Drawing.Point(12, 76);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(313, 101);
            this.grpMode.TabIndex = 5;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Mode";
            // 
            // lblSearchMode
            // 
            this.lblSearchMode.AutoSize = true;
            this.lblSearchMode.Location = new System.Drawing.Point(47, 63);
            this.lblSearchMode.Name = "lblSearchMode";
            this.lblSearchMode.Size = new System.Drawing.Size(73, 13);
            this.lblSearchMode.TabIndex = 10;
            this.lblSearchMode.Text = "Search mode:";
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(14, 23);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(206, 21);
            this.cbFilter.TabIndex = 7;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbEntryFilter_SelectedIndexChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(224, 250);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblEntryCount
            // 
            this.lblEntryCount.Location = new System.Drawing.Point(113, 255);
            this.lblEntryCount.Name = "lblEntryCount";
            this.lblEntryCount.Size = new System.Drawing.Size(101, 13);
            this.lblEntryCount.TabIndex = 9;
            this.lblEntryCount.Text = "0 entries";
            this.lblEntryCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 32000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // chkEntryFilterNot
            // 
            this.chkEntryFilterNot.AutoSize = true;
            this.chkEntryFilterNot.Location = new System.Drawing.Point(243, 25);
            this.chkEntryFilterNot.Name = "chkEntryFilterNot";
            this.chkEntryFilterNot.Size = new System.Drawing.Size(49, 17);
            this.chkEntryFilterNot.TabIndex = 10;
            this.chkEntryFilterNot.Text = "NOT";
            this.chkEntryFilterNot.UseVisualStyleBackColor = true;
            this.chkEntryFilterNot.CheckedChanged += new System.EventHandler(this.chkEntryFilterNot_CheckedChanged);
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.cbFilter);
            this.grpFilter.Controls.Add(this.chkEntryFilterNot);
            this.grpFilter.Location = new System.Drawing.Point(12, 12);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(313, 58);
            this.grpFilter.TabIndex = 11;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter";
            // 
            // MappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 286);
            this.Controls.Add(this.btnOpenPreferences);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.lblEntryCount);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MappingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map XML entries to files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MappingForm_FormClosing);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbDirect;
        private System.Windows.Forms.RadioButton rbMapped;
        private System.Windows.Forms.RadioButton rbLookup;
        private System.Windows.Forms.RadioButton rbSearch;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.TextBox txtLibraryPath;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label lblEntryCount;
        private System.Windows.Forms.ComboBox cbSearchMode;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblSearchMode;
        private System.Windows.Forms.CheckBox chkEntryFilterNot;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.Button btnOpenPreferences;
        private System.Windows.Forms.LinkLabel lblAboutLookup;
    }
}