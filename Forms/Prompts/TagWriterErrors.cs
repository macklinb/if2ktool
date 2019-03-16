using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class TagWriterErrors : Form
    {
        Main mainForm;

        public TagWriterErrors(Dictionary<Entry, string> entryErrors)
        {
            mainForm = (Main)Application.OpenForms["Main"];
            InitializeComponent();

            // Display error icon in picturebox
            pictureBox.Image = Bitmap.FromHicon(SystemIcons.Error.Handle);

            if (entryErrors != null && entryErrors.Count > 0)
            {
                var nodes = new List<TreeNode>();

                // Generate a TreeView like the following
                // + Album Artist - Track Title
                // ----- Path: C:\some\mapped\file path.mp3
                // ----- Error: An error occurred while writing tags!
                foreach (var entryError in entryErrors)
                {
                    var node = new TreeNode();
                    node.Text = string.Format("{0} - {1}", entryError.Key.albumArtist, entryError.Key.trackTitle);
                    node.Name = entryError.Key.id.ToString();
                    node.Nodes.Add("Path: " + entryError.Key.mappedFilePath);
                    node.Nodes.Add("Error: " + entryError.Value);
                    nodes.Add(node);
                }

                // Add each child node
                treeView.Nodes.AddRange(nodes.ToArray());
            }
        }
        
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                TreeNode selectedNode = e.Node;

                // If a child node is selected
                if (e.Node.Level == 1)
                {
                    selectedNode = e.Node.Parent;
                    treeView.SelectedNode = e.Node.Parent;
                }

                 // Get the row of the entry of the selected node
                var row = mainForm.GetRows(EntryFilter.AllEntries)
                                  .FirstOrDefault(x => ((Entry)x.DataBoundItem).id.ToString() == selectedNode.Name);

                if (row != null)
                    mainForm.SetRowSelection(row.Index, false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
