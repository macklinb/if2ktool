using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class GenerateReplicateLibraryStructure : Form
    {
        struct RobocopyProcessArgs
        {
            public string sourceDir;
            public string targetDir;
            public string logFilePath;
            public string zipFilePath;
        }

        public GenerateReplicateLibraryStructure()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Ask the user for the required arguments
            var args = GetArgs();

            // GetArgs will return null if there were errors
            if (args == null)
                return;

            // Show the progress bar and hide the Generate button
            progressBar.Show();
            btnGenerate.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;

            // Start a new BackgroundWorker to do the actual process
            var worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += (s1, e1) =>
            {
                btnGenerate.Enabled = true;
                progressBar.Hide();
                this.Close();
            };

            // Run the worker
            worker.RunWorkerAsync(args);
        }

        RobocopyProcessArgs? GetArgs()
        {
            Debug.Log("Generating replicate library structure...");
            string sourceDir, targetDir, zipFilePath = "", logFilePath = "";

            // Pick the source directory
            if (OpenFolderDialog("Source directory", out sourceDir) == false)
                return null;

            // If zip structure is checked, pick the output zip file
            if (chkZipStructure.Checked)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = sourceDir;
                saveFileDialog.Filter = "ZIP file|*.zip;";
                saveFileDialog.FileName = Path.GetFileName(sourceDir) + ".zip";
                saveFileDialog.OverwritePrompt = true;

                DialogResult result = DialogResult.None;
                this.Invoke(() => result = saveFileDialog.ShowDialog());

                if (result == DialogResult.OK)
                {
                    zipFilePath = saveFileDialog.FileName;

                    // Delete the already-existing file at the path
                    if (File.Exists(zipFilePath))
                    {
                        try
                        {
                            File.Delete(zipFilePath);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Could not delete the existing zip file! (" + e.GetType().Name + ")\n" + e.Message);
                            return null;
                        }
                    }

                    // Generate a temporary target dir in the Temp directory
                    targetDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                }
                else
                    return null;
            }

            // Otherwise pick the output directory
            else
            {
                if (OpenFolderDialog("Target directory", out targetDir) == false)
                    return null;

                // Don't allow the user to pick the same directory ever (as this will overwrite existing files)
                if (targetDir == sourceDir)
                {
                    MessageBox.Show("Cannot pick the same directory as the source directory, as this would overwrite files!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }

                // Warn the user if there are files in the target directory
                if (Directory.Exists(targetDir) && Directory.EnumerateFileSystemEntries(targetDir).Any())
                {
                    MessageBox.Show("The target directory exists and has files that may be overwritten. As a safety measure, consider picking an empty directory instead!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }

            // Initialize the logFilePath, even if "Include robocopy log" isn't checked, as it is used to log errors
            logFilePath = Path.GetTempFileName();

            // Move the log file to a new ".log" extension
            File.Move(logFilePath, logFilePath = Path.ChangeExtension(logFilePath, ".log"));

            // Create the target directory
            try
            {
                Directory.CreateDirectory(targetDir);
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not create the directory (" + ex.GetType().Name + ")\n" + ex.Message, true);
                return null;
            }

            // If we're here, then we have all the required paths to start the worker
            return new RobocopyProcessArgs()
            {
                sourceDir = sourceDir,
                targetDir = targetDir,
                logFilePath = logFilePath,
                zipFilePath = zipFilePath
            };
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (RobocopyProcessArgs)e.Argument;

            Debug.Log(" Set source directory to: " + args.sourceDir);
            Debug.Log(" Set target directory to: " + args.targetDir);

            if (chkZipStructure.Checked)
                Debug.Log(" Set zip path to: " + args.zipFilePath);
            if (chkIncludeLog.Checked)
                Debug.Log(" Set log file path to: " + args.logFilePath);
            
            // Create the process
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "robocopy";
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = string.Format("\"{0}\" \"{1}\" /create /e /unilog:\"{2}\"", args.sourceDir, args.targetDir, args.logFilePath);

            var sw = Stopwatch.StartNew();

            Debug.Log(" Creating process using: " + processStartInfo.FileName + " " + processStartInfo.Arguments);
            Debug.Log(" Waiting for completion...");

            int exitCode = -1;
            
            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit(30000);
                sw.Stop();

                // Exited
                if (process.HasExited)
                {
                    exitCode = process.ExitCode;
                    Debug.LogInline("Process exited with code " + process.ExitCode + ", took " + sw.ElapsedMilliseconds + " ms");
                }

                // Time out
                else
                {
                    process.Kill();
                    Debug.LogInline("Process timed out and was killed");
                }
            }

            // Encapsulate this entire block in a trycatch, as there is a lot that can go wrong
            try
            {
                // If the process errored or timed out
                if (exitCode != 1)
                {
                    // Copy the log to the temp folder
                    Debug.Log("The process had errors! The output log written to: " + args.logFilePath);

                    // Delete the target dir
                    Directory.Delete(args.targetDir, true);
                }

                // If the process exited cleanly
                else
                {
                    // If we should include the log, move the file to the target folder, and with a new name
                    if (chkIncludeLog.Checked)
                        File.Move(args.logFilePath, Path.Combine(args.targetDir, Path.GetFileName(args.sourceDir) + ".log"));

                    // Otherwise delete the log, as it isn't needed
                    else
                        File.Delete(args.logFilePath);

                    // If we should zip, zip resulting file hierarchy, then delete the target folder
                    if (chkZipStructure.Checked)
                    {
                        Debug.LogLine();
                        Debug.LogInline("Zipping hierarchy (this may take a while)...");
                        ZipFile.CreateFromDirectory(args.targetDir, args.zipFilePath, CompressionLevel.NoCompression, false, Encoding.UTF8);

                        // Open in explorer
                        Process.Start("explorer.exe", "/select, \"" + args.zipFilePath + "\"");

                        Debug.LogInline("Done!"); Debug.LogLine();
                        Debug.LogInline("Deleting temporary target directory...");
                        Directory.Delete(args.targetDir, true);
                        Debug.LogInline("Done!"); Debug.LogLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred while processing the output (" + ex.GetType().Name + ")\n" + ex.Message);
                Debug.LogError("The target directory may still exist at: " + args.targetDir);
            }
        }

        bool OpenFolderDialog(string title, out string path)
        {
            // Create new CommonOpenFileDialog (the default FolderBrowserDialog is crap)
            var openFolderDialog = new CommonOpenFileDialog();
            openFolderDialog.IsFolderPicker = true;
            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            openFolderDialog.Title = title;

            CommonFileDialogResult result = CommonFileDialogResult.None;

            // Invoke dialogue in context of main thread
            this.Invoke(() => result = openFolderDialog.ShowDialog());

            if (result == CommonFileDialogResult.Ok)
            {
                path = openFolderDialog.FileName;
                return true;
            }
            else
            {
                path = null;
                return false;
            }
        }
    }
}
