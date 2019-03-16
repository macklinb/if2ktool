namespace if2ktool
{
    partial class GenerateReplicateLibraryStructure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateReplicateLibraryStructure));
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkZipStructure = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.chkIncludeLog = new System.Windows.Forms.CheckBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(17, 14);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(267, 82);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            // 
            // chkZipStructure
            // 
            this.chkZipStructure.AutoSize = true;
            this.chkZipStructure.Location = new System.Drawing.Point(19, 25);
            this.chkZipStructure.Name = "chkZipStructure";
            this.chkZipStructure.Size = new System.Drawing.Size(145, 17);
            this.chkZipStructure.TabIndex = 1;
            this.chkZipStructure.Text = "Zip resulting file hierarchy";
            this.chkZipStructure.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(216, 183);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkIncludeLog
            // 
            this.chkIncludeLog.AutoSize = true;
            this.chkIncludeLog.Location = new System.Drawing.Point(19, 48);
            this.chkIncludeLog.Name = "chkIncludeLog";
            this.chkIncludeLog.Size = new System.Drawing.Size(125, 17);
            this.chkIncludeLog.TabIndex = 4;
            this.chkIncludeLog.Text = "Include robocopy log";
            this.chkIncludeLog.UseVisualStyleBackColor = true;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkZipStructure);
            this.grpOptions.Controls.Add(this.chkIncludeLog);
            this.grpOptions.Location = new System.Drawing.Point(12, 99);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(279, 78);
            this.grpOptions.TabIndex = 5;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 183);
            this.progressBar.MarqueeAnimationSpeed = 200;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(198, 23);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // ReplicateLibraryStructure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 219);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReplicateLibraryStructure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replicate library structure";
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkZipStructure;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkIncludeLog;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}