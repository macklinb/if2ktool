using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class GenerateLookupJson : Form
    {
        string[] lines;
        const string SCRIPT_NAME = "CreateLookupJSON.exe";
        const string PATTERNS_NAME = "json_patterns.txt";
        const string DEFAULT_FOOBAR_LOCATION = "C:\\Program Files (x86)\\foobar2000\\foobar2000.exe";
        const string FOOBAR_COMPONENT_TEXTTOOLS = "components\\foo_texttools.dll";
        const string FOOBAR_COMPONENT_ALBUMLIST = "components\\foo_albumlist.dll";

        public GenerateLookupJson()
        {
            InitializeComponent();

            toolTip.SetToolTip(btnGenerate, Consts.TOOLTIP_GENERATE_LOOKUP_BUTTON);

            lines = Properties.Resources.JSON_Patterns.Replace("\r", "").Split('\n');
            SetText();
        }

        public void SetText()
        {
            if (lines == null || lines.Length < 6)
            {
                txtTrackPattern.Text = "There was an issue loading the resource!";
                return;
            }

            int offset = chkFormatted.Checked ? 0 : 3;
            txtTrackPattern.Text = lines[0 + offset];
            txtGroupHeader.Text = lines[1 + offset];
            txtGroupFooter.Text = lines[2 + offset];
            txtTrackPattern.DeselectAll();
            txtGroupHeader.DeselectAll();
            txtGroupFooter.DeselectAll();
        }

        private void chkFormatted_CheckedChanged(object sender, EventArgs e)
        {
            SetText();
        }

        private void txtTrackPattern_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtTrackPattern.SelectAll();
        }

        private void txtGroupHeader_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtGroupHeader.SelectAll();
        }

        private void txtGroupFooter_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtGroupFooter.SelectAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateJson();
        }

        void GenerateJson()
        {
            var result = MessageBox.Show("This tool will simulate a series of user inputs in order to take the effort out of generating a lookup JSON.\n\nWhile it should not necessarily be affected by other inputs, it is recommended that you don't do anything for a few seconds while it's running (you'll know if it's done when it asks you to save the generated file).\n\nIf this is okay, click \"OK\", otherwise click \"Cancel\"", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Cancel)
                return;

            Debug.Log(string.Empty);
            Debug.Log("Starting JSON generator...");

            string foobarLocation = DEFAULT_FOOBAR_LOCATION;

            // Load the last set foobar location (only if it exists)
            if (!string.IsNullOrEmpty(Properties.Settings.Default.lastFoobarPath))
            {
                if (File.Exists(DEFAULT_FOOBAR_LOCATION))
                {
                    foobarLocation = Properties.Settings.Default.lastFoobarPath;
                }

                // Fallback to the default, and remove the saved one.
                else
                {
                    foobarLocation = DEFAULT_FOOBAR_LOCATION;
                    Properties.Settings.Default.lastFoobarPath = string.Empty;
                }
            }

            // Check if the script/exe exists
            if (!File.Exists(SCRIPT_NAME))
            {
                File.WriteAllBytes(SCRIPT_NAME, Properties.Resources.CreateLookupJSON_exe);
                Debug.Log(" " + SCRIPT_NAME + " copied into place from resources");
            }
            else
                Debug.Log(" Using " + SCRIPT_NAME + " already in root directory");

            // Check if the patterns file exists
            if (!File.Exists(PATTERNS_NAME))
            {
                File.WriteAllText(PATTERNS_NAME, Properties.Resources.JSON_Patterns);
                Debug.Log(" " + PATTERNS_NAME + " copied into place from resources");
            }
            else
                Debug.Log(" Using " + PATTERNS_NAME + " already in root directory");

            // Ask user to select the foobar2000 location manually
            if (!File.Exists(foobarLocation))
            {
                Debug.LogWarning(" foobar2000 not found at " + foobarLocation);

                if (LookForExecutable("foobar2000", out string fooSelectedPath))
                {
                    foobarLocation = fooSelectedPath;
                    Properties.Settings.Default.lastFoobarPath = fooSelectedPath;
                    Properties.Settings.Default.Save();
                }

                // User cancelled path selection
                else return;
            }

            Debug.Log(" foobar2000 path: " + foobarLocation);

            // Check if required components exist
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(foobarLocation), FOOBAR_COMPONENT_TEXTTOOLS)))
            {
                Debug.LogError(" foo_texttools is required!");
                MessageBox.Show("The component \"foo_texttools\" is required! Please install it and try again!", "Prerequisite missing!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(foobarLocation), FOOBAR_COMPONENT_ALBUMLIST)))
            {
                Debug.LogError(" foo_albumlist is required!");
                MessageBox.Show("The component \"foo_albumlist\" is required! Please install it and try again!", "Prerequisite missing!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = SCRIPT_NAME;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

            processStartInfo.Arguments += "/FoobarExe \"" + foobarLocation + "\" ";
            processStartInfo.Arguments += "/ToStdout ";
            processStartInfo.Arguments += chkFormatted.Checked ? "/Formatted" : "";

            var sb = new System.Text.StringBuilder();
            var sw = Stopwatch.StartNew();

            Debug.Log("Creating process using: " + processStartInfo.FileName + " " + processStartInfo.Arguments);

            Debug.LogInline("Waiting for data... ");

            int exitCode = -1;

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit(10000);

                if (process.HasExited)
                {
                    exitCode = process.ExitCode;
                    Debug.LogInline("Process exited with code " + process.ExitCode + ", took " + sw.ElapsedMilliseconds + " ms");
                }
                else
                {
                    process.Kill();
                    Debug.LogInline("Process timed out and was killed");
                }

                Debug.LogLine();
            }

            // Bring if2ktool window back into the foreground
            Application.OpenForms["Main"].BringToFront();
            this.BringToFront();

            if (exitCode == 0)
            {
                string jsonString = sb.ToString();
                sb.Clear();

                // Check that we recieved data
                if (string.IsNullOrEmpty(jsonString) || string.IsNullOrWhiteSpace(jsonString))
                {
                    Debug.LogError("No data was recieved from the process! Please ensure there are no other windows obscuring foobar2000, and try again. Otherwise you can still fill the form yourself.", true);
                }

                // Check that the data recieved was a valid JSON
                else if (!IsJsonWellFormed(jsonString))
                {
                    Debug.LogError("Generated data was malformed! You may have tags that use JSON reserved or control characters!\n\nIf this is the case, or if you think this is a bug, please open a new issue on the Github page", true);
                }
                else
                {
                    int size = System.Text.Encoding.UTF8.GetByteCount(jsonString);
                    Debug.Log("Recieved " + size + " bytes of seemingly well-formed data");

                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "Save json file";
                    saveFileDialog.Filter = "JSON file|*.json;";
                    saveFileDialog.FileName = "export.json";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, jsonString);
                        Debug.Log("Wrote to " + saveFileDialog.FileName);

                        // Open in explorer
                        Process.Start("explorer.exe", "/select, \"" + saveFileDialog.FileName + "\"");

                        // Close form
                        this.Close();
                    }
                    else
                    {
                        Debug.Log("User cancelled SaveFileDialog");
                    }
                }
            }
            else
            {
                Debug.LogError("The process failed with code " + exitCode, true);
            }
        }

        private bool IsJsonWellFormed(string jsonStr)
        {
            // List of serialization errors
            List<string> errors = new List<string>();

            var jss = new JsonSerializerSettings
            {
                Error = delegate (object z, Newtonsoft.Json.Serialization.ErrorEventArgs eea)
                {
                    errors.Add(eea.ErrorContext.Error.Message);
                    eea.ErrorContext.Handled = true;
                }
            };

            try
            {
                JsonConvert.DeserializeObject<MappingWorker.LookupCollection>(jsonStr);
            }

            // Make sure the file can be deserialized (the Error delegate above doesn't handle malformed json, only deserialization errors)
            catch (Exception x)
            {
                Debug.LogError("An exception occurred while deserializing (it this a well formed JSON?)\n" + x.Message.ToString());
                return false;
            }

            // Stop if there were internal errors in deserialization
            if (errors.Any())
            {
                Debug.LogError("One or more errors occurred during deserialization!\n" +
                                string.Join("\n", errors.Select(x => "    " + x)));
                return false;
            }

            return true;
        }

        private bool LookForExecutable(string name, out string selectedPath)
        {
            if (MessageBox.Show(name + " executable not found! Please select the executable manually.", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Title = "Open " + name + " exe";
                openFileDialog.Filter = "Executable|*.exe;";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = openFileDialog.FileName;
                    return true;
                }
            }

            selectedPath = null;
            return false;
        }
    }
}
