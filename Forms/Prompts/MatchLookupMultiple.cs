using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace if2ktool
{
    public partial class MatchLookupMultiple : Form
    {
        public MappingWorker.LookupEntry matchedLookupEntry { get; private set; }
        public Guid[] ignoreGuids { get; private set; }
        public bool ignoreAllUnmatched;

        Entry sourceEntry;
        Main mainForm;

        MappingWorker.LookupEntry[] items;
        MappingWorker.LookupEntry[] itemsUnmatched;

        const string MODE_CANDIDATES_DESC = "Found more than one possible match for the following entry. Please select the correct item then click \"OK\", or click \"Ignore\" to skip over this entry.";
        const string MODE_ALL_DESC = "No match was found for the following entry. Please select the correct item and then click \"OK\", or click \"Ignore\" to skip over this entry.";

        public enum Mode
        {
            SelectCandidates,
            SelectAll
        }
        
        public MatchLookupMultiple(Entry entry, List<MappingWorker.LookupEntry> candidates, Mode mode)
        {
            InitializeComponent();

            mainForm = (Main)Application.OpenForms["Main"];

            this.sourceEntry = entry;

            if (entry != null)
            {
                SetLabelText(lblTitleSource, entry.trackTitle);
                SetLabelText(lblArtistSource, entry.artist);
                SetLabelText(lblAlbumSource, entry.album);
                SetLabelText(lblTrackNumSource, entry.trackNumber.ToString());
                SetLabelText(lblFileNameSource, entry.fileName);
            }
            
            lstCandidates.DoubleBuffered(true);

            // Add items to lstCandidates
            lstCandidates.DisplayMember = "displayTitle";

            if (candidates != null)
            {
                items = candidates.OrderBy(x => x.displayTitle).ToArray();
                itemsUnmatched = candidates.Where(x => x.isMatched == false).ToArray();

                lstCandidates.DataSource = items;
            }

            lstCandidates.ClearSelected();

            btnOK.Enabled = lstCandidates.SelectedIndex > -1;

            // Select the lookup entry from a small list of candidates
            // This places the listBox above the information, and adjusts the label text accordingly
            if (mode == Mode.SelectCandidates)
            {
                lblDescription.Text = MODE_CANDIDATES_DESC;
                lstCandidates.Location = new Point(32, 58);
                lstCandidates.Size = new Size(298, 69);
                this.Size = new Size(378, 433);

                chkHideMatched.Visible = false;
                txtSearch.Visible = false;

                // Hide the ignore mode dropdown (only allow ignoring this track)
                cbIgnoreMode.Visible = false;
            }

            // Select the lookup entry from every possible lookup entry
            // This places the listBox to the right of the info, and adjusts the label text accordingly
            else if (mode == Mode.SelectAll)
            {
                lblDescription.Text = MODE_ALL_DESC;
                lstCandidates.Location = new Point(12, 9);
                lstCandidates.Size = new Size(196, 264);
                this.Size = new Size(580, 354);
            }

            cbIgnoreMode.SelectedIndex = 0;
        }

        private void MatchLookupMultiple_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            this.Flash(false);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            matchedLookupEntry = (MappingWorker.LookupEntry)lstCandidates.SelectedItem;

            this.Close();
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            if (cbIgnoreMode.SelectedIndex == 1)
            {
                ignoreGuids = mainForm.GetEntriesEnumerable(EntryFilter.AllEntries).Where(x => x.album == sourceEntry.album && (x.albumArtist == sourceEntry.albumArtist || (sourceEntry.compilation == true ? x.compilation == true : false)) ).Select(x => x.id).ToArray();
            }
            else if (cbIgnoreMode.SelectedIndex == 2)
            {
                ignoreAllUnmatched = true;
            }

            matchedLookupEntry = null;
            this.Close();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            matchedLookupEntry = null;
            this.Close();
        }

        private void lstCandidates_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = lstCandidates.SelectedIndex > -1;

            if (lstCandidates.SelectedIndex >= 0)
            {
                var lookupEntry = (MappingWorker.LookupEntry)lstCandidates.SelectedItem;

                SetLabelText(lblTitleCandidate, lookupEntry.title);
                SetEqualsMark(lblTitleEquals, sourceEntry.trackTitle, lookupEntry.title);

                SetLabelText(lblArtistCandidate, lookupEntry.artist);
                SetEqualsMark(lblArtistEquals, sourceEntry.artist, lookupEntry.artist);

                SetLabelText(lblAlbumCandidate, lookupEntry.album);
                SetEqualsMark(lblAlbumEquals, sourceEntry.album, lookupEntry.album);

                SetLabelText(lblTrackNumCandidate, lookupEntry.trackNumber);
                SetEqualsMark(lblTrackNumEquals, sourceEntry.trackNumber.ToString(), lookupEntry.trackNumber);

                SetLabelText(lblFileNameCandidate, lookupEntry.fileName);
                SetEqualsMark(lblFileNameEquals, sourceEntry.fileName, lookupEntry.fileName);
            }
            else
            {
                SetLabelText(lblTitleCandidate, string.Empty);
                SetEqualsMark(lblTitleEquals, sourceEntry.trackTitle, null);

                SetLabelText(lblArtistCandidate, string.Empty);
                SetEqualsMark(lblArtistEquals, sourceEntry.artist, null);

                SetLabelText(lblAlbumCandidate, string.Empty);
                SetEqualsMark(lblAlbumEquals, sourceEntry.album, null);

                SetLabelText(lblTrackNumCandidate, string.Empty);
                SetEqualsMark(lblTrackNumEquals, sourceEntry.trackNumber.ToString(), null);

                SetLabelText(lblFileNameCandidate, string.Empty);
                SetEqualsMark(lblFileNameEquals, sourceEntry.fileName, null);
            }
        }

        void SetLabelText(Label label, string value)
        {
            label.Text = string.IsNullOrEmpty(value) ? "-" : value;
        }

        void SetEqualsMark(Label label, string sourceValue, string candidateValue)
        {
            if (MappingWorker.IsEitherNull(sourceValue, candidateValue))
            {
                label.Text = "-";
                label.ForeColor = SystemColors.ControlText;
            }
            else
            {
                if (MappingWorker.StringEquality(sourceValue, candidateValue))
                {
                    label.Text = "✓";
                    label.ForeColor = Color.Green;
                }
                else
                {
                    label.Text = "✗";
                    label.ForeColor = Color.Red;
                }
            }
        }

        // Draw rows that are already matched in grey.
        private void lstCandidates_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index == -1) return;

            Graphics g = e.Graphics;
            // g.FillRectangle(new SolidBrush(Color.Olive), e.Bounds);
            var lookupEntry = (MappingWorker.LookupEntry)lstCandidates.Items[e.Index];
            g.DrawString(lookupEntry.displayTitle, e.Font, new SolidBrush(lookupEntry.isMatched ? SystemColors.GrayText : SystemColors.ControlText), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        // Swaps to and from the itemsUnmatched array
        private void chkHideMatched_CheckedChanged(object sender, EventArgs e)
        {
            lstCandidates.DataSource = chkHideMatched.Checked ? itemsUnmatched : items;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lstCandidates.Items.Count; i++)
            {
                var entry = (MappingWorker.LookupEntry)lstCandidates.Items[i];

                if (entry.title.ToLower().StartsWith(txtSearch.Text.ToLower()))
                {
                    lstCandidates.SelectedIndex = i;
                    return;
                }
            }

            lstCandidates.ClearSelected();
        }

        private void MatchLookupMultiple_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if we have selected an already-mapped entry
            if (matchedLookupEntry != null && matchedLookupEntry.isMatched)
            {
                // If the user doesn't want to overwrite the entry, cancel form closing
                if (MessageBox.Show("Another iTunes Library XML entry is already mapped to this lookup entry. Click \"Yes\" if you want to overwrite it, removing the association to the other XML entry.", "Already mapped", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    this.DialogResult = DialogResult.None;
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
