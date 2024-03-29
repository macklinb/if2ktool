﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace if2ktool
{
    public partial class ExportPlaylistForm : Form
    {
        Main mainForm;

        public ExportPlaylistForm(List<Playlist> playlists)
        {
            mainForm = (Main)Application.OpenForms["Main"];

            InitializeComponent();

            toolTip.SetToolTip(chkExtendedM3u, Consts.TOOLTIP_EXTENDED_M3U);

            // Add all playlists to the ListBox
            lstPlaylists.DisplayMember = "name";
            lstPlaylists.Items.AddRange(playlists.ToArray());

            lstPlaylists_SelectedValueChanged(this, null);
        }

        private void lstPlaylists_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lstPlaylists.SelectedItems.Count == 1)
            {
                btnExport.Enabled = true;
                Playlist p = (Playlist)lstPlaylists.SelectedItem;
                lblIdValue.Text = p.playlistId.ToString();
                lblPersistentIdValue.Text = p.playlistPersistentId;
                lblIsSmartValue.Text = p.IsSmartPlaylist.ToString();
                lblTracksValue.Text = p.Count.ToString();
            }
            else if (lstPlaylists.SelectedItems.Count > 1)
            {
                btnExport.Enabled = true;
                lblIdValue.Text = "-";
                lblPersistentIdValue.Text = "-";
                lblIsSmartValue.Text = "-";
                lblTracksValue.Text = "-";
            }
            else
            {
                btnExport.Enabled = false;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string savePath = "";

            int playlistCount = lstPlaylists.SelectedItems.Count;

            // Select a direct filepath
            if (playlistCount == 1)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save playlist";
                saveFileDialog.FileName = ((Playlist)lstPlaylists.SelectedItem).name + ".m3u";
                saveFileDialog.Filter = "M3U playlist|*.m3u;*.m3u8";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                else
                    savePath = saveFileDialog.FileName;
            }

            // Select a folder to save in if multiple playlists are selected
            else if (playlistCount > 1)
            {
                var openFolderDialog = new CommonOpenFileDialog();
                openFolderDialog.IsFolderPicker = true;
                openFolderDialog.Title = "Save playlists";

                if (openFolderDialog.ShowDialog() != CommonFileDialogResult.Ok)
                    return;
                else
                    savePath = openFolderDialog.FileName;
            }

            List<Entry> entries = mainForm.GetEntries(EntryFilter.AllEntries);
            bool extendedM3u = chkExtendedM3u.Checked;
            bool ignoreErrors = chkIgnoreErrors.Checked;

            var playlistsWithErrors = new List<string>();

            foreach (Playlist p in lstPlaylists.SelectedItems)
            {
                Debug.Log("Building playlist \"" + p.name + "\"");

                // Create a StringBuilder to hold the playlist text
                var sb = new System.Text.StringBuilder();

                // Amount of errors that occured while building
                int errors = 0;

                if (extendedM3u)
                {
                    sb.AppendLine("#EXTM3U");
                }

                if (p.playlistItems == null || p.playlistItems.Length == 0)
                {
                    Debug.LogWarning("Playlist has no items!");
                    continue;
                }

                foreach (int itemId in p.playlistItems)
                {
                    Entry entry = null;

                    // Loop over entries and find first with the itemId
                    for (int i = 0; i < entries.Count; i++)
                    {
                        if (entries[i].trackId == itemId)
                        {
                            entry = entries[i];
                            break;
                        }
                    }

                    // An item was not found (this should never happen)
                    if (entry == null)
                    {
                        Debug.LogWarning("\t-> Track with ID " + itemId + " does not exist!");
                        errors++;
                        continue;
                    }
                    else if (!entry.isMapped)
                    {
                        Debug.LogWarning("\t-> Entry \"" + entry.fileName + " not mapped!\"");
                        errors++;
                        continue;
                    }

                    if (extendedM3u)
                        sb.AppendLine("#EXTINF:" + entry.totalTime + ", " + entry.artist + " - " + entry.trackTitle);

                    sb.AppendLine(entry.mappedFilePath + "\n");
                }

                if (errors > 0)
                {
                    Debug.LogError("Errors occured while contructing this playlist, " + (ignoreErrors ? "saving anyway" : "and was not saved"));
                    playlistsWithErrors.Add(p.name);

                    if (!ignoreErrors) continue;
                }

                string text = sb.ToString();

                // Check if the text contains UTF-8 characters, and if so we need to export as an m3u8 so that foobar2000 properly parses the unicode characters
                bool isAscii = System.Text.Encoding.UTF8.GetByteCount(text) == text.Length;
                string ext = isAscii ? ".m3u" : ".m3u8";

                string saveFilePath = "";

                if (playlistCount == 1)
                    saveFilePath = Path.ChangeExtension(savePath, ext);
                else
                {
                    string validName = p.name;

                    foreach (char c in Path.GetInvalidFileNameChars())
                        validName = validName.Replace(c, '_');

                    saveFilePath = Path.Combine(savePath, validName + ext);
                }

                // Save playlist to file
                File.Delete(saveFilePath);
                File.WriteAllText(saveFilePath, sb.ToString());

                Debug.Log("Wrote " + p.name + " to " + saveFilePath);
            }

            if (playlistsWithErrors.Count > 0)
            {
                Debug.LogError("A number of errors occured while building the following playlist(s):\n " + string.Join("\n", playlistsWithErrors.ToArray()) + "\n\nConsult the log for more information", true);
            }

            // If some playlists succeeded, open in explored
            if (playlistsWithErrors.Count < lstPlaylists.SelectedItems.Count)
            {
                // Open in explorer
                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + savePath + "\"");
            }
        }
    }
}
