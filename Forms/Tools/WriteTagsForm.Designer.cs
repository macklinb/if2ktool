namespace if2ktool
{
    partial class WriteTagsForm
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
            this.lblFilter = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.chkSkipRating = new System.Windows.Forms.CheckBox();
            this.chkSkipPlayCount = new System.Windows.Forms.CheckBox();
            this.chkSkipLastPlayed = new System.Windows.Forms.CheckBox();
            this.chkSkipDateAdded = new System.Windows.Forms.CheckBox();
            this.btnWriteTags = new System.Windows.Forms.Button();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.chkEntryFilterNot = new System.Windows.Forms.CheckBox();
            this.grpSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(20, 15);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Filter:";
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(58, 12);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(121, 21);
            this.cbFilter.TabIndex = 1;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // grpSelect
            // 
            this.grpSelect.Controls.Add(this.chkSkipRating);
            this.grpSelect.Controls.Add(this.chkSkipPlayCount);
            this.grpSelect.Controls.Add(this.chkSkipLastPlayed);
            this.grpSelect.Controls.Add(this.chkSkipDateAdded);
            this.grpSelect.Location = new System.Drawing.Point(12, 39);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(222, 82);
            this.grpSelect.TabIndex = 2;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Skip";
            // 
            // chkSkipRating
            // 
            this.chkSkipRating.AutoSize = true;
            this.chkSkipRating.Location = new System.Drawing.Point(110, 51);
            this.chkSkipRating.Name = "chkSkipRating";
            this.chkSkipRating.Size = new System.Drawing.Size(57, 17);
            this.chkSkipRating.TabIndex = 3;
            this.chkSkipRating.Text = "Rating";
            this.chkSkipRating.UseVisualStyleBackColor = true;
            // 
            // chkSkipPlayCount
            // 
            this.chkSkipPlayCount.AutoSize = true;
            this.chkSkipPlayCount.Location = new System.Drawing.Point(110, 28);
            this.chkSkipPlayCount.Name = "chkSkipPlayCount";
            this.chkSkipPlayCount.Size = new System.Drawing.Size(77, 17);
            this.chkSkipPlayCount.TabIndex = 2;
            this.chkSkipPlayCount.Text = "Play Count";
            this.chkSkipPlayCount.UseVisualStyleBackColor = true;
            // 
            // chkSkipLastPlayed
            // 
            this.chkSkipLastPlayed.AutoSize = true;
            this.chkSkipLastPlayed.Location = new System.Drawing.Point(17, 51);
            this.chkSkipLastPlayed.Name = "chkSkipLastPlayed";
            this.chkSkipLastPlayed.Size = new System.Drawing.Size(81, 17);
            this.chkSkipLastPlayed.TabIndex = 1;
            this.chkSkipLastPlayed.Text = "Last Played";
            this.chkSkipLastPlayed.UseVisualStyleBackColor = true;
            // 
            // chkSkipDateAdded
            // 
            this.chkSkipDateAdded.AutoSize = true;
            this.chkSkipDateAdded.Location = new System.Drawing.Point(17, 28);
            this.chkSkipDateAdded.Name = "chkSkipDateAdded";
            this.chkSkipDateAdded.Size = new System.Drawing.Size(83, 17);
            this.chkSkipDateAdded.TabIndex = 0;
            this.chkSkipDateAdded.Text = "Date Added";
            this.chkSkipDateAdded.UseVisualStyleBackColor = true;
            // 
            // btnWriteTags
            // 
            this.btnWriteTags.Location = new System.Drawing.Point(144, 127);
            this.btnWriteTags.Name = "btnWriteTags";
            this.btnWriteTags.Size = new System.Drawing.Size(90, 23);
            this.btnWriteTags.TabIndex = 3;
            this.btnWriteTags.Text = "Write tags";
            this.btnWriteTags.UseVisualStyleBackColor = true;
            this.btnWriteTags.Click += new System.EventHandler(this.btnWriteTags_Click);
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(12, 132);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(34, 13);
            this.lblFileCount.TabIndex = 4;
            this.lblFileCount.Text = "0 files";
            // 
            // chkEntryFilterNot
            // 
            this.chkEntryFilterNot.AutoSize = true;
            this.chkEntryFilterNot.Location = new System.Drawing.Point(188, 15);
            this.chkEntryFilterNot.Name = "chkEntryFilterNot";
            this.chkEntryFilterNot.Size = new System.Drawing.Size(49, 17);
            this.chkEntryFilterNot.TabIndex = 5;
            this.chkEntryFilterNot.Text = "NOT";
            this.chkEntryFilterNot.UseVisualStyleBackColor = true;
            this.chkEntryFilterNot.CheckedChanged += new System.EventHandler(this.chkEntryFilterNot_CheckedChanged);
            // 
            // WriteTagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 160);
            this.Controls.Add(this.chkEntryFilterNot);
            this.Controls.Add(this.lblFileCount);
            this.Controls.Add(this.btnWriteTags);
            this.Controls.Add(this.grpSelect);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.lblFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WriteTagsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Write tags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WriteTagsForm_FormClosing);
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.GroupBox grpSelect;
        private System.Windows.Forms.CheckBox chkSkipRating;
        private System.Windows.Forms.CheckBox chkSkipPlayCount;
        private System.Windows.Forms.CheckBox chkSkipLastPlayed;
        private System.Windows.Forms.CheckBox chkSkipDateAdded;
        private System.Windows.Forms.Button btnWriteTags;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.CheckBox chkEntryFilterNot;
    }
}