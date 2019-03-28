namespace if2ktool
{
    partial class MatchLookupMultiple
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpTags = new System.Windows.Forms.GroupBox();
            this.lblFileNameEquals = new System.Windows.Forms.Label();
            this.lblTrackNumEquals = new System.Windows.Forms.Label();
            this.lblAlbumEquals = new System.Windows.Forms.Label();
            this.lblArtistEquals = new System.Windows.Forms.Label();
            this.lblTitleEquals = new System.Windows.Forms.Label();
            this.lblFileNameCandidate = new System.Windows.Forms.Label();
            this.lblFileNameSource = new System.Windows.Forms.Label();
            this.lblFileNameHeader = new System.Windows.Forms.Label();
            this.lblTrackNumCandidate = new System.Windows.Forms.Label();
            this.lblTrackNumSource = new System.Windows.Forms.Label();
            this.lblTrackNumHeader = new System.Windows.Forms.Label();
            this.lblAlbumCandidate = new System.Windows.Forms.Label();
            this.lblAlbumSource = new System.Windows.Forms.Label();
            this.lblAlbumHeader = new System.Windows.Forms.Label();
            this.lblArtistCandidate = new System.Windows.Forms.Label();
            this.lblArtistSource = new System.Windows.Forms.Label();
            this.lblArtistHeader = new System.Windows.Forms.Label();
            this.lblTitleCandidate = new System.Windows.Forms.Label();
            this.lblTitleSource = new System.Windows.Forms.Label();
            this.lblTitleHeader = new System.Windows.Forms.Label();
            this.lstCandidates = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.chkHideMatched = new System.Windows.Forms.CheckBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.cbIgnoreMode = new System.Windows.Forms.ComboBox();
            this.grpTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(214, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(338, 46);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Found more than one possible match for the following entry. Please select the cor" +
    "rect item then click \"OK\", or click \"Ignore\" to skip over this entry.";
            // 
            // grpTags
            // 
            this.grpTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTags.Controls.Add(this.lblFileNameEquals);
            this.grpTags.Controls.Add(this.lblTrackNumEquals);
            this.grpTags.Controls.Add(this.lblAlbumEquals);
            this.grpTags.Controls.Add(this.lblArtistEquals);
            this.grpTags.Controls.Add(this.lblTitleEquals);
            this.grpTags.Controls.Add(this.lblFileNameCandidate);
            this.grpTags.Controls.Add(this.lblFileNameSource);
            this.grpTags.Controls.Add(this.lblFileNameHeader);
            this.grpTags.Controls.Add(this.lblTrackNumCandidate);
            this.grpTags.Controls.Add(this.lblTrackNumSource);
            this.grpTags.Controls.Add(this.lblTrackNumHeader);
            this.grpTags.Controls.Add(this.lblAlbumCandidate);
            this.grpTags.Controls.Add(this.lblAlbumSource);
            this.grpTags.Controls.Add(this.lblAlbumHeader);
            this.grpTags.Controls.Add(this.lblArtistCandidate);
            this.grpTags.Controls.Add(this.lblArtistSource);
            this.grpTags.Controls.Add(this.lblArtistHeader);
            this.grpTags.Controls.Add(this.lblTitleCandidate);
            this.grpTags.Controls.Add(this.lblTitleSource);
            this.grpTags.Controls.Add(this.lblTitleHeader);
            this.grpTags.Location = new System.Drawing.Point(214, 59);
            this.grpTags.Name = "grpTags";
            this.grpTags.Size = new System.Drawing.Size(334, 216);
            this.grpTags.TabIndex = 1;
            this.grpTags.TabStop = false;
            this.grpTags.Text = "Tags";
            // 
            // lblFileNameEquals
            // 
            this.lblFileNameEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileNameEquals.Location = new System.Drawing.Point(294, 169);
            this.lblFileNameEquals.Name = "lblFileNameEquals";
            this.lblFileNameEquals.Size = new System.Drawing.Size(24, 30);
            this.lblFileNameEquals.TabIndex = 31;
            this.lblFileNameEquals.Text = "✓";
            this.lblFileNameEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrackNumEquals
            // 
            this.lblTrackNumEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrackNumEquals.Location = new System.Drawing.Point(294, 133);
            this.lblTrackNumEquals.Name = "lblTrackNumEquals";
            this.lblTrackNumEquals.Size = new System.Drawing.Size(24, 30);
            this.lblTrackNumEquals.TabIndex = 30;
            this.lblTrackNumEquals.Text = "✓";
            this.lblTrackNumEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAlbumEquals
            // 
            this.lblAlbumEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlbumEquals.Location = new System.Drawing.Point(294, 97);
            this.lblAlbumEquals.Name = "lblAlbumEquals";
            this.lblAlbumEquals.Size = new System.Drawing.Size(24, 30);
            this.lblAlbumEquals.TabIndex = 29;
            this.lblAlbumEquals.Text = "✗";
            this.lblAlbumEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblArtistEquals
            // 
            this.lblArtistEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtistEquals.Location = new System.Drawing.Point(294, 61);
            this.lblArtistEquals.Name = "lblArtistEquals";
            this.lblArtistEquals.Size = new System.Drawing.Size(24, 30);
            this.lblArtistEquals.TabIndex = 28;
            this.lblArtistEquals.Text = "✗";
            this.lblArtistEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleEquals
            // 
            this.lblTitleEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleEquals.Location = new System.Drawing.Point(294, 25);
            this.lblTitleEquals.Name = "lblTitleEquals";
            this.lblTitleEquals.Size = new System.Drawing.Size(24, 30);
            this.lblTitleEquals.TabIndex = 27;
            this.lblTitleEquals.Text = "✗";
            this.lblTitleEquals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFileNameCandidate
            // 
            this.lblFileNameCandidate.Location = new System.Drawing.Point(88, 186);
            this.lblFileNameCandidate.Name = "lblFileNameCandidate";
            this.lblFileNameCandidate.Size = new System.Drawing.Size(200, 13);
            this.lblFileNameCandidate.TabIndex = 23;
            this.lblFileNameCandidate.Text = "-";
            this.lblFileNameCandidate.UseMnemonic = false;
            // 
            // lblFileNameSource
            // 
            this.lblFileNameSource.Location = new System.Drawing.Point(88, 169);
            this.lblFileNameSource.Name = "lblFileNameSource";
            this.lblFileNameSource.Size = new System.Drawing.Size(200, 13);
            this.lblFileNameSource.TabIndex = 22;
            this.lblFileNameSource.Text = "Source title";
            this.lblFileNameSource.UseMnemonic = false;
            // 
            // lblFileNameHeader
            // 
            this.lblFileNameHeader.AutoSize = true;
            this.lblFileNameHeader.Location = new System.Drawing.Point(17, 169);
            this.lblFileNameHeader.Name = "lblFileNameHeader";
            this.lblFileNameHeader.Size = new System.Drawing.Size(57, 13);
            this.lblFileNameHeader.TabIndex = 21;
            this.lblFileNameHeader.Text = "File Name:";
            // 
            // lblTrackNumCandidate
            // 
            this.lblTrackNumCandidate.Location = new System.Drawing.Point(88, 150);
            this.lblTrackNumCandidate.Name = "lblTrackNumCandidate";
            this.lblTrackNumCandidate.Size = new System.Drawing.Size(200, 13);
            this.lblTrackNumCandidate.TabIndex = 20;
            this.lblTrackNumCandidate.Text = "-";
            this.lblTrackNumCandidate.UseMnemonic = false;
            // 
            // lblTrackNumSource
            // 
            this.lblTrackNumSource.Location = new System.Drawing.Point(88, 133);
            this.lblTrackNumSource.Name = "lblTrackNumSource";
            this.lblTrackNumSource.Size = new System.Drawing.Size(200, 13);
            this.lblTrackNumSource.TabIndex = 19;
            this.lblTrackNumSource.Text = "Source title";
            this.lblTrackNumSource.UseMnemonic = false;
            // 
            // lblTrackNumHeader
            // 
            this.lblTrackNumHeader.AutoSize = true;
            this.lblTrackNumHeader.Location = new System.Drawing.Point(17, 133);
            this.lblTrackNumHeader.Name = "lblTrackNumHeader";
            this.lblTrackNumHeader.Size = new System.Drawing.Size(17, 13);
            this.lblTrackNumHeader.TabIndex = 18;
            this.lblTrackNumHeader.Text = "#:";
            // 
            // lblAlbumCandidate
            // 
            this.lblAlbumCandidate.Location = new System.Drawing.Point(88, 114);
            this.lblAlbumCandidate.Name = "lblAlbumCandidate";
            this.lblAlbumCandidate.Size = new System.Drawing.Size(200, 13);
            this.lblAlbumCandidate.TabIndex = 17;
            this.lblAlbumCandidate.Text = "-";
            this.lblAlbumCandidate.UseMnemonic = false;
            // 
            // lblAlbumSource
            // 
            this.lblAlbumSource.Location = new System.Drawing.Point(88, 97);
            this.lblAlbumSource.Name = "lblAlbumSource";
            this.lblAlbumSource.Size = new System.Drawing.Size(200, 13);
            this.lblAlbumSource.TabIndex = 16;
            this.lblAlbumSource.Text = "Source title";
            this.lblAlbumSource.UseMnemonic = false;
            // 
            // lblAlbumHeader
            // 
            this.lblAlbumHeader.AutoSize = true;
            this.lblAlbumHeader.Location = new System.Drawing.Point(17, 97);
            this.lblAlbumHeader.Name = "lblAlbumHeader";
            this.lblAlbumHeader.Size = new System.Drawing.Size(36, 13);
            this.lblAlbumHeader.TabIndex = 15;
            this.lblAlbumHeader.Text = "Album";
            // 
            // lblArtistCandidate
            // 
            this.lblArtistCandidate.Location = new System.Drawing.Point(88, 78);
            this.lblArtistCandidate.Name = "lblArtistCandidate";
            this.lblArtistCandidate.Size = new System.Drawing.Size(200, 13);
            this.lblArtistCandidate.TabIndex = 14;
            this.lblArtistCandidate.Text = "-";
            this.lblArtistCandidate.UseMnemonic = false;
            // 
            // lblArtistSource
            // 
            this.lblArtistSource.Location = new System.Drawing.Point(88, 61);
            this.lblArtistSource.Name = "lblArtistSource";
            this.lblArtistSource.Size = new System.Drawing.Size(200, 13);
            this.lblArtistSource.TabIndex = 13;
            this.lblArtistSource.Text = "Source title";
            this.lblArtistSource.UseMnemonic = false;
            // 
            // lblArtistHeader
            // 
            this.lblArtistHeader.AutoSize = true;
            this.lblArtistHeader.Location = new System.Drawing.Point(17, 61);
            this.lblArtistHeader.Name = "lblArtistHeader";
            this.lblArtistHeader.Size = new System.Drawing.Size(33, 13);
            this.lblArtistHeader.TabIndex = 12;
            this.lblArtistHeader.Text = "Artist:\r\n";
            // 
            // lblTitleCandidate
            // 
            this.lblTitleCandidate.Location = new System.Drawing.Point(88, 42);
            this.lblTitleCandidate.Name = "lblTitleCandidate";
            this.lblTitleCandidate.Size = new System.Drawing.Size(200, 13);
            this.lblTitleCandidate.TabIndex = 11;
            this.lblTitleCandidate.Text = "-";
            this.lblTitleCandidate.UseMnemonic = false;
            // 
            // lblTitleSource
            // 
            this.lblTitleSource.Location = new System.Drawing.Point(88, 25);
            this.lblTitleSource.Name = "lblTitleSource";
            this.lblTitleSource.Size = new System.Drawing.Size(200, 13);
            this.lblTitleSource.TabIndex = 6;
            this.lblTitleSource.Text = "Source title";
            this.lblTitleSource.UseMnemonic = false;
            // 
            // lblTitleHeader
            // 
            this.lblTitleHeader.AutoSize = true;
            this.lblTitleHeader.Location = new System.Drawing.Point(17, 25);
            this.lblTitleHeader.Name = "lblTitleHeader";
            this.lblTitleHeader.Size = new System.Drawing.Size(30, 13);
            this.lblTitleHeader.TabIndex = 0;
            this.lblTitleHeader.Text = "Title:";
            // 
            // lstCandidates
            // 
            this.lstCandidates.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstCandidates.Location = new System.Drawing.Point(12, 9);
            this.lstCandidates.Name = "lstCandidates";
            this.lstCandidates.Size = new System.Drawing.Size(196, 264);
            this.lstCandidates.TabIndex = 2;
            this.lstCandidates.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstCandidates_DrawItem);
            this.lstCandidates.SelectedIndexChanged += new System.EventHandler(this.lstCandidates_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(473, 281);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnIgnore.Location = new System.Drawing.Point(277, 281);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(47, 23);
            this.btnIgnore.TabIndex = 4;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // chkHideMatched
            // 
            this.chkHideMatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideMatched.AutoSize = true;
            this.chkHideMatched.Location = new System.Drawing.Point(16, 285);
            this.chkHideMatched.Name = "chkHideMatched";
            this.chkHideMatched.Size = new System.Drawing.Size(98, 17);
            this.chkHideMatched.TabIndex = 5;
            this.chkHideMatched.Text = "Hide matched?";
            this.chkHideMatched.UseVisualStyleBackColor = true;
            this.chkHideMatched.CheckedChanged += new System.EventHandler(this.chkHideMatched_CheckedChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.Location = new System.Drawing.Point(120, 282);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(88, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(217, 281);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(54, 23);
            this.btnAbort.TabIndex = 7;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // cbIgnoreMode
            // 
            this.cbIgnoreMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIgnoreMode.FormattingEnabled = true;
            this.cbIgnoreMode.Items.AddRange(new object[] {
            "This track",
            "This album",
            "All unmatched"});
            this.cbIgnoreMode.Location = new System.Drawing.Point(324, 282);
            this.cbIgnoreMode.Name = "cbIgnoreMode";
            this.cbIgnoreMode.Size = new System.Drawing.Size(94, 21);
            this.cbIgnoreMode.TabIndex = 8;
            // 
            // MatchLookupMultiple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 315);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.cbIgnoreMode);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.chkHideMatched);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstCandidates);
            this.Controls.Add(this.grpTags);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchLookupMultiple";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MatchLookupMultiple_FormClosing);
            this.Load += new System.EventHandler(this.MatchLookupMultiple_Load);
            this.grpTags.ResumeLayout(false);
            this.grpTags.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox grpTags;
        private System.Windows.Forms.Label lblFileNameCandidate;
        private System.Windows.Forms.Label lblFileNameSource;
        private System.Windows.Forms.Label lblFileNameHeader;
        private System.Windows.Forms.Label lblTrackNumCandidate;
        private System.Windows.Forms.Label lblTrackNumSource;
        private System.Windows.Forms.Label lblTrackNumHeader;
        private System.Windows.Forms.Label lblAlbumCandidate;
        private System.Windows.Forms.Label lblAlbumSource;
        private System.Windows.Forms.Label lblAlbumHeader;
        private System.Windows.Forms.Label lblArtistCandidate;
        private System.Windows.Forms.Label lblArtistSource;
        private System.Windows.Forms.Label lblArtistHeader;
        private System.Windows.Forms.Label lblTitleCandidate;
        private System.Windows.Forms.Label lblTitleSource;
        private System.Windows.Forms.Label lblTitleHeader;
        private System.Windows.Forms.ListBox lstCandidates;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Label lblFileNameEquals;
        private System.Windows.Forms.Label lblTrackNumEquals;
        private System.Windows.Forms.Label lblAlbumEquals;
        private System.Windows.Forms.Label lblArtistEquals;
        private System.Windows.Forms.Label lblTitleEquals;
        private System.Windows.Forms.CheckBox chkHideMatched;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.ComboBox cbIgnoreMode;
    }
}