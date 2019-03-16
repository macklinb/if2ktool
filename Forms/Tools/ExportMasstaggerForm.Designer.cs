namespace if2ktool
{
    partial class ExportMasstaggerForm
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
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.chkSkipRating = new System.Windows.Forms.CheckBox();
            this.chkSkipPlayCount = new System.Windows.Forms.CheckBox();
            this.chkSkipLastPlayed = new System.Windows.Forms.CheckBox();
            this.chkSkipDateAdded = new System.Windows.Forms.CheckBox();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkNoHeader = new System.Windows.Forms.CheckBox();
            this.chkAddSortField = new System.Windows.Forms.CheckBox();
            this.grpDebugging = new System.Windows.Forms.GroupBox();
            this.chkAppendUnderscore = new System.Windows.Forms.CheckBox();
            this.chkDebugSortOrder = new System.Windows.Forms.CheckBox();
            this.lblDebugWarning = new System.Windows.Forms.Label();
            this.cbSortOrder = new System.Windows.Forms.ComboBox();
            this.lblSortBy = new System.Windows.Forms.Label();
            this.chkFilterNot = new System.Windows.Forms.CheckBox();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.grpSelect.SuspendLayout();
            this.grpDebugging.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSelect
            // 
            this.grpSelect.Controls.Add(this.chkSkipRating);
            this.grpSelect.Controls.Add(this.chkSkipPlayCount);
            this.grpSelect.Controls.Add(this.chkSkipLastPlayed);
            this.grpSelect.Controls.Add(this.chkSkipDateAdded);
            this.grpSelect.Location = new System.Drawing.Point(12, 74);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(207, 101);
            this.grpSelect.TabIndex = 7;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Skip";
            // 
            // chkSkipRating
            // 
            this.chkSkipRating.AutoSize = true;
            this.chkSkipRating.Location = new System.Drawing.Point(104, 48);
            this.chkSkipRating.Name = "chkSkipRating";
            this.chkSkipRating.Size = new System.Drawing.Size(57, 17);
            this.chkSkipRating.TabIndex = 3;
            this.chkSkipRating.Text = "Rating";
            this.chkSkipRating.UseVisualStyleBackColor = true;
            // 
            // chkSkipPlayCount
            // 
            this.chkSkipPlayCount.AutoSize = true;
            this.chkSkipPlayCount.Location = new System.Drawing.Point(104, 25);
            this.chkSkipPlayCount.Name = "chkSkipPlayCount";
            this.chkSkipPlayCount.Size = new System.Drawing.Size(77, 17);
            this.chkSkipPlayCount.TabIndex = 2;
            this.chkSkipPlayCount.Text = "Play Count";
            this.chkSkipPlayCount.UseVisualStyleBackColor = true;
            // 
            // chkSkipLastPlayed
            // 
            this.chkSkipLastPlayed.AutoSize = true;
            this.chkSkipLastPlayed.Location = new System.Drawing.Point(15, 48);
            this.chkSkipLastPlayed.Name = "chkSkipLastPlayed";
            this.chkSkipLastPlayed.Size = new System.Drawing.Size(81, 17);
            this.chkSkipLastPlayed.TabIndex = 1;
            this.chkSkipLastPlayed.Text = "Last Played";
            this.chkSkipLastPlayed.UseVisualStyleBackColor = true;
            // 
            // chkSkipDateAdded
            // 
            this.chkSkipDateAdded.AutoSize = true;
            this.chkSkipDateAdded.Location = new System.Drawing.Point(15, 25);
            this.chkSkipDateAdded.Name = "chkSkipDateAdded";
            this.chkSkipDateAdded.Size = new System.Drawing.Size(83, 17);
            this.chkSkipDateAdded.TabIndex = 0;
            this.chkSkipDateAdded.Text = "Date Added";
            this.chkSkipDateAdded.UseVisualStyleBackColor = true;
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(12, 22);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(120, 21);
            this.cbFilter.TabIndex = 6;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(257, 184);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(90, 13);
            this.lblCount.TabIndex = 9;
            this.lblCount.Text = "0 entries";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(353, 179);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkNoHeader
            // 
            this.chkNoHeader.AutoSize = true;
            this.chkNoHeader.Location = new System.Drawing.Point(16, 28);
            this.chkNoHeader.Name = "chkNoHeader";
            this.chkNoHeader.Size = new System.Drawing.Size(155, 17);
            this.chkNoHeader.TabIndex = 4;
            this.chkNoHeader.Text = "Export without MTS header";
            this.chkNoHeader.UseVisualStyleBackColor = true;
            // 
            // chkAddSortField
            // 
            this.chkAddSortField.AutoSize = true;
            this.chkAddSortField.Location = new System.Drawing.Point(16, 51);
            this.chkAddSortField.Name = "chkAddSortField";
            this.chkAddSortField.Size = new System.Drawing.Size(144, 17);
            this.chkAddSortField.TabIndex = 10;
            this.chkAddSortField.Text = "Add sort field before data";
            this.chkAddSortField.UseVisualStyleBackColor = true;
            // 
            // grpDebugging
            // 
            this.grpDebugging.Controls.Add(this.chkAppendUnderscore);
            this.grpDebugging.Controls.Add(this.chkDebugSortOrder);
            this.grpDebugging.Controls.Add(this.lblDebugWarning);
            this.grpDebugging.Controls.Add(this.chkNoHeader);
            this.grpDebugging.Controls.Add(this.chkAddSortField);
            this.grpDebugging.Location = new System.Drawing.Point(225, 12);
            this.grpDebugging.Name = "grpDebugging";
            this.grpDebugging.Size = new System.Drawing.Size(218, 163);
            this.grpDebugging.TabIndex = 11;
            this.grpDebugging.TabStop = false;
            this.grpDebugging.Text = "Debugging";
            // 
            // chkAppendUnderscore
            // 
            this.chkAppendUnderscore.AutoSize = true;
            this.chkAppendUnderscore.Location = new System.Drawing.Point(37, 133);
            this.chkAppendUnderscore.Name = "chkAppendUnderscore";
            this.chkAppendUnderscore.Size = new System.Drawing.Size(150, 17);
            this.chkAppendUnderscore.TabIndex = 14;
            this.chkAppendUnderscore.Text = "Append underscore to title";
            this.chkAppendUnderscore.UseVisualStyleBackColor = true;
            // 
            // chkDebugSortOrder
            // 
            this.chkDebugSortOrder.AutoSize = true;
            this.chkDebugSortOrder.Location = new System.Drawing.Point(16, 110);
            this.chkDebugSortOrder.Name = "chkDebugSortOrder";
            this.chkDebugSortOrder.Size = new System.Drawing.Size(157, 17);
            this.chkDebugSortOrder.TabIndex = 13;
            this.chkDebugSortOrder.Text = "Debug sort order (titles only)";
            this.chkDebugSortOrder.UseVisualStyleBackColor = true;
            // 
            // lblDebugWarning
            // 
            this.lblDebugWarning.AutoSize = true;
            this.lblDebugWarning.Location = new System.Drawing.Point(14, 75);
            this.lblDebugWarning.Name = "lblDebugWarning";
            this.lblDebugWarning.Size = new System.Drawing.Size(194, 26);
            this.lblDebugWarning.TabIndex = 12;
            this.lblDebugWarning.Text = "Enabling either of the above options will\r\ncause the generated file to be invalid" +
    ".";
            // 
            // cbSortOrder
            // 
            this.cbSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSortOrder.FormattingEnabled = true;
            this.cbSortOrder.Location = new System.Drawing.Point(70, 181);
            this.cbSortOrder.Name = "cbSortOrder";
            this.cbSortOrder.Size = new System.Drawing.Size(149, 21);
            this.cbSortOrder.TabIndex = 12;
            this.cbSortOrder.SelectedValueChanged += new System.EventHandler(this.cbSortOrder_SelectedValueChanged);
            // 
            // lblSortBy
            // 
            this.lblSortBy.AutoSize = true;
            this.lblSortBy.Location = new System.Drawing.Point(21, 184);
            this.lblSortBy.Name = "lblSortBy";
            this.lblSortBy.Size = new System.Drawing.Size(43, 13);
            this.lblSortBy.TabIndex = 13;
            this.lblSortBy.Text = "Sort by:";
            // 
            // chkFilterNot
            // 
            this.chkFilterNot.AutoSize = true;
            this.chkFilterNot.Location = new System.Drawing.Point(147, 24);
            this.chkFilterNot.Name = "chkFilterNot";
            this.chkFilterNot.Size = new System.Drawing.Size(49, 17);
            this.chkFilterNot.TabIndex = 14;
            this.chkFilterNot.Text = "NOT";
            this.chkFilterNot.UseVisualStyleBackColor = true;
            this.chkFilterNot.CheckedChanged += new System.EventHandler(this.chkFilterNot_CheckedChanged);
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.cbFilter);
            this.grpFilter.Controls.Add(this.chkFilterNot);
            this.grpFilter.Location = new System.Drawing.Point(12, 12);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(207, 56);
            this.grpFilter.TabIndex = 15;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "Filter";
            // 
            // ExportMasstaggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 215);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.cbSortOrder);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblSortBy);
            this.Controls.Add(this.grpDebugging);
            this.Controls.Add(this.grpSelect);
            this.Controls.Add(this.btnExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportMasstaggerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export mts";
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.grpDebugging.ResumeLayout(false);
            this.grpDebugging.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSelect;
        private System.Windows.Forms.CheckBox chkSkipLastPlayed;
        private System.Windows.Forms.CheckBox chkSkipDateAdded;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkNoHeader;
        private System.Windows.Forms.CheckBox chkAddSortField;
        private System.Windows.Forms.GroupBox grpDebugging;
        private System.Windows.Forms.Label lblDebugWarning;
        private System.Windows.Forms.ComboBox cbSortOrder;
        private System.Windows.Forms.Label lblSortBy;
        private System.Windows.Forms.CheckBox chkFilterNot;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.CheckBox chkSkipRating;
        private System.Windows.Forms.CheckBox chkSkipPlayCount;
        private System.Windows.Forms.CheckBox chkDebugSortOrder;
        private System.Windows.Forms.CheckBox chkAppendUnderscore;
    }
}