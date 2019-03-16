namespace if2ktool
{
    partial class MatchManualPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchManualPrompt));
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblMissingPath = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkIgnoreAll = new System.Windows.Forms.CheckBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.chkPersistPath = new System.Windows.Forms.CheckBox();
            this.chkLookOther = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtFixedPath = new System.Windows.Forms.TextBox();
            this.grpFix = new System.Windows.Forms.GroupBox();
            this.grpFix.SuspendLayout();
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
            this.lblHeader.Size = new System.Drawing.Size(168, 52);
            this.lblHeader.TabIndex = 4;
            this.lblHeader.Text = "Could not find the file for the entry:\r\n\r\n\r\nat the path:";
            // 
            // lblMissingPath
            // 
            this.lblMissingPath.Location = new System.Drawing.Point(34, 75);
            this.lblMissingPath.Name = "lblMissingPath";
            this.lblMissingPath.Size = new System.Drawing.Size(301, 28);
            this.lblMissingPath.TabIndex = 6;
            this.lblMissingPath.Text = "Test";
            this.lblMissingPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirm.Location = new System.Drawing.Point(280, 291);
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
            this.btnAbort.Location = new System.Drawing.Point(12, 291);
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
            this.btnSkip.Location = new System.Drawing.Point(199, 291);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 18;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(18, 110);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(311, 26);
            this.lblDescription.TabIndex = 19;
            this.lblDescription.Text = "Please manually browse to the specified file, then click \"Confirm\"\r\nor click \"Ski" +
    "p\" to ignore and continue.";
            // 
            // chkIgnoreAll
            // 
            this.chkIgnoreAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreAll.AutoSize = true;
            this.chkIgnoreAll.Location = new System.Drawing.Point(87, 295);
            this.chkIgnoreAll.Name = "chkIgnoreAll";
            this.chkIgnoreAll.Size = new System.Drawing.Size(69, 17);
            this.chkIgnoreAll.TabIndex = 14;
            this.chkIgnoreAll.Text = "Ignore all";
            this.chkIgnoreAll.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.Location = new System.Drawing.Point(240, 61);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(83, 23);
            this.btnFind.TabIndex = 11;
            this.btnFind.Text = "Find file...";
            this.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // chkPersistPath
            // 
            this.chkPersistPath.AutoSize = true;
            this.chkPersistPath.Location = new System.Drawing.Point(25, 90);
            this.chkPersistPath.Name = "chkPersistPath";
            this.chkPersistPath.Size = new System.Drawing.Size(154, 30);
            this.chkPersistPath.TabIndex = 15;
            this.chkPersistPath.Text = "Open picker to this path for\r\nfuture prompts";
            this.chkPersistPath.UseVisualStyleBackColor = true;
            // 
            // chkLookOther
            // 
            this.chkLookOther.AutoSize = true;
            this.chkLookOther.Location = new System.Drawing.Point(25, 57);
            this.chkLookOther.Name = "chkLookOther";
            this.chkLookOther.Size = new System.Drawing.Size(184, 30);
            this.chkLookOther.TabIndex = 12;
            this.chkLookOther.Text = "Look in this directory (plus parent)\r\nfor other missing files in this album\r\n";
            this.chkLookOther.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(240, 90);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(83, 23);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtFixedPath
            // 
            this.txtFixedPath.Location = new System.Drawing.Point(13, 25);
            this.txtFixedPath.Name = "txtFixedPath";
            this.txtFixedPath.ReadOnly = true;
            this.txtFixedPath.Size = new System.Drawing.Size(314, 20);
            this.txtFixedPath.TabIndex = 19;
            // 
            // grpFix
            // 
            this.grpFix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFix.Controls.Add(this.txtFixedPath);
            this.grpFix.Controls.Add(this.btnClear);
            this.grpFix.Controls.Add(this.chkLookOther);
            this.grpFix.Controls.Add(this.chkPersistPath);
            this.grpFix.Controls.Add(this.btnFind);
            this.grpFix.Location = new System.Drawing.Point(12, 150);
            this.grpFix.Name = "grpFix";
            this.grpFix.Size = new System.Drawing.Size(343, 133);
            this.grpFix.TabIndex = 16;
            this.grpFix.TabStop = false;
            this.grpFix.Text = "Fix matched path";
            // 
            // MatchManualPrompt
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 326);
            this.Controls.Add(this.lblMissingPath);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.grpFix);
            this.Controls.Add(this.chkIgnoreAll);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchManualPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Track unmatched";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MatchManualPrompt_FormClosing);
            this.grpFix.ResumeLayout(false);
            this.grpFix.PerformLayout();
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
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkIgnoreAll;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.CheckBox chkPersistPath;
        private System.Windows.Forms.CheckBox chkLookOther;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtFixedPath;
        private System.Windows.Forms.GroupBox grpFix;
    }
}