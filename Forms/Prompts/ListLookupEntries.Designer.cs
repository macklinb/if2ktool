namespace if2ktool
{
    partial class ListLookupEntries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListLookupEntries));
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpTags = new System.Windows.Forms.GroupBox();
            this.lblFilePathValue = new System.Windows.Forms.Label();
            this.lblFilePathHeader = new System.Windows.Forms.Label();
            this.lblTrackNumValue = new System.Windows.Forms.Label();
            this.lblTrackNumHeader = new System.Windows.Forms.Label();
            this.lblAlbumValue = new System.Windows.Forms.Label();
            this.lblAlbumHeader = new System.Windows.Forms.Label();
            this.lblArtistValue = new System.Windows.Forms.Label();
            this.lblArtistHeader = new System.Windows.Forms.Label();
            this.lblTitleValue = new System.Windows.Forms.Label();
            this.lblTitleHeader = new System.Windows.Forms.Label();
            this.listJsonEntries = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(2, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(411, 132);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            // 
            // grpTags
            // 
            this.grpTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTags.Controls.Add(this.lblFilePathValue);
            this.grpTags.Controls.Add(this.lblFilePathHeader);
            this.grpTags.Controls.Add(this.lblTrackNumValue);
            this.grpTags.Controls.Add(this.lblTrackNumHeader);
            this.grpTags.Controls.Add(this.lblAlbumValue);
            this.grpTags.Controls.Add(this.lblAlbumHeader);
            this.grpTags.Controls.Add(this.lblArtistValue);
            this.grpTags.Controls.Add(this.lblArtistHeader);
            this.grpTags.Controls.Add(this.lblTitleValue);
            this.grpTags.Controls.Add(this.lblTitleHeader);
            this.grpTags.Location = new System.Drawing.Point(2, 135);
            this.grpTags.Name = "grpTags";
            this.grpTags.Size = new System.Drawing.Size(411, 213);
            this.grpTags.TabIndex = 1;
            this.grpTags.TabStop = false;
            this.grpTags.Text = "Tags";
            // 
            // lblFilePathValue
            // 
            this.lblFilePathValue.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblFilePathValue.Location = new System.Drawing.Point(88, 169);
            this.lblFilePathValue.Name = "lblFilePathValue";
            this.lblFilePathValue.Size = new System.Drawing.Size(310, 30);
            this.lblFilePathValue.TabIndex = 24;
            this.lblFilePathValue.TabStop = true;
            this.lblFilePathValue.Text = "-";
            this.lblFilePathValue.Click += new System.EventHandler(this.lblFilePathValue_Click);
            // 
            // lblFilePathHeader
            // 
            this.lblFilePathHeader.AutoSize = true;
            this.lblFilePathHeader.Location = new System.Drawing.Point(17, 169);
            this.lblFilePathHeader.Name = "lblFilePathHeader";
            this.lblFilePathHeader.Size = new System.Drawing.Size(51, 13);
            this.lblFilePathHeader.TabIndex = 21;
            this.lblFilePathHeader.Text = "File Path:";
            // 
            // lblTrackNumValue
            // 
            this.lblTrackNumValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrackNumValue.Location = new System.Drawing.Point(88, 133);
            this.lblTrackNumValue.Name = "lblTrackNumValue";
            this.lblTrackNumValue.Size = new System.Drawing.Size(310, 30);
            this.lblTrackNumValue.TabIndex = 20;
            this.lblTrackNumValue.Text = "-";
            this.lblTrackNumValue.UseMnemonic = false;
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
            // lblAlbumValue
            // 
            this.lblAlbumValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlbumValue.Location = new System.Drawing.Point(88, 97);
            this.lblAlbumValue.Name = "lblAlbumValue";
            this.lblAlbumValue.Size = new System.Drawing.Size(310, 30);
            this.lblAlbumValue.TabIndex = 17;
            this.lblAlbumValue.Text = "-";
            this.lblAlbumValue.UseMnemonic = false;
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
            // lblArtistValue
            // 
            this.lblArtistValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArtistValue.Location = new System.Drawing.Point(88, 61);
            this.lblArtistValue.Name = "lblArtistValue";
            this.lblArtistValue.Size = new System.Drawing.Size(310, 30);
            this.lblArtistValue.TabIndex = 14;
            this.lblArtistValue.Text = "-";
            this.lblArtistValue.UseMnemonic = false;
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
            // lblTitleValue
            // 
            this.lblTitleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitleValue.Location = new System.Drawing.Point(88, 25);
            this.lblTitleValue.Name = "lblTitleValue";
            this.lblTitleValue.Size = new System.Drawing.Size(310, 30);
            this.lblTitleValue.TabIndex = 11;
            this.lblTitleValue.Text = "-";
            this.lblTitleValue.UseMnemonic = false;
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
            // listJsonEntries
            // 
            this.listJsonEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listJsonEntries.FormattingEnabled = true;
            this.listJsonEntries.Location = new System.Drawing.Point(0, 0);
            this.listJsonEntries.Name = "listJsonEntries";
            this.listJsonEntries.Size = new System.Drawing.Size(278, 377);
            this.listJsonEntries.TabIndex = 2;
            this.listJsonEntries.SelectedIndexChanged += new System.EventHandler(this.lstCandidates_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(335, 354);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listJsonEntries);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblDescription);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.grpTags);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(695, 377);
            this.splitContainer1.SplitterDistance = 278;
            this.splitContainer1.TabIndex = 4;
            // 
            // ListLookupEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 399);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ListLookupEntries";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.grpTags.ResumeLayout(false);
            this.grpTags.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox grpTags;
        private System.Windows.Forms.Label lblFilePathHeader;
        private System.Windows.Forms.Label lblTrackNumValue;
        private System.Windows.Forms.Label lblTrackNumHeader;
        private System.Windows.Forms.Label lblAlbumValue;
        private System.Windows.Forms.Label lblAlbumHeader;
        private System.Windows.Forms.Label lblArtistValue;
        private System.Windows.Forms.Label lblArtistHeader;
        private System.Windows.Forms.Label lblTitleValue;
        private System.Windows.Forms.Label lblTitleHeader;
        private System.Windows.Forms.ListBox listJsonEntries;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblFilePathValue;
    }
}