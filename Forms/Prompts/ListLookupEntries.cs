using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace if2ktool
{
    public partial class ListLookupEntries : Form
    {
        public ListLookupEntries(List<MappingWorker.LookupEntry> lookupEntries)
        {
            InitializeComponent();
            listJsonEntries.DoubleBuffered(true);
            listJsonEntries.Items.Clear();

            lblDescription.Text = Consts.UNMATCHED_JSON;

            // Add items to lstCandidates
            listJsonEntries.DisplayMember = "displayTitle";
            listJsonEntries.ValueMember = "value";
            listJsonEntries.Items.AddRange(lookupEntries.ToArray());
        }

        private void lstCandidates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listJsonEntries.SelectedIndex >= 0)
            {
                var lookupEntry = (MappingWorker.LookupEntry)listJsonEntries.SelectedItem;

                SetLabelText(lblTitleValue, lookupEntry.title);
                SetLabelText(lblArtistValue, lookupEntry.artist);
                SetLabelText(lblAlbumValue, lookupEntry.album);
                SetLabelText(lblTrackNumValue, lookupEntry.trackNumber);
                SetLabelText(lblFilePathValue, lookupEntry.path);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SetLabelText(Label label, string value)
        {
            label.Text = string.IsNullOrEmpty(value) ? "-" : value;
        }

        private void lblFilePathValue_Click(object sender, EventArgs e)
        {
            if (lblFilePathValue.Text != "-")
            {
                string path = lblFilePathValue.Text;

                if (File.Exists(path))
                    System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + path + "\"");
                else
                    MessageBox.Show("Cannot open explorer to file! File no longer exists!", "File missing!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
