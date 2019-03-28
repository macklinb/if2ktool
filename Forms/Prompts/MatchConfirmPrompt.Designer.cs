namespace if2ktool
{
    partial class MatchConfirmPrompt
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblMissingPath = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.chkAutomaticFuzzy = new System.Windows.Forms.CheckBox();
            this.cbAutomaticFuzzy = new System.Windows.Forms.ComboBox();
            this.lblFuzzyPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(34, 38);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(112, 13);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "Artist - Album - 01 Title";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(18, 18);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(294, 156);
            this.lblHeader.TabIndex = 4;
            this.lblHeader.Text = "Could not find the file for the entry:\r\n\r\n\r\nat the path:\r\n\r\n\r\n\r\nHowever, we found" +
    " an approximate/fuzzy match:\r\n\r\n\r\n\r\nClick \"Confirm\" to use this match, or click " +
    "\"Skip\" to continue.";
            // 
            // lblMissingPath
            // 
            this.lblMissingPath.Location = new System.Drawing.Point(34, 72);
            this.lblMissingPath.Name = "lblMissingPath";
            this.lblMissingPath.Size = new System.Drawing.Size(300, 36);
            this.lblMissingPath.TabIndex = 6;
            this.lblMissingPath.Text = "Test\r\n";
            this.lblMissingPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirm.Location = new System.Drawing.Point(280, 223);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 9;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(21, 223);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(69, 23);
            this.btnAbort.TabIndex = 17;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnSkip.Location = new System.Drawing.Point(199, 223);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 18;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // chkAutomaticFuzzy
            // 
            this.chkAutomaticFuzzy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutomaticFuzzy.AutoSize = true;
            this.chkAutomaticFuzzy.Location = new System.Drawing.Point(21, 189);
            this.chkAutomaticFuzzy.Name = "chkAutomaticFuzzy";
            this.chkAutomaticFuzzy.Size = new System.Drawing.Size(276, 17);
            this.chkAutomaticFuzzy.TabIndex = 20;
            this.chkAutomaticFuzzy.Text = "Automatically                          all future fuzzy matches";
            this.chkAutomaticFuzzy.UseVisualStyleBackColor = true;
            this.chkAutomaticFuzzy.CheckedChanged += new System.EventHandler(this.chkAutomaticFuzzy_CheckedChanged);
            // 
            // cbAutomaticFuzzy
            // 
            this.cbAutomaticFuzzy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutomaticFuzzy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutomaticFuzzy.FormattingEnabled = true;
            this.cbAutomaticFuzzy.Items.AddRange(new object[] {
            "Accept",
            "Ignore"});
            this.cbAutomaticFuzzy.Location = new System.Drawing.Point(109, 187);
            this.cbAutomaticFuzzy.Name = "cbAutomaticFuzzy";
            this.cbAutomaticFuzzy.Size = new System.Drawing.Size(63, 21);
            this.cbAutomaticFuzzy.TabIndex = 21;
            // 
            // lblFuzzyPath
            // 
            this.lblFuzzyPath.Location = new System.Drawing.Point(34, 125);
            this.lblFuzzyPath.Name = "lblFuzzyPath";
            this.lblFuzzyPath.Size = new System.Drawing.Size(300, 36);
            this.lblFuzzyPath.TabIndex = 22;
            this.lblFuzzyPath.Text = "Test\r\n";
            this.lblFuzzyPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MatchConfirmPrompt
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 259);
            this.Controls.Add(this.lblFuzzyPath);
            this.Controls.Add(this.lblMissingPath);
            this.Controls.Add(this.cbAutomaticFuzzy);
            this.Controls.Add(this.chkAutomaticFuzzy);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchConfirmPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Track unmatched";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MatchManualPrompt_FormClosing);
            this.Load += new System.EventHandler(this.MatchConfirmPrompt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblMissingPath;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.CheckBox chkAutomaticFuzzy;
        private System.Windows.Forms.ComboBox cbAutomaticFuzzy;
        private System.Windows.Forms.Label lblFuzzyPath;
    }
}