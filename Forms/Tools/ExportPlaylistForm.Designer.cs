namespace if2ktool
{
    partial class ExportPlaylistForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportPlaylistForm));
            this.lstPlaylists = new System.Windows.Forms.ListBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.lblPersistentId = new System.Windows.Forms.Label();
            this.lblIsSmart = new System.Windows.Forms.Label();
            this.lblTracks = new System.Windows.Forms.Label();
            this.lblTracksValue = new System.Windows.Forms.Label();
            this.lblIsSmartValue = new System.Windows.Forms.Label();
            this.lblPersistentIdValue = new System.Windows.Forms.Label();
            this.lblIdValue = new System.Windows.Forms.Label();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.chkExtendedM3u = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.chkIgnoreErrors = new System.Windows.Forms.CheckBox();
            this.grpProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstPlaylists
            // 
            this.lstPlaylists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlaylists.FormattingEnabled = true;
            this.lstPlaylists.Location = new System.Drawing.Point(0, 6);
            this.lstPlaylists.Name = "lstPlaylists";
            this.lstPlaylists.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstPlaylists.Size = new System.Drawing.Size(190, 154);
            this.lstPlaylists.TabIndex = 0;
            this.lstPlaylists.SelectedValueChanged += new System.EventHandler(this.lstPlaylists_SelectedValueChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(12, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(391, 46);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(11, 22);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(21, 13);
            this.lblId.TabIndex = 3;
            this.lblId.Text = "ID:";
            // 
            // lblPersistentId
            // 
            this.lblPersistentId.AutoSize = true;
            this.lblPersistentId.Location = new System.Drawing.Point(11, 42);
            this.lblPersistentId.Name = "lblPersistentId";
            this.lblPersistentId.Size = new System.Drawing.Size(40, 13);
            this.lblPersistentId.TabIndex = 4;
            this.lblPersistentId.Text = "Per ID:";
            // 
            // lblIsSmart
            // 
            this.lblIsSmart.AutoSize = true;
            this.lblIsSmart.Location = new System.Drawing.Point(11, 62);
            this.lblIsSmart.Name = "lblIsSmart";
            this.lblIsSmart.Size = new System.Drawing.Size(51, 13);
            this.lblIsSmart.TabIndex = 5;
            this.lblIsSmart.Text = "Is Smart?";
            // 
            // lblTracks
            // 
            this.lblTracks.AutoSize = true;
            this.lblTracks.Location = new System.Drawing.Point(11, 82);
            this.lblTracks.Name = "lblTracks";
            this.lblTracks.Size = new System.Drawing.Size(43, 13);
            this.lblTracks.TabIndex = 6;
            this.lblTracks.Text = "Tracks:";
            // 
            // lblTracksValue
            // 
            this.lblTracksValue.Location = new System.Drawing.Point(76, 82);
            this.lblTracksValue.Name = "lblTracksValue";
            this.lblTracksValue.Size = new System.Drawing.Size(102, 13);
            this.lblTracksValue.TabIndex = 11;
            this.lblTracksValue.Text = "-";
            // 
            // lblIsSmartValue
            // 
            this.lblIsSmartValue.Location = new System.Drawing.Point(76, 62);
            this.lblIsSmartValue.Name = "lblIsSmartValue";
            this.lblIsSmartValue.Size = new System.Drawing.Size(102, 13);
            this.lblIsSmartValue.TabIndex = 10;
            this.lblIsSmartValue.Text = "-";
            // 
            // lblPersistentIdValue
            // 
            this.lblPersistentIdValue.Location = new System.Drawing.Point(76, 42);
            this.lblPersistentIdValue.Name = "lblPersistentIdValue";
            this.lblPersistentIdValue.Size = new System.Drawing.Size(102, 13);
            this.lblPersistentIdValue.TabIndex = 9;
            this.lblPersistentIdValue.Text = "-";
            // 
            // lblIdValue
            // 
            this.lblIdValue.Location = new System.Drawing.Point(76, 22);
            this.lblIdValue.Name = "lblIdValue";
            this.lblIdValue.Size = new System.Drawing.Size(102, 13);
            this.lblIdValue.TabIndex = 8;
            this.lblIdValue.Text = "-";
            // 
            // grpProperties
            // 
            this.grpProperties.Controls.Add(this.lblTracksValue);
            this.grpProperties.Controls.Add(this.lblId);
            this.grpProperties.Controls.Add(this.lblIsSmartValue);
            this.grpProperties.Controls.Add(this.lblPersistentId);
            this.grpProperties.Controls.Add(this.lblPersistentIdValue);
            this.grpProperties.Controls.Add(this.lblIsSmart);
            this.grpProperties.Controls.Add(this.lblIdValue);
            this.grpProperties.Controls.Add(this.lblTracks);
            this.grpProperties.Location = new System.Drawing.Point(0, 0);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.Size = new System.Drawing.Size(197, 109);
            this.grpProperties.TabIndex = 12;
            this.grpProperties.TabStop = false;
            this.grpProperties.Text = "Properties";
            // 
            // chkExtendedM3u
            // 
            this.chkExtendedM3u.AutoSize = true;
            this.chkExtendedM3u.Location = new System.Drawing.Point(14, 142);
            this.chkExtendedM3u.Name = "chkExtendedM3u";
            this.chkExtendedM3u.Size = new System.Drawing.Size(97, 17);
            this.chkExtendedM3u.TabIndex = 13;
            this.chkExtendedM3u.Text = "Extended M3U";
            this.chkExtendedM3u.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(325, 218);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 32000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(9, 55);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lstPlaylists);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.splitContainer.Panel1MinSize = 190;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.chkIgnoreErrors);
            this.splitContainer.Panel2.Controls.Add(this.grpProperties);
            this.splitContainer.Panel2.Controls.Add(this.chkExtendedM3u);
            this.splitContainer.Panel2MinSize = 190;
            this.splitContainer.Size = new System.Drawing.Size(391, 160);
            this.splitContainer.SplitterDistance = 190;
            this.splitContainer.TabIndex = 15;
            // 
            // chkIgnoreErrors
            // 
            this.chkIgnoreErrors.AutoSize = true;
            this.chkIgnoreErrors.Location = new System.Drawing.Point(13, 119);
            this.chkIgnoreErrors.Name = "chkIgnoreErrors";
            this.chkIgnoreErrors.Size = new System.Drawing.Size(105, 17);
            this.chkIgnoreErrors.TabIndex = 16;
            this.chkIgnoreErrors.Text = "Ignore any errors";
            this.chkIgnoreErrors.UseVisualStyleBackColor = true;
            // 
            // ExportPlaylistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 252);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblDescription);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(431, 291);
            this.Name = "ExportPlaylistForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export playlists";
            this.grpProperties.ResumeLayout(false);
            this.grpProperties.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstPlaylists;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblPersistentId;
        private System.Windows.Forms.Label lblIsSmart;
        private System.Windows.Forms.Label lblTracks;
        private System.Windows.Forms.Label lblTracksValue;
        private System.Windows.Forms.Label lblIsSmartValue;
        private System.Windows.Forms.Label lblPersistentIdValue;
        private System.Windows.Forms.Label lblIdValue;
        private System.Windows.Forms.GroupBox grpProperties;
        private System.Windows.Forms.CheckBox chkExtendedM3u;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.CheckBox chkIgnoreErrors;
    }
}