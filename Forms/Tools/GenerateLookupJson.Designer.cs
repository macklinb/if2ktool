using System;
using System.Windows.Forms;

namespace if2ktool
{
    partial class GenerateLookupJson
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateLookupJson));
            this.label1 = new System.Windows.Forms.Label();
            this.lblTrackPattern = new System.Windows.Forms.Label();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblGroupFooter = new System.Windows.Forms.Label();
            this.txtTrackPattern = new System.Windows.Forms.TextBox();
            this.txtGroupHeader = new System.Windows.Forms.TextBox();
            this.txtGroupFooter = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkFormatted = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 73);
            this.label1.TabIndex = 999;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // lblTrackPattern
            // 
            this.lblTrackPattern.AutoSize = true;
            this.lblTrackPattern.Location = new System.Drawing.Point(13, 86);
            this.lblTrackPattern.Name = "lblTrackPattern";
            this.lblTrackPattern.Size = new System.Drawing.Size(74, 13);
            this.lblTrackPattern.TabIndex = 999;
            this.lblTrackPattern.Text = "Track pattern:";
            // 
            // lblGroupHeader
            // 
            this.lblGroupHeader.AutoSize = true;
            this.lblGroupHeader.Location = new System.Drawing.Point(12, 266);
            this.lblGroupHeader.Name = "lblGroupHeader";
            this.lblGroupHeader.Size = new System.Drawing.Size(75, 13);
            this.lblGroupHeader.TabIndex = 999;
            this.lblGroupHeader.Text = "Group header:";
            // 
            // lblGroupFooter
            // 
            this.lblGroupFooter.AutoSize = true;
            this.lblGroupFooter.Location = new System.Drawing.Point(12, 292);
            this.lblGroupFooter.Name = "lblGroupFooter";
            this.lblGroupFooter.Size = new System.Drawing.Size(69, 13);
            this.lblGroupFooter.TabIndex = 999;
            this.lblGroupFooter.Text = "Group footer:";
            // 
            // txtTrackPattern
            // 
            this.txtTrackPattern.Location = new System.Drawing.Point(12, 108);
            this.txtTrackPattern.Multiline = true;
            this.txtTrackPattern.Name = "txtTrackPattern";
            this.txtTrackPattern.ReadOnly = true;
            this.txtTrackPattern.Size = new System.Drawing.Size(289, 149);
            this.txtTrackPattern.TabIndex = 1;
            this.txtTrackPattern.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtTrackPattern_MouseDoubleClick);
            // 
            // txtGroupHeader
            // 
            this.txtGroupHeader.Location = new System.Drawing.Point(93, 263);
            this.txtGroupHeader.Name = "txtGroupHeader";
            this.txtGroupHeader.ReadOnly = true;
            this.txtGroupHeader.Size = new System.Drawing.Size(208, 20);
            this.txtGroupHeader.TabIndex = 2;
            this.txtGroupHeader.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtGroupHeader_MouseDoubleClick);
            // 
            // txtGroupFooter
            // 
            this.txtGroupFooter.Location = new System.Drawing.Point(93, 289);
            this.txtGroupFooter.Name = "txtGroupFooter";
            this.txtGroupFooter.ReadOnly = true;
            this.txtGroupFooter.Size = new System.Drawing.Size(208, 20);
            this.txtGroupFooter.TabIndex = 3;
            this.txtGroupFooter.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtGroupFooter_MouseDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(226, 318);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkFormatted
            // 
            this.chkFormatted.AutoSize = true;
            this.chkFormatted.Checked = true;
            this.chkFormatted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFormatted.Location = new System.Drawing.Point(222, 85);
            this.chkFormatted.Name = "chkFormatted";
            this.chkFormatted.Size = new System.Drawing.Size(79, 17);
            this.chkFormatted.TabIndex = 0;
            this.chkFormatted.Text = "Formatted?";
            this.chkFormatted.UseVisualStyleBackColor = true;
            this.chkFormatted.CheckedChanged += new System.EventHandler(this.chkFormatted_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 318);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(101, 23);
            this.btnGenerate.TabIndex = 1000;
            this.btnGenerate.Text = "Generate JSON*";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // LookupJsonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 351);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.chkFormatted);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtGroupFooter);
            this.Controls.Add(this.txtGroupHeader);
            this.Controls.Add(this.txtTrackPattern);
            this.Controls.Add(this.lblGroupFooter);
            this.Controls.Add(this.lblGroupHeader);
            this.Controls.Add(this.lblTrackPattern);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LookupJsonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JSON lookup patterns";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTrackPattern;
        private System.Windows.Forms.Label lblGroupHeader;
        private System.Windows.Forms.Label lblGroupFooter;
        private System.Windows.Forms.TextBox txtTrackPattern;
        private System.Windows.Forms.TextBox txtGroupHeader;
        private System.Windows.Forms.TextBox txtGroupFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkFormatted;
        private Button btnGenerate;
        private ToolTip toolTip;
    }
}