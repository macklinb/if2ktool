using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace if2ktool
{
    public partial class Main : Form
    {
        // --- Private Variables ---
        private SortableBindingList<Entry> entries { get; set; }
        private List<Playlist> playlists { get; set; }

        private int dgvEntriesRowHeight;
        private int dgvScrollOffsetFromSelection;

        // --- Static Variables ---
        public static string sourceLibraryFolderPath { get; private set; }
        public static string sourceLibraryXmlPath { get; private set; }
        public static bool shouldInhibitDataGridViewSelectionEvent;

        public static bool libraryXmlIsMacForm { get; private set; }

        // Event which is fired when the marked state of any row is changed
        public EventHandler<MarkedChangedEventArgs> MarkedChanged;

        public struct MarkedChangedEventArgs
        {
            public int newMarkedCount;
        }

        public Main()
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            dgvEntries.AutoGenerateColumns = false;

            // Double buffer DataGridView to reduce flicker
            dgvEntries.DoubleBuffered(true);

            // Stop redrawing window when resizing, since it causes lag when trying to resize sometimes hundreds of thousands of rows/columns
            this.ResizeBegin += (s, e) => this.SuspendLayout();
            this.ResizeEnd += (s, e) => this.ResumeLayout(true);

            // Double buffer TableLayoutPanel to reduce flicker
            tableLayoutPanel.DoubleBuffered(true);

            // Disable horizontal scrollbar in tableLayoutPanel
            tableLayoutPanel.HorizontalScroll.Maximum = 0;
            tableLayoutPanel.HorizontalScroll.Visible = false;
            tableLayoutPanel.AutoScroll = true;
            //splitContainerVertical.SplitterDistance += 15;
            //splitContainerVertical.SplitterDistance -= 5;

            EnumUtils.InitComboBoxWithEnum(cbRatingValue, typeof(Rating));
            EnumUtils.InitComboBoxWithEnum(cbRatingValue, typeof(Rating));

            // Disable scrolling in cbRatingValue
            cbRatingValue.MouseWheel += (s, e) =>
            {
                if (!cbRatingValue.Focused)
                    ((HandledMouseEventArgs)e).Handled = true;
            };

            dtpDateAddedValue.MouseDoubleClick += dtpProperty_MouseDoubleClick;
            dtpLastPlayedValue.MouseDoubleClick += dtpProperty_MouseDoubleClick;

            // Prevent Control-C from terminating the form process
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                e.Cancel = true;
            };

            LoadStatesFromSettings();
            InitDataGridView();
            //CreateDateTimePickers();
        }

        // Load form state from settings. Call this to update form stuffs from settings
        public void LoadStatesFromSettings()
        {
            dgvEntries.AllowUserToResizeRows = Settings.Current.mainAllowEditRowHeight;

            // Set some TagLib options
            if (Settings.Current.forceID3v2Version != ID3v2Version.None)
            {
                TagLib.Id3v2.Tag.DefaultVersion = Convert.ToByte(Settings.Current.forceID3v2Version);
                TagLib.Id3v2.Tag.ForceDefaultVersion = true;
            }

            TagLib.Id3v2.Tag.UseNumericGenres = Settings.Current.useNumericGenresID3v2;
        }

        // Initialize some stuff that can't be done in the designer
        // The comment blocks are to save Visual Studio from deleting columns, as it tends to
        private void InitDataGridView()
        {
            colChecked.Tag = "Checked";
            colWroteTags.Tag = "Wrote tags?";
            colTrackNumber.Tag = "Track No.";

            // Modify the rating column to display the dropdown box using the Description attribute
            var ratingColumn = (DataGridViewComboBoxColumn)dgvEntries.Columns["colRating"];
            ratingColumn.HeaderText = "Rating";
            ratingColumn.ValueMember = "Value";
            ratingColumn.DisplayMember = "Description";
            ratingColumn.DataSource = Enum.GetValues(typeof(Rating))
            .Cast<Enum>()
            .Select(value => new
            {
                ((DescriptionAttribute)Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute))).Description,
                value
            })
            .OrderBy(item => item.value)
            .ToList();

            /*
            // Programatically create a DateTimePicker column for DateAdded and LastPlayed
            var dateAddedColumn = new DataGridViewDateTimeColumn();
            dateAddedColumn.HeaderText = "Date Added";
            dateAddedColumn.DataPropertyName = "dateAdded";
            dateAddedColumn.ValueType = typeof(DateTime);
            dateAddedColumn.Width = 120;

            int dateAddedColumnIndex = dgvEntries.Columns["dateAddedDataGridViewTextBoxColumn"].Index;
            dgvEntries.Columns.RemoveAt(dateAddedColumnIndex);
            dgvEntries.Columns.Insert(dateAddedColumnIndex, dateAddedColumn);

            var lastPlayedColumn = new DataGridViewDateTimeColumn();
            lastPlayedColumn.HeaderText = "Last Played";
            lastPlayedColumn.DataPropertyName = "lastPlayed";
            lastPlayedColumn.ValueType = typeof(DateTime);
            lastPlayedColumn.Width = 120;

            int lastPlayedColumnIndex = dgvEntries.Columns["lastPlayedDataGridViewTextBoxColumn"].Index;
            dgvEntries.Columns.RemoveAt(lastPlayedColumnIndex);
            dgvEntries.Columns.Insert(lastPlayedColumnIndex, lastPlayedColumn);
            */

            // Set the contextMenuStrip on all headers
            foreach (DataGridViewColumn col in dgvEntries.Columns)
                col.HeaderCell.ContextMenuStrip = columnHeaderContextMenu;
        }

        // Visual Studio kept randomly deleting all of my DateTimePickers from Main.Designer, and it was really pissing me off so I have to manually create and initialize the DateTimePickerAlt's here instead
        private void CreateDateTimePickers()
        {
            dtpDateAddedValue = new DateTimePickerAlt();
            this.dtpDateAddedValue.CheckedLast = false;
            this.dtpDateAddedValue.CustomFormat = "MM\'/\'dd\'/\'yyyy hh\':\'mm tt";
            this.dtpDateAddedValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateAddedValue.Location = new System.Drawing.Point(83, 213);
            this.dtpDateAddedValue.Margin = new System.Windows.Forms.Padding(0);
            this.dtpDateAddedValue.Name = "dtpDateAddedValue";
            this.dtpDateAddedValue.ShowCheckBox = true;
            this.dtpDateAddedValue.Size = new System.Drawing.Size(171, 20);
            this.dtpDateAddedValue.TabIndex = 3;
            this.dtpDateAddedValue.ValueChangedSpecial += new System.EventHandler(this.dtpProperty_ValueChangedSpecial);
            this.dtpDateAddedValue.Enter += new System.EventHandler(this.dtpProperty_Enter);
            this.dtpDateAddedValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpProperty_KeyDown);
            this.dtpDateAddedValue.Leave += new System.EventHandler(this.dtpProperty_Leave);
            this.dtpDateAddedValue.Dock = DockStyle.Fill;

            dtpLastPlayedValue = new DateTimePickerAlt();
            this.dtpLastPlayedValue.CheckedLast = false;
            this.dtpLastPlayedValue.CustomFormat = "MM\'/\'dd\'/\'yyyy hh\':\'mm tt";
            this.dtpLastPlayedValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLastPlayedValue.Location = new System.Drawing.Point(83, 234);
            this.dtpLastPlayedValue.Margin = new System.Windows.Forms.Padding(0);
            this.dtpLastPlayedValue.Name = "dtpLastPlayedValue";
            this.dtpLastPlayedValue.ShowCheckBox = true;
            this.dtpLastPlayedValue.Size = new System.Drawing.Size(171, 20);
            this.dtpLastPlayedValue.TabIndex = 4;
            this.dtpLastPlayedValue.ValueChangedSpecial += new System.EventHandler(this.dtpProperty_ValueChangedSpecial);
            this.dtpLastPlayedValue.Enter += new System.EventHandler(this.dtpProperty_Enter);
            this.dtpLastPlayedValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpProperty_KeyDown);
            this.dtpLastPlayedValue.Leave += new System.EventHandler(this.dtpProperty_Leave);
            this.dtpLastPlayedValue.Dock = DockStyle.Fill;

            this.tableLayoutPanel.Controls.Add(this.dtpDateAddedValue, 1, 11);
            this.tableLayoutPanel.Controls.Add(this.dtpLastPlayedValue, 1, 12);
        }

        private void LoadLibraryXML(string path)
        {
            // Exit if file doesn't exist
            if (!System.IO.File.Exists(path))
            {
                Debug.LogError("File doesn't exist at " + path, true);
                return;
            }

            Debug.Log("Loading XML from \"" + path + "\"");

            XDocument xdoc = null;
            XElement tracksRoot = null;
            XElement playlistArray = null;

            // Attempt to load XML document
            try
            {
                xdoc = XDocument.Load(path);
            }
            catch (Exception e)
            {
                Debug.LogError("An exception occurred while loading the XML document. Is it a well-formed XML?\n" + e.Message, true);
                return;
            }

            // Exit if isn't XML, or if isn't plist formed, or if the document has no dict under root plist
            if (xdoc == null || xdoc.Elements().Count() == 0 || xdoc.Root.Name != Consts.ELEMENT_PLIST || xdoc.Root.Element("dict") == null)
            {
                Debug.LogError("File doesn't seem to be an iTunes Library XML", true);
                return;
            }

            // Loop over "keys" under root/plist -> dict
            // Get sourceLibraryPath and tracksRoot
            foreach (var key in xdoc.Root.Element("dict").Elements("key"))
            {
                if (key.Value == Consts.PLIST_KEY_MUSIC_LIBRARY)
                {
                    sourceLibraryFolderPath = PlistHelper.GetValueOfKey(key) + "Music";

                    // Remove URL encoding, but keep URI form/scope etc.
                    sourceLibraryFolderPath = Uri.UnescapeDataString(sourceLibraryFolderPath);

                    // Remove "file://localhost" and replace '/' with '\\'
                    sourceLibraryFolderPath = sourceLibraryFolderPath.Substring(17).Replace('/', '\\');

                    // Check if the library XML is from a Mac (sourceLibraryPath will not contain drive portion)
                    libraryXmlIsMacForm = sourceLibraryFolderPath[2] != ':';
                }
                else if (key.Value == Consts.PLIST_KEY_TRACKS)
                {
                    tracksRoot = (XElement)key.NextNode;

                    // Check if there was no tracksRoot
                    if (tracksRoot == null || tracksRoot.Name != "dict")
                    {
                        Debug.LogError("File is malformed (no <dict> after <key>Tracks</key>)", true);
                        return;
                    }
                }
                else if (key.Value == Consts.PLIST_KEY_PLAYLISTS)
                {
                    playlistArray = (XElement)key.NextNode;

                    // Check if there was no playlistArray
                    if (playlistArray == null || playlistArray.Name != "array")
                    {
                        Debug.LogWarning("No playlists array node found! Playlists will not be available.", true);
                    }
                }
            }

            // Check if we found a sourceLibraryPath
            if (string.IsNullOrEmpty(sourceLibraryFolderPath))
            {
                Debug.LogError("Failed to find key \"" + Consts.PLIST_KEY_MUSIC_LIBRARY + "\", in XML - required for remapping to know how much of the track path to replace.", true);
                return;
            }

            // Check if we found a tracksRoot
            if (tracksRoot == null)
            {
                Debug.LogError("File is malformed (no key <key>Tracks</key> under root <dict>)", true);
                return;
            }

            // Load entries, creating an Entry for each of them.

            Debug.Log("Loading entries...");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            entries = new SortableBindingList<Entry>();

            // Loop over "dict" elements following "Tracks"
            foreach (var track in tracksRoot.Descendants("dict"))
            {
                // Skip entries that are not files
                if (PlistHelper.GetValueOfKeyInDict(track, Consts.PLIST_KEY_TRACK_TYPE) != Consts.VALID_TRACK_TYPE)
                    continue;

                // Skip entries that are videos
                //if (TrackInfo.GetValueOfKeyInDict(track, Consts.PLIST_KEY_HAS_VIDEO) == "true")
                //    continue;

                // Skip entries that are podcasts
                if (PlistHelper.GetValueOfKeyInDict(track, Consts.PLIST_KEY_PODCAST) == "true")
                    continue;

                var entry = new Entry(track);
                entries.Add(entry);
            }

            sw.Stop();
            Debug.Log("Loaded " + entries.Count + " entries in " + sw.ElapsedMilliseconds + "ms");

            // Load playlists
            Debug.Log("Loading playlists...");
            sw.Restart();
            playlists = new List<Playlist>();

            foreach (var playlistDict in playlistArray.Elements("dict"))
            {
                // Skip master playlist
                if (PlistHelper.IsKeyInDict(playlistDict, "Master"))
                    continue;

                // Skip music playlist
                if (PlistHelper.IsKeyInDict(playlistDict, "Music"))
                    continue;

                // Skip podcast playlist
                if (PlistHelper.IsKeyInDict(playlistDict, "Podcasts"))
                    continue;

                // Skip audiobooks playlist
                if (PlistHelper.IsKeyInDict(playlistDict, "Audiobooks"))
                    continue;

                var playlist = new Playlist(playlistDict);
                playlists.Add(playlist);
            }

            sw.Stop();
            Debug.Log("Loaded " + playlists.Count + " playlists in " + sw.ElapsedMilliseconds + "ms");

            // Loop over "dict" elements in "array" element under "Playlists"
            Debug.Log("Creating TreeView for entries...");

            sourceLibraryXmlPath = path;

            //tableLayoutPanel.Enabled = true;
            dgvEntries.DataSource = entries;

            // Set the "x selected, x marked" label in the bottom right
            SetSelectedAndMarkedLabel(dgvEntries.SelectedRows.Count, GetEntriesCount(EntryFilter.Marked));

            // Set the window name
            this.Text = sourceLibraryXmlPath + " - if2ktool";

            GC.Collect();

            // --- Create TreeView ---

            // Remove any existing nodes in the TreeView
            treeViewEntries.Nodes.Clear();

            // Create TreeView in new task
            Task.Run(() =>
            {
                sw.Restart();

                string artistNodeKey = "Unknown Artist";
                string albumNodeKey = "Unknown Artist";

                TreeNode rootNode = new TreeNode();

                // Sorting the entries in advance is a LOT faster than calling .Sort on the TreeView
                var sortedEntries = GetEntries(EntryFilter.AllEntries)
                                        .OrderBy(x => x.compilation == false ? x.albumArtist : string.Empty)
                                        .ThenBy(x => x.album)
                                        .ThenBy(x => x.fileName);

                foreach (var e in sortedEntries)
                {
                    // Get artist and album node keys, and create a trackNode
                    if (e.compilation)
                        artistNodeKey = "Compilations";
                    else
                        artistNodeKey = e.albumArtist;

                    albumNodeKey = e.album;

                    // If a node for this artist doesn't exist, create it
                    if (rootNode.Nodes[artistNodeKey] == null)
                        rootNode.Nodes.Add(artistNodeKey, artistNodeKey);

                    // If a node for this album doesn't exist under the artist, create it
                    if (rootNode.Nodes[artistNodeKey].Nodes[albumNodeKey] == null)
                        rootNode.Nodes[artistNodeKey].Nodes.Add(albumNodeKey, albumNodeKey);

                    // Add the track to the tree under Artist -> Album
                    rootNode.Nodes[artistNodeKey].Nodes[albumNodeKey].Nodes.Add(e.trackId.ToString(), e.fileName);
                }

                // Invoke addition of TreeNodes to treeViewEntries in the context of it's own thread
                treeViewEntries.Invoke(() =>
                {
                    treeViewEntries.Nodes.AddRange(rootNode.Nodes.Cast<TreeNode>().ToArray());

                    // ! Replaced with AddRange
                    //foreach (TreeNode node in rootNode.Nodes)
                        //treeViewEntries.Nodes.Add(node);

                    // ! Don't need since the entries are sorted in advance
                    //treeViewEntries.Sort();
                });

                Debug.Log("Generated TreeView in " + sw.ElapsedMilliseconds + "ms");
            });
        }

        void CloseLibrary()
        {
            MappingWorker.StopWorker();
            TagWriterWorker.StopWorker();

            entries.Clear();
            playlists.Clear();
            treeViewEntries.Nodes.Clear();
            
            dgvEntries.ClearSelection();
            UpdateProperties();

            sourceLibraryFolderPath = null;
            sourceLibraryXmlPath = null;
            libraryXmlIsMacForm = false;

            this.Text = "if2ktool";

            GC.Collect();
        }

        // --- Misc Methods ---

        // Get subset of entries based on the EntryFilter with LINQ (instead of individually discounting every entry that doesn't meet criteria)
        public List<DataGridViewRow> GetRows(EntryFilter filter, bool NOT = false)
        {
            switch (filter)
            {
                case EntryFilter.AllEntries:
                {
                    if (NOT)
                        return null;
                    else
                        return dgvEntries.Rows.Cast<DataGridViewRow>().ToList();
                }
                case EntryFilter.Selection:
                {
                    if (NOT)
                        return dgvEntries.Rows.Cast<DataGridViewRow>().Except(dgvEntries.SelectedRows.Cast<DataGridViewRow>()).ToList();
                    else
                        return dgvEntries.SelectedRows.Cast<DataGridViewRow>().ToList();
                }
                case EntryFilter.Marked:
                {
                    return dgvEntries.Rows.Cast<DataGridViewRow>().Where(x => ((Entry)x.DataBoundItem).isChecked == !NOT).ToList();
                }
                case EntryFilter.Mapped:
                {
                    return dgvEntries.Rows.Cast<DataGridViewRow>().Where(x => ((Entry)x.DataBoundItem).isMapped == !NOT).ToList();
                }
                case EntryFilter.Saved:
                {
                    return dgvEntries.Rows.Cast<DataGridViewRow>().Where(x => ((Entry)x.DataBoundItem).wroteTags == !NOT).ToList();
                }
                default:
                    return null;
            }
        }

        public List<Entry> GetEntries(EntryFilter filter, bool NOT = false)
        {
            switch (filter)
            {
                case EntryFilter.AllEntries:
                {
                    if (NOT)
                        return null;
                    else
                        return entries.ToList();
                }
                case EntryFilter.Selection:
                {
                    return GetRows(filter, NOT).Select(x => (Entry)x.DataBoundItem).ToList();
                }
                case EntryFilter.Marked:
                {
                    return entries.Where(x => x.isChecked == !NOT).ToList();
                }
                case EntryFilter.Mapped:
                {
                    return entries.Where(x => x.isMapped == !NOT).ToList();
                }
                case EntryFilter.Saved:
                {
                    return entries.Where(x => x.wroteTags == !NOT).ToList();
                }
                default:
                    return null;
            }
        }

        // Get the count of entries based on an EntryFilter
        public int GetEntriesCount(EntryFilter filter, bool NOT = false)
        {
            if (entries == null || dgvEntries.Rows.Count == 0)
                return 0;

            return
                filter == EntryFilter.AllEntries && NOT == false ?
                    entries.Count :
                filter == EntryFilter.Selection ?
                    NOT ? dgvEntries.Rows.Count - dgvEntries.SelectedRows.Count : dgvEntries.SelectedRows.Count :
                filter == EntryFilter.Marked ?
                    entries.Count(x => x.isChecked == !NOT) :
                filter == EntryFilter.Mapped ?
                    entries.Count(x => x.isMapped == !NOT) :
                filter == EntryFilter.Saved ?
                    entries.Count(x => x.wroteTags == !NOT) : 0;
        }

        private void SetMarkedStateOfRow(DataGridViewRow row, bool state)
        {
            ((Entry)row.DataBoundItem).isChecked = state;

            // Invalidate cells so they get repainted
            dgvEntries.InvalidateCell(colChecked.Index, row.Index);
        }

        private void SetMarkedStateOfRows(IEnumerable<DataGridViewRow> rows, bool state)
        {
            foreach (DataGridViewRow row in rows)
                SetMarkedStateOfRow(row, state);

            // Refresh DataGridView edits
            dgvEntries.Update();

            // Update marked count label
            SetMarkedLabel();
        }

        public void SetSelectedLabel(int value = -1)
        {
            if (value == -1)
                value = dgvEntries.SelectedRows.Count;

            SetSelectedAndMarkedLabel(value, -1);
        }

        public void SetMarkedLabel(int value = -1)
        {
            if (value == -1)
                value = entries.Count(x => x.isChecked);

            // Invoke dgvEntries_MarkedChanged
            if (MarkedChanged != null)
                MarkedChanged.Invoke(this, new MarkedChangedEventArgs() { newMarkedCount = value });

            SetSelectedAndMarkedLabel(-1, value);
        }

        public void SetSelectedAndMarkedLabel(int selected = -1, int marked = -1)
        {
            string[] labels = lblSelectedCount.Text.Split(new string[] { ", " }, StringSplitOptions.None);

            if (selected != -1)
                labels[0] = selected + " selected";
            if (marked != -1)
                labels[1] = marked + " marked";

            lblSelectedCount.Text = string.Join(", ", labels);
        }

        // This selects the row at the index
        public void SetRowSelection(int index, bool inhibitEvents)
        {
            if (inhibitEvents)
                shouldInhibitDataGridViewSelectionEvent = true;

            if (index >= 0 && index < dgvEntries.RowCount && dgvEntries.DisplayedRowCount(false) > 0)
            {
                if (Settings.Current.workerScrollWithSelection)
                {
                    dgvEntries.ClearSelection();
                    dgvEntries.Rows[index].Selected = true;

                    int scrollingRowIndex = -1;
                    /*
                    if (Settings.Current.workerLockViewToSelection)
                    {
                        if (dgvScrollOffsetFromSelection > 0)
                            dgvScrollOffsetFromSelection = 0;
                        else if (dgvScrollOffsetFromSelection < -dgvEntries.DisplayedRowCount(false))
                            dgvScrollOffsetFromSelection = -dgvEntries.DisplayedRowCount(false);
                    }
                    */

                    // If the selected row is still within view, keep scrolling with the selection - otherwise detach from it
                    if (dgvScrollOffsetFromSelection <= 0 && dgvScrollOffsetFromSelection >= -dgvEntries.DisplayedRowCount(false))
                    {
                        scrollingRowIndex = index + dgvScrollOffsetFromSelection;
                    }

                    // If we should lock the view within the selection
                    else if (Settings.Current.workerLockViewToSelection)
                    {
                        if (dgvScrollOffsetFromSelection < 0)
                            scrollingRowIndex = index - dgvEntries.DisplayedRowCount(false);
                        else if (dgvScrollOffsetFromSelection > 0)
                            scrollingRowIndex = index;
                    }

                    // Clamp the scrollingRowIndex to the range of DataGridView rows
                    if (scrollingRowIndex >= 0 && scrollingRowIndex < dgvEntries.RowCount)
                        dgvEntries.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
                }
            }

            if (inhibitEvents)
                shouldInhibitDataGridViewSelectionEvent = false;
        }

        public void SetProgress(int count, int max, long timeMs)
        {
            float percentageFloat = ((float)count / (float)max) * 100;
            progressBar.Value = (int)percentageFloat;
            lblStatus.Text = string.Format("{0}/{1} ({2:0.000}%) - {3}", count, max, percentageFloat, TimeSpan.FromMilliseconds(timeMs).ToString("m\\:ss"));
        }

        // Show or hide the progress bar. This also readies the main form for worker processing
        public void ShowProgress(bool show)
        {
            lblStatus.Text = string.Empty;
            progressBar.Value = 0;
            lblStatus.Visible = show;
            progressBar.Visible = show;

            // Show/hide properties table
            tableLayoutPanel.Enabled = !show;

            // Disable sorting of rows
            foreach (DataGridViewColumn column in dgvEntries.Columns)
                column.SortMode = show ? DataGridViewColumnSortMode.NotSortable : DataGridViewColumnSortMode.Automatic;
        }

        // This method updates the leftmost "Properties" panel with information from the selected rows in the DataGridView
        public void UpdateProperties()
        {
            if (dgvEntries.SelectedRows.Count > 0)
            {
                var selected = GetEntries(EntryFilter.Selection);
                Entry firstEntry = selected[0];

                // Track title
                lblTitleValue.Text = selected.Any(x => x.trackTitle != firstEntry.trackTitle) ? Consts.NOT_EQUAL_CHAR : firstEntry.trackTitle;

                // Artist
                lblArtistValue.Text = selected.Any(x => x.artist != firstEntry.artist) ? Consts.NOT_EQUAL_CHAR : firstEntry.artist;

                // Album artist
                lblAlbumArtistValue.Text = selected.Any(x => x.albumArtist != firstEntry.albumArtist) ? Consts.NOT_EQUAL_CHAR : firstEntry.albumArtist;

                // Album
                lblAlbumValue.Text = selected.Any(x => x.album != firstEntry.album) ? Consts.NOT_EQUAL_CHAR : firstEntry.album;

                // Year
                lblYearValue.Text = selected.Any(x => x.year != firstEntry.year) ? Consts.NOT_EQUAL_CHAR : firstEntry.year;

                // Track number
                lblTrackNumValue.Text = selected.Any(x => x.trackNumber != firstEntry.trackNumber) ? Consts.NOT_EQUAL_CHAR : firstEntry.trackNumber;

                // File name
                lblFileNameValue.Text = selected.Any(x => x.fileName != firstEntry.fileName) ? Consts.NOT_EQUAL_CHAR : firstEntry.fileName;

                // File path
                //lblFilePathValue.Text = selected.Any(x => x.location != firstEntry.location) ? Consts.NOT_EQUAL_CHAR :
                //"..." + firstEntry.location.Replace(sourceLibraryPath, "").Replace('/', '\\'); ;

                // To save time, get the common path of two random paths first, before processing the entire array
                if (selected.Count > 1000)
                {
                    var subSample = new List<string>();
                    subSample.Add(selected[0].relativeFilePath);
                    subSample.Add(selected[new Random().Next(1, selected.Count - 1)].relativeFilePath);

                    if (string.IsNullOrEmpty(FindCommonPath("\\", subSample)))
                        lblFilePathValue.Text = Consts.NOT_EQUAL_CHAR;

                    // Proceed normally
                    else
                        lblFilePathValue.Text = FindCommonPath("\\", selected.Select(x => x.relativeFilePath).ToList());
                }
                else
                    lblFilePathValue.Text = FindCommonPath("\\", selected.Select(x => x.relativeFilePath).ToList());

                // Mapped file path
                //txtMappedFilePathValue.Text = selected.Any(x => x.mappedFilePath != firstEntry.mappedFilePath) ? Consts.NOT_EQUAL_CHAR : firstEntry.mappedFilePath;
                txtMappedFilePathValue.Text = FindCommonPath("\\", selected.Select(x => x.mappedFilePath).ToList());

                // Lookup index
                lblLookupIndexValue.Text =
                    selected.Any(x => x.lookupIndex != firstEntry.lookupIndex) ? Consts.NOT_EQUAL_CHAR : (firstEntry.lookupIndex == -1 ? "-" : firstEntry.lookupIndex.ToString());

                // Date added
                // If any of the selected values are different, show the not-equals sign
                if (selected.Any(x => x.dateAdded != firstEntry.dateAdded))
                {
                    // Uncheck the dtp if a majority of rows have dateAddedDisabled
                    if (selected.Count(x => x.dateAddedDisabled) > selected.Count / 2)
                        dtpDateAddedValue.Checked = false;
                    else
                        dtpDateAddedValue.Checked = true;
                    
                    dtpDateAddedValue.CustomFormat = Consts.NOT_EQUAL_CHAR;
                }
                // Show the DateTime
                else
                {
                    dtpDateAddedValue.CustomFormat = Consts.DATE_TIME_FORMAT;

                    // If the date is out of range, show the minimum possible date
                    if (firstEntry.dateAdded < dtpDateAddedValue.MinDate ||
                        firstEntry.dateAdded > dtpDateAddedValue.MaxDate)
                    {
                        dtpDateAddedValue.Value = dtpDateAddedValue.MinDate;
                    }
                    else
                    {
                        dtpDateAddedValue.Value = firstEntry.dateAdded;
                    }

                    // If the dateAdded is supposed to be disabled, uncheck the box
                    dtpDateAddedValue.Checked = !firstEntry.dateAddedDisabled;
                }

                // Last played
                // If any of the selected values are different, show the not-equals sign
                if (selected.Any(x => x.lastPlayed != firstEntry.lastPlayed))
                {
                    // Uncheck the dtp if a majority of rows have lastPlayedDisabled
                    if (selected.Count(x => x.lastPlayedDisabled) > selected.Count / 2)
                        dtpLastPlayedValue.Checked = false;
                    else
                        dtpLastPlayedValue.Checked = true;
                    
                    dtpLastPlayedValue.CustomFormat = Consts.NOT_EQUAL_CHAR;
                }

                // If the date is out of range, show an empty value
                else if (firstEntry.lastPlayed < dtpLastPlayedValue.MinDate ||
                         firstEntry.lastPlayed > dtpLastPlayedValue.MaxDate)
                {
                    //dtpLastPlayedValue.CustomFormat = " ";
                    dtpLastPlayedValue.Checked = !firstEntry.lastPlayedDisabled;
                }
                else
                {
                    dtpLastPlayedValue.CustomFormat = Consts.DATE_TIME_FORMAT;
                    dtpLastPlayedValue.Value = firstEntry.lastPlayed;

                    // If the lastPlayed is supposed to be disabled, uncheck the box
                    dtpLastPlayedValue.Checked = !firstEntry.lastPlayedDisabled;
                }

                // Ensure that CheckedLast is the same
                dtpDateAddedValue.CheckedLast = dtpDateAddedValue.Checked;
                dtpLastPlayedValue.CheckedLast = dtpLastPlayedValue.Checked;

                // Play count
                // If any of the selected values are different, hide the NumericUpDown component and show a label
                if (selected.Any(x => x.playCount != firstEntry.playCount))
                {
                    txtPlayCountValue.Text = Consts.NOT_EQUAL_CHAR;
                }
                else
                {
                    txtPlayCountValue.Text = firstEntry.playCount.ToString();
                }

                // Rating
                if (selected.Any(x => x.rating != firstEntry.rating))
                    cbRatingValue.SelectedValue = Rating.Unrated;
                else
                    cbRatingValue.SelectedValue = (Rating)firstEntry.rating;

                dtpPropertyDirty = false;
            }
            else
            {
                lblTitleValue.Text = lblArtistValue.Text = lblAlbumArtistValue.Text = lblAlbumValue.Text = lblYearValue.Text = lblTrackNumValue.Text = lblFileNameValue.Text = lblFilePathValue.Text = lblMappedFilePath.Text = lblLookupIndexValue.Text = txtPlayCountValue.Text = "-";
                dtpDateAddedValue.Value = dtpLastPlayedValue.Value = DateTime.Now;
                dtpDateAddedValue.Checked = dtpLastPlayedValue.Checked = false;
                cbRatingValue.SelectedValue = Rating.Unrated;
            }
        }

        public static string FindCommonPath(string separator, List<string> paths)
        {
            // If there are no paths, return an empty string
            if (paths == null || paths.Count == 0)
                return string.Empty;

            // If there's only 1 path, just return it
            if (paths.Count == 1)
                return paths[0];

            // Remove null elements
            paths = paths.Where(x => !string.IsNullOrEmpty(x)).ToList();

            // If there are no paths, return an empty string (again)
            if (paths == null || paths.Count == 0)
                return string.Empty;

            string commonPath = String.Empty;

            List<string> separatedPath = paths
                .First(str => str.Length == paths.Max(st2 => st2.Length))
                .Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            foreach (string pathSegment in separatedPath.AsEnumerable())
            {
                if (commonPath.Length == 0 && paths.All(str => str.StartsWith(pathSegment)))
                    commonPath = pathSegment;
                else if (paths.All(str => str.StartsWith(commonPath + separator + pathSegment)))
                    commonPath += separator + pathSegment;
                else
                    break;
            }

            return commonPath;
        }

        #region Control Events

        // --- ToolStripMenuItem Events ---

        // File DropDownOpening event
        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            closeToolStripMenuItem.Enabled = !string.IsNullOrEmpty(sourceLibraryXmlPath);
            exportMtsToolStripMenuItem.Enabled = exportPlaylistsToolStripMenuItem.Enabled = dgvEntries.RowCount > 0;
        }

        // File, Open library XML...
        private void openLibraryXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Open iTunes Library XML",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "XML file|*.xml|All files|*"
            };

            if (!string.IsNullOrEmpty(Properties.Settings.Default.lastLibraryXmlPath))
            {
                string initialDir = Properties.Settings.Default.lastLibraryXmlPath;
                dialog.FileName = Path.GetFileName(initialDir);
                dialog.InitialDirectory = Path.GetDirectoryName(initialDir);
            }
            else
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadLibraryXML(dialog.FileName);
                Properties.Settings.Default.lastLibraryXmlPath = dialog.FileName;
            }
        }

        // File -> Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close the current library? All changes will be lost!", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                CloseLibrary();
        }

        // File -> Export masstagger mts...
        private void exportMtsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExportMasstaggerForm().ShowDialog();
        }

        // File -> Export playlist(s)...
        private void exportPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExportPlaylistForm(playlists).ShowDialog();
        }

        // Edit - Opening event for Edit menu
        // Enable and disable the menu items under Edit based on context
        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            // Only show "Check" if the selection has items unchecked
            bool selectionHasUncheckedItems =
                dgvEntries.SelectedRows.Cast<DataGridViewRow>()
                .Any(x => ((Entry)x.DataBoundItem).isChecked == false);

            // Only show "Uncheck" if the selection has items checked
            bool selectionHasCheckedItems =
                dgvEntries.SelectedRows.Cast<DataGridViewRow>()
                .Any(x => ((Entry)x.DataBoundItem).isChecked == true);

            checkToolStripMenuItem.Enabled = selectionHasUncheckedItems;
            uncheckToolStripMenuItem.Enabled = selectionHasCheckedItems;

            // Only show these items if we have entries
            toggleCheckedToolStripMenuItem.Enabled =
            selectAllToolStripMenuItem.Enabled =
            deselectAllToolStripMenuItem.Enabled =
            checkAllToolStripMenuItem.Enabled =
            uncheckAllToolStripMenuItem.Enabled =
            invertCheckedToolStripMenuItem.Enabled = dgvEntries.Rows.Count > 0;
        }

        // Edit -> Check: Set the marked state of selected rows to true
        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMarkedStateOfRows(dgvEntries.SelectedRows.Cast<DataGridViewRow>(), true);
        }

        // Edit -> Uncheck. Set the marked state of selected rows to false
        private void uncheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMarkedStateOfRows(dgvEntries.SelectedRows.Cast<DataGridViewRow>(), false);
        }

        // Edit -> Toggle checked. Toggles the check state of rows. If multiple rows are selected, we toggle on if the majority of cells are toggled off (and vice versa)
        private void toggleCheckedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get collection of selected checked and unchecked rows
            var checkedRows = dgvEntries.SelectedRows
                .Cast<DataGridViewRow>().Where(x => ((Entry)x.DataBoundItem).isChecked);
            var uncheckedRows = dgvEntries.SelectedRows
                .Cast<DataGridViewRow>().Where(x => !((Entry)x.DataBoundItem).isChecked);

            // If all rows are unchecked, check all rows
            if (checkedRows.Count() == 0)
                SetMarkedStateOfRows(uncheckedRows, true);

            // If all rows are checked, uncheck all rows
            else if (uncheckedRows.Count() == 0)
                SetMarkedStateOfRows(checkedRows, false);

            // Set checked state of checked rows to false if there are more unchecked rows
            else if (uncheckedRows.Count() >= checkedRows.Count())
                SetMarkedStateOfRows(checkedRows, false);

            // Set checked state of unchecked rows to true if there are more checked rows
            else if (checkedRows.Count() > uncheckedRows.Count())
                SetMarkedStateOfRows(uncheckedRows, true);
        }

        // Edit -> Select all
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvEntries.SelectAll();
        }

        // Edit -> Deselect all
        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvEntries.ClearSelection();
        }

        // Edit -> Check all
        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ask the user if they're sure
            if (MessageBox.Show("Check all rows?", "Check rows?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SetMarkedStateOfRows(dgvEntries.Rows.Cast<DataGridViewRow>(), true);
            }
        }
        
        // Edit -> Uncheck all
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ask the user if they're sure
            if (MessageBox.Show("Uncheck all rows?", "Uncheck rows?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SetMarkedStateOfRows(dgvEntries.Rows.Cast<DataGridViewRow>(), false);
            }
        }

        // Edit -> Invert checked. Set the marked state of all rows to the opposite of their current value
        private void invertCheckedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ask the user if they're sure
            if (MessageBox.Show("Invert checked state of all rows?", "Invert rows?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Commit any current edits
                dgvEntries.CommitEdit(DataGridViewDataErrorContexts.Commit);

                int checkColumnIndex = dgvEntries.Columns["colChecked"].Index;

                foreach (DataGridViewRow row in dgvEntries.Rows)
                {
                    var entry = (Entry)row.DataBoundItem;
                    entry.isChecked = !entry.isChecked;
                }

                dgvEntries.RefreshEdit();
                dgvEntries.InvalidateColumn(checkColumnIndex);

                // Update marked count label
                SetMarkedLabel();
            }
        }
        
        // Edit -> Preferences...
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var prefs = new PreferencesForm();
            prefs.ShowDialog();
        }

        // Drop down opening event for View
        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            showConsoleToolStripMenuItem.Checked = Settings.Current.showConsole;
        }
        
        // View -> Show console clicked
        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var show = showConsoleToolStripMenuItem.Checked;
            Settings.Current.showConsole = show;
            ConsoleHelper.ToggleConsole(show);
        }
        
        // Opening event form Tools menu
        // Used to enable/disable menu items based on if they can be started
        private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            mapToolStripMenuItem.Enabled = !MappingWorker.InProgress && dgvEntries.RowCount > 0;
            writeTagsToolStripMenuItem.Enabled = removeStatisticsTagsToolStripMenuItem.Enabled = !MappingWorker.InProgress && !TagWriterWorker.InProgress && dgvEntries.RowCount > 0;
        }

        // Tools -> Map entries to files
        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open new MatchForm, do not if one is already open
            if (Application.OpenForms.Cast<Form>().Any(f => f is MappingForm))
            {
                Application.OpenForms["MappingForm"].Focus();
            }
            else
            {
                var mappingForm = new MappingForm();
                mappingForm.Show();
            }
        }

        // Tools -> Write tags
        private void writeTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WriteTagsForm().Show();
        }

        // Tools -> Remove tags
        private void removeStatisticsTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WriteTagsForm(true).Show();
        }

        // Tools -> Generate lookup JSON
        private void generateJSONLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GenerateLookupJson().Show();
        }

        // Tools -> Generate replicate library structure
        private void generateReplicateLibraryStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GenerateReplicateLibraryStructure().ShowDialog();
        }

        // Action drop down open event

        private void actionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            pauseToolStripMenuItem.Enabled = cancelToolStripMenuItem.Enabled = MappingWorker.InProgress || TagWriterWorker.InProgress;

            if (MappingWorker.InProgress)
            {
                pauseToolStripMenuItem.Text = (MappingWorker.Paused ? "Unpause" : "Pause") + " mapping";
                cancelToolStripMenuItem.Text = "Cancel mapping";
            }
            else if (TagWriterWorker.InProgress)
            {
                pauseToolStripMenuItem.Text = (TagWriterWorker.Paused ? "Unpause" : "Pause") + " tag writer";
                cancelToolStripMenuItem.Text = "Cancel tag writer";
            }
            else
            {
                pauseToolStripMenuItem.Text = "Pause";
                cancelToolStripMenuItem.Text = "Cancel";
            }
        }
        
        // Action -> Pause/Unpause
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MappingWorker.InProgress)
                MappingWorker.Paused = !MappingWorker.Paused;
            else if (TagWriterWorker.InProgress)
                TagWriterWorker.Paused = !TagWriterWorker.Paused;
        }

        // Action -> Cancel
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool savedState = MappingWorker.Paused | TagWriterWorker.Paused;

            // Firstly, pause the worker to ask the user
            if (MappingWorker.InProgress) MappingWorker.Paused = true;
            else if (TagWriterWorker.InProgress) TagWriterWorker.Paused = true;

            if (MessageBox.Show("Cancel the current operation?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (MappingWorker.InProgress)
                    MappingWorker.StopWorker();
                if (TagWriterWorker.InProgress)
                    TagWriterWorker.StopWorker();
            }

            // Restore the last paused state
            if (MappingWorker.InProgress) MappingWorker.Paused = savedState;
            else if (TagWriterWorker.InProgress) TagWriterWorker.Paused = savedState;
        }

        // --- DataGridView Events ---

        // Called when the dgvEntries needs the row height. Provide the overridden row height
        private void dgvEntries_RowHeightInfoNeeded(object sender, DataGridViewRowHeightInfoNeededEventArgs e)
        {
            if (dgvEntriesRowHeight > 0 && e.Height != dgvEntriesRowHeight)
            {
                e.Height = dgvEntriesRowHeight;
            }
        }

        // Called when the user changes the height of a row. Limit the row height to a minimum of 18 pixels
        private void dgvEntries_RowHeightInfoPushed(object sender, DataGridViewRowHeightInfoPushedEventArgs e)
        {
            dgvEntriesRowHeight = Math.Max(e.Height, 18);
            dgvEntries.Invalidate();
        }

        // Event fires when the dirty state for the current cell changes (which occurs when the user changed something - but before saving the change)
        // This will immediately commit changes to the colChecked column to the DataBoundItem
        // If the option is enabled, also will propagates checkbox changes to other selected rows
        private void dgvEntries_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell cell = dgv.CurrentCell;

            if (cell.RowIndex >= 0)
            {
                if (cell.OwningColumn.Name == "colChecked")
                {
                    // Only propagate changes if there are more than one row selected
                    // Only to this if "Propogate cell edits" is checked
                    if (dgv.SelectedRows.Count > 1 && Settings.Current.mainPropagateCheckEdits)
                    {
                        // If checkbox value changed, copy it's value to all selectedrows
                        if (cell.EditedFormattedValue != null &&
                            !cell.EditedFormattedValue.Equals(cell.Value))
                        {
                            foreach (DataGridViewRow row in dgv.SelectedRows)
                                row.Cells[cell.ColumnIndex].Value = cell.EditedFormattedValue;
                        }
                    }

                    // Immediately commit and save the checkbox changes to the collection.
                    // This has the effect of saving when the value changes, not when the user leaves the row
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dgv.EndEdit();

                    SetMarkedLabel();
                }
                else if (cell.OwningColumn.ValueType == typeof(DateTime))
                {

                }
            }
        }

        // Fired when the user pressed a key while the dgvEntries control is focused
        // We use this to toggle the checked state when Space is pressed
        private void dgvEntries_KeyUp(object sender, KeyEventArgs e)
        {
            // Check key pressed was Space
            if (e.KeyCode == Keys.Space)
            {
                // Check to see if we actually have a selection
                if (dgvEntries.SelectedRows.Count > 0)
                {
                    // Fire the Edit -> Toggle checked event
                    toggleCheckedToolStripMenuItem_Click(this, new EventArgs());
                }
            }
        }
        
        DataGridViewSelectedRowCollection selected;
        int selectedColumnIndex = -1;

        // Used to maintain selection after sorting
        int[] selectedTrackIds;
        int savedSelectedColumn = -1;
        int lastSelectedTrackId = -1;

        // Event fired when the user mouses down on a cell
        private void dgvEntries_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 1. Save the track IDs of the current selection when we're sorting, so that we can re-select the correct rows after sort

            // If we clicked on the header to sort
            if (e.RowIndex == -1 && dgvEntries.SelectedRows.Count >= 1)
            {
                // Avoid doing this for more that 500 rows (as it ends up taking too long)
                if (dgvEntries.SelectedRows.Count > 5000)
                {
                    Debug.LogWarning("Too many rows selected to maintain post-sort selection in a timely manner!");
                    return;
                }
                
                // Inhibit selection events before sorting
                shouldInhibitDataGridViewSelectionEvent = true;

                // Firstly, save the selected column index
                savedSelectedColumn = dgvEntries.CurrentCell.ColumnIndex;

                // Save the CurrentRow's (the last row selected) track ID
                // This is the equivalent of the first row in the SelectedRows collection (aka the last row selected)
                // However, if we programatically set the selected rows, this will not update the CurrentRow. So check to see if the CurrentRow is selected first
                if (dgvEntries.CurrentRow.Selected == false)
                    lastSelectedTrackId = ((Entry)dgvEntries.SelectedRows[0].DataBoundItem).trackId;
                else
                    lastSelectedTrackId = ((Entry)dgvEntries.CurrentRow.DataBoundItem).trackId;

                // Initialize an array with the size of the selection
                // This is used to store the track ID's for each row in the selection
                selectedTrackIds = new int[GetEntriesCount(EntryFilter.Selection)];

                int firstDisplayedRowIndex = dgvEntries.FirstDisplayedScrollingRowIndex;
                int lastDisplayedRowIndex = Math.Min(firstDisplayedRowIndex + dgvEntries.DisplayedRowCount(false), dgvEntries.Rows.Count - 1);

                // Store the track ID (selectedIndex should be an index of the row in the SelectedRows array)
                void StoreTrackIdForRow(int selectedIndex)
                {
                    selectedTrackIds[selectedIndex] = ((Entry)dgvEntries.SelectedRows[selectedIndex].DataBoundItem).trackId;
                }

                if (dgvEntries.SelectedRows.Count <= 100)
                {
                    // If we're only dealing with a hundred rows or less, use a normal for loop - as it is faster in these instances
                    for (int i = 0; i < selectedTrackIds.Length; i++)
                        StoreTrackIdForRow(i);
                }
                else
                {
                    // Otherwise, use a Parallel.For, as it is faster for a large amount of rows, but has a startup penalty
                    Parallel.For(0, selectedTrackIds.Length, i => StoreTrackIdForRow(i));
                }
            }

            // 2. Save the multi-selection to a private variable.

            // Save multi-selection
            if (dgvEntries.SelectedRows.Count > 1)
            {
                selected = dgvEntries.SelectedRows;
                selectedColumnIndex = e.ColumnIndex;
            }

            // Clear saved multi-selection
            else
                selected = null;
        }

        // Fired when the selection changes
        private void dgvEntries_SelectionChanged(object sender, EventArgs e)
        {
            if (shouldInhibitDataGridViewSelectionEvent)
                return;

            // Update selected count label
            SetSelectedLabel(dgvEntries.SelectedRows.Count);

            // Update properties panel
            UpdateProperties();

            if (selectedColumnIndex == -1 || selected == null || selected.Count == 0)
                return;
            
            // This is a fix for the DataGridView deselecting our multi-selection when we check or uncheck the checkbox in the first row
            // To do this, we check to see if the click occurred in the checkbox cell, and if the cells are no longer selected, we re-select them
            // This relies on the selection being saved in dgvEntries_CellMouseDown
            if (dgvEntries.Columns[selectedColumnIndex].Name == "colChecked" &&
                selected.Count > 1 &&
                selected.Cast<DataGridViewRow>().Any(r => r.Selected == false))
            {
                // Set this value to true to prevent events subscribed to SelectionChanged from following through as a result of setting DataGridViewRow.Selected. This is necessary otherwise the SelectionChanged event will fire thousands of times
                shouldInhibitDataGridViewSelectionEvent = true;

                // Set all multi-selected rows back to selected
                foreach (DataGridViewRow row in selected)
                    row.Selected = true;

                shouldInhibitDataGridViewSelectionEvent = false;
            }
        }

        // Called when the cells are being formatted for their value
        private void dgvEntries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == colDateAdded.Index || e.ColumnIndex == colLastPlayed.Index) && e.RowIndex >= 0)
            {
                var entry = (Entry)dgvEntries.Rows[e.RowIndex].DataBoundItem;

                // If the cell is enabled (we use the tag object as an Enabled flag), yet the disabled value flag is true
                if ((e.CellStyle.Tag == null || (bool)e.CellStyle.Tag == true) &&
                    ((e.ColumnIndex == colDateAdded.Index && entry.dateAddedDisabled == true) ||
                     (e.ColumnIndex == colLastPlayed.Index && entry.lastPlayedDisabled == true)))
                {
                    e.CellStyle.ForeColor = Color.DarkGray;

                    // Set an "Enabled" flag to false to mark this cell style as disabled
                    e.CellStyle.Tag = false;
                }

                // If the cell is disabled, yet the disabled value disabled flag is false
                else if ((e.CellStyle.Tag != null && (bool)e.CellStyle.Tag == false) &&
                    ((e.ColumnIndex == colDateAdded.Index && entry.dateAddedDisabled == false) ||
                     (e.ColumnIndex == colLastPlayed.Index && entry.lastPlayedDisabled == false)))
                {
                    e.CellStyle.ForeColor = dgvEntries[e.ColumnIndex, e.RowIndex].OwningColumn.DefaultCellStyle.ForeColor;
                    e.CellStyle.Tag = true;
                }
            }
        }

        // Called when the DataGridView is scrolled
        private void dgvEntries_Scroll(object sender, ScrollEventArgs e)
        {
            if (shouldInhibitDataGridViewSelectionEvent)
                return;

            // Update the scroll offset. This is the offset of the first displayed row from the selected row, where negative offsets are above the selection
            // This is only used to offset the automatic scrolling when a worker is in progress, which is updated in SetRowSelection
            if (dgvEntries.SelectedRows.Count > 0)
                dgvScrollOffsetFromSelection = e.NewValue - dgvEntries.SelectedRows[0].Index;
        }

        // Called after the DataGridView has been sorted
        private void dgvEntries_Sorted(object sender, EventArgs e)
        {
            // Translate the selectedTrackId's back into indexes for selection
            if (selectedTrackIds != null)
            {
                // Initialize an list to store the row indexes of the tracks in the prior selection
                int[] rowIndexes = new int[selectedTrackIds.Length];

                // This is the number of rowIndexes we've found.
                int foundRowIndexesCount = 0;

                // The dgvEntries row index of the last selected row, correct to the current sort order
                int lastSelectedRowIndex = -1;
                bool foundLastSelectedRowIndex = false;

                // In testing, a for nested in a Parallel.For takes the least amount of processing time in all tests (including low capacity tests) versus all other combinations
                //var sw = System.Diagnostics.Stopwatch.StartNew();

                // Loop over all entries to match the selectedTrackId's to their new row indexes
                Parallel.For(0, entries.Count, (rowIndex, loopState_0) =>
                {
                    // Loop over all selectedTrackIds
                    for (int i = 0; i < selectedTrackIds.Length; i++)
                    {
                        // Check to see if the trackId of this row matches a trackId in the selected rows
                        if (entries[rowIndex].trackId == selectedTrackIds[i])
                        {
                            // ! Instead of adding, put the row index in the same spot as the selectedTrackIds array
                            rowIndexes[i] = rowIndex;
                            foundRowIndexesCount++;
                            //rowIndexes.Add(rowIndex);

                            // Also find the lastSelectedTrackId if it hasn't yet been found
                            // The first row in SelectedRows - and by extension selectedTrackIds will ALWAYS be the last one that was selected
                            // However, since this is a parallel for, we're not guaranteed to hit it first, hence why we save the lastSelectedTrackId, and check it for every iteration
                            if (!foundLastSelectedRowIndex)
                            {
                                if (entries[rowIndex].trackId == lastSelectedTrackId)
                                {
                                    lastSelectedRowIndex = rowIndex;
                                    foundLastSelectedRowIndex = true;
                                }
                            }

                            break;
                        }
                    }

                    // Stop iterating if we have translated all trackId's to rowIndexes
                    // Comment this to test timing
                    if (foundRowIndexesCount == selectedTrackIds.Length)
                        loopState_0.Stop();
                });

                //sw.Stop();
                //Debug.Log("Parallel.For + for took " + sw.ElapsedMilliseconds);

                // Clear the current selection (it is currently selecting rows at the same index as before the sort, therefore the selection is invalid)
                dgvEntries.ClearSelection();

                // Scroll to the first row in the selection
                // If we have a keepWithinViewRowIndex, scroll the DataGridView so that it is within view (plus the offset as saved earlier)
                if (lastSelectedRowIndex != -1)
                {
                    int scrollingRowIndex = lastSelectedRowIndex - (dgvEntries.DisplayedRowCount(false) / 2);

                    // Firstly, clamp it to within range
                    if (scrollingRowIndex < 0)
                        scrollingRowIndex = 0;
                    else if (scrollingRowIndex >= dgvEntries.Rows.Count)
                        scrollingRowIndex = dgvEntries.Rows.Count - 1;

                    // Finally, scroll the scrollingRowIndex to be the first row in view
                    dgvEntries.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
                }

                // Set the CurrentCell before selecting new rows, in order to allow multi-selection after sorting
                dgvEntries.CurrentCell = dgvEntries.Rows[lastSelectedRowIndex].Cells[savedSelectedColumn];

                // Select the new rows, the order should be maintained from the previous SelectedRows order
                for (int i = 0; i < rowIndexes.Length; i++)
                {
                    dgvEntries.Rows[rowIndexes[i]].Selected = true;
                }



                // Un-inhibit selection event
                shouldInhibitDataGridViewSelectionEvent = false;

                // Reset some variables
                selectedTrackIds = null;
                lastSelectedTrackId = -1;
            }

            shouldInhibitDataGridViewSelectionEvent = false;
        }

        // --- TreeView Events ---

        // This is essentially the selection event for a TreeView
        // When a node is selected, we select the rows pertaining to it in the DataGridView
        // Select based on different criteria depending on the level that is selected
        private void treeViewEntries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                // Inhibit selection events
                shouldInhibitDataGridViewSelectionEvent = true;

                dgvEntries.ClearSelection();

                // Select all tracks by this artist
                if (e.Node.Level == 0)
                {
                    foreach (var row in dgvEntries.Rows.Cast<DataGridViewRow>()
                        .Where(x => ((Entry)x.DataBoundItem).albumArtist == e.Node.Text))
                    {
                        row.Selected = true;
                    }
                }

                // Select all tracks in this album, of this albumArtist
                else if (e.Node.Level == 1)
                {
                    bool hasAlbumArtist = e.Node.Parent.Text != Consts.DEFAULT_ARTIST_STR && e.Node.Parent.Text != "Compilations";
                    string albumArtist = e.Node.Parent.Text;

                    foreach (var row in dgvEntries.Rows.Cast<DataGridViewRow>()
                        .Where(x => ((Entry)x.DataBoundItem).album == e.Node.Text &&
                                    (hasAlbumArtist ? ((Entry)x.DataBoundItem).albumArtist == albumArtist : true)))
                    {
                        row.Selected = true;
                    }
                }

                // Select the track, of the trackId
                else if (e.Node.Level == 2)
                {
                    dgvEntries.Rows.Cast<DataGridViewRow>()
                        .First(x => ((Entry)x.DataBoundItem).trackId.ToString() == e.Node.Name)
                        .Selected = true;
                }

                // Scroll to first row of selection
                if (dgvEntries.SelectedRows.Count > 0)
                {
                    // SelectedRows isn't always ordered - so we have to select the one with the lowest row index
                    int lowestIndex = dgvEntries.SelectedRows.Cast<DataGridViewRow>()
                                                             .Min(x => x.Index);

                    dgvEntries.FirstDisplayedScrollingRowIndex = lowestIndex;
                }

                // Un-inhibit selection events, but fire selection
                shouldInhibitDataGridViewSelectionEvent = false;
                dgvEntries_SelectionChanged(this, null);
            }
        }

        // Fired when focus enters the TreeView component
        // Since AfterSelect only fires when the TreeView is focused, we have to manually call it when focus re-enters the TreeView, and we have a selected node. That way, selections coming from out-of-focus still follow through.
        private void treeViewEntries_Enter(object sender, EventArgs e)
        {
            if (treeViewEntries.SelectedNode != null)
            {
                treeViewEntries_AfterSelect(this, new TreeViewEventArgs(treeViewEntries.SelectedNode, TreeViewAction.ByMouse));
            }
        }

        // --- Properties Events ---

        private void txtMappedFilePathValue_Leave(object sender, EventArgs e)
        {
            // Do nothing when there is no selection
            if (GetEntriesCount(EntryFilter.Selection) == 0)
                return;

            // Ignore when enter is pressed on != symbol
            if (txtMappedFilePathValue.Text == Consts.NOT_EQUAL_CHAR)
                return;

            // Check if the value actually changed
            if (txtMappedFilePathValue.Modified)
            {
                string newValue = txtMappedFilePathValue.Text;
                var selected = GetEntries(EntryFilter.Selection);

                // Allow clearing multiple mapped file paths, but do not allow setting multiple
                if (string.IsNullOrEmpty(newValue))
                {
                    selected.ForEach((x) => x.mappedFilePath = string.Empty);
                }
                else if (selected.Count == 1)
                {
                    // Ensure that the file path is valid, and that it actually points to a file
                    // We'll let the TagWriterWorker worry about if the file is actually valid or not
                    if (!File.Exists(newValue))
                    {
                        Debug.LogError("\"" + newValue + "\" isn't a path to an existing file!", true);
                        txtMappedFilePathValue.Undo();
                    }
                    // Write the change
                    else
                    {
                        selected[0].mappedFilePath = newValue;
                        dgvEntries.Refresh();
                        dgvEntries.Update();
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot write multiple mappedFilePaths! This value has to be unique");
                    txtMappedFilePathValue.Undo();
                }
            }
        }

        private void txtMappedFilePathValue_KeyDown(object sender, KeyEventArgs e)
        {
            // Leave focus when enter is pressed (calls the Leave event, therefore setting value)
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.ActiveControl = null;
            }

            // Cancel input when escape is pressed
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtMappedFilePathValue.Undo();
                this.ActiveControl = null;
            }
        }

        // Double click on Mapped path to pick file
        private void txtMappedFilePathValue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Don't allow if there are multiple or no entries selected
            if (dgvEntries.SelectedRows.Count != 1)
                return;

            string initialPath = (!string.IsNullOrWhiteSpace(txtMappedFilePathValue.Text) && txtMappedFilePathValue.Text != Consts.NOT_EQUAL_CHAR) ? Path.GetDirectoryName(txtMappedFilePathValue.Text) : sourceLibraryFolderPath;

            var fileOpenDialog = new OpenFileDialog();
            fileOpenDialog.Filter = "Audio Files|*.mp3;*.m4a;*.m4p;*.ogg;*.flac;*.alac;*.aiff;*.wav;*.wave;*.wma|All files|*.*;";

            if (Directory.Exists(initialPath))
                fileOpenDialog.InitialDirectory = initialPath;

            // If the user picked a file, make it the target of this entry
            if (fileOpenDialog.ShowDialog() == DialogResult.OK)
            {
                txtMappedFilePathValue.Focus();
                txtMappedFilePathValue.Text = fileOpenDialog.FileName;
                txtMappedFilePathValue.Modified = true;
                this.ActiveControl = null;
            }
        }

        bool dtpPropertyDirty = false;

        // Stop showing != when focus enters dtpDateAddedValue or dtpLastPlayedValue, as to allow the user to set a value
        private void dtpProperty_Enter(object sender, EventArgs e)
        {
            dtpPropertyDirty = false;
            var dtp = (DateTimePickerAlt)sender;

            // If there are multiple representative values being shown, show the first DateTime available
            if (dtp.CustomFormat == Consts.NOT_EQUAL_CHAR && false)
            {
                dtp.CustomFormat = Consts.DATE_TIME_FORMAT;

                DateTime dateTimeValue = DateTime.Now;

                if (object.ReferenceEquals(dtp, dtpDateAddedValue))
                    dateTimeValue = GetEntries(EntryFilter.Selection)[0].dateAdded;
                else if (object.ReferenceEquals(dtp, dtpLastPlayedValue))
                    dateTimeValue = GetEntries(EntryFilter.Selection)[0].lastPlayed;

                // If the dateTimeValue is out of range, set it to now
                if (dateTimeValue < dtp.MinDate || dateTimeValue > dtp.MaxDate)
                    dateTimeValue = DateTime.Now;

                dtp.Value = dateTimeValue;
            }

            // If there is no representative value being shown, show the current DateTime
            else if (dtp.CustomFormat == " ")
            {
                dtp.CustomFormat = Consts.DATE_TIME_FORMAT;
                dtp.Value = DateTime.Now;
            }
        }

        // Called when the user pressed a key while either dtpDateAddedValue or dtpLastPlayedValue is focused
        private void dtpProperty_KeyDown(object sender, KeyEventArgs e)
        {
            // Leave focus when enter/escape is pressed (causing the Leave event to fire, therefore setting value)
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                // If escape is pressed. reset the value, setting the dirty flag to false
                if (e.KeyCode == Keys.Escape)
                {
                    UpdateProperties();
                    Debug.LogWarning("Cancelled");
                    dtpPropertyDirty = false;
                }

                // Force leave focus
                this.ActiveControl = null;

                //dgvEntries.Refresh();
                //dgvEntries.Update();
            }

            // Uncheck the DateTimePicker if delete is pressed
            else if (e.KeyCode == Keys.Delete)
            {
                var dtp = (DateTimePickerAlt)sender;
                dtp.Checked = false;
            }
        }

        // When the user double clicks the field, allow them to edit when the field has a not equals sign
        private void dtpProperty_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var dtp = (DateTimePickerAlt)sender;

            if (dtp.CustomFormat == Consts.NOT_EQUAL_CHAR)
            {
                dtpPropertyDirty = true;

                dtp.CustomFormat = Consts.DATE_TIME_FORMAT;
                dtp.Checked = true;

                DateTime dateTimeValue = DateTime.Now;

                if (object.ReferenceEquals(dtp, dtpDateAddedValue))
                    dateTimeValue = GetEntries(EntryFilter.Selection)[0].dateAdded;
                else if (object.ReferenceEquals(dtp, dtpLastPlayedValue))
                    dateTimeValue = GetEntries(EntryFilter.Selection)[0].lastPlayed;

                // If the dateTimeValue is out of range, set it to now
                if (dateTimeValue < dtp.MinDate || dateTimeValue > dtp.MaxDate)
                    dateTimeValue = DateTime.Now;

                dtp.Value = dateTimeValue;
            }
        }

        // Called when the value of either dtpDateAddedValue or dtpLastPlayedValue changes, or if the checkbox state changes
        private void dtpProperty_ValueChangedSpecial(object sender, EventArgs e)
        {
            var dtp = (DateTimePickerAlt)sender;

            // Immediately set dateAddedDisabled/lastPlayedDisabled if the check was from unfocused state
            if (dtp.Checked != dtp.CheckedLast)
            {
                //dtpPropertyDirty = true;

                var selected = GetEntries(EntryFilter.Selection);

                if (object.ReferenceEquals(dtp, dtpDateAddedValue))
                {
                    selected.ForEach(x => x.dateAddedDisabled = !dtp.Checked);
                    dgvEntries.InvalidateColumn(colDateAdded.Index);
                }
                else if (object.ReferenceEquals(dtp, dtpLastPlayedValue))
                {
                    selected.ForEach(x => x.lastPlayedDisabled = !dtp.Checked);
                    dgvEntries.InvalidateColumn(colLastPlayed.Index);
                }

                dgvEntries.Update();
                //dtpProperty_Leave(sender, e);
            }

            // Set dtpDateAddedValueDirty when the value changed, while focused
            else if (dtp.Focused)
            {
                dtpPropertyDirty = true;
            }
        }

        // Event called when focus leaves either the dtpDateAddedValue or dtpLastPlayedValue DateTimePickers
        private void dtpProperty_Leave(object sender, EventArgs e)
        {
            // Do nothing when there is no selection
            if (GetEntriesCount(EntryFilter.Selection) == 0)
                return;

            // If the property in question didn't change, do nothing
            if (dtpPropertyDirty == false)
                return;

            var dtp = (DateTimePickerAlt)sender;
            var selected = GetEntries(EntryFilter.Selection);

            // Set dateAddedDisabled/lastPlayedDisabled to true if the DateTimePicker was unchecked
            if (dtp.Checked == false || dtp.Value == dtp.MinDate)
            {
                if (object.ReferenceEquals(dtp, dtpDateAddedValue))
                    selected.ForEach(x => x.dateAddedDisabled = true);
                else if (object.ReferenceEquals(dtp, dtpLastPlayedValue))
                    selected.ForEach(x => x.lastPlayedDisabled = true);
            }

            // Set the new value, invalidating the column so that cells are updated
            // Note that setting dateAdded or lastPlayed will enable the field (setting dateAddedDisabled/lastPlayedDisabled to false)
            else
            {
                DateTime newValue = dtp.Value;

                if (object.ReferenceEquals(dtp, dtpDateAddedValue))
                {
                    selected.ForEach((x) => { x.dateAdded = newValue; });
                    dgvEntries.InvalidateColumn(colDateAdded.Index);
                }
                else if (object.ReferenceEquals(dtp, dtpLastPlayedValue))
                {
                    selected.ForEach((x) => { x.lastPlayed = newValue; });
                    dgvEntries.InvalidateColumn(colLastPlayed.Index);
                }

                dtpPropertyDirty = false;
                //dgvEntries.Refresh();
                dgvEntries.Update();
            }
        }

        bool firstEntered = false;

        private void txtPlayCountValue_MouseDown(object sender, MouseEventArgs e)
        {
            if (!firstEntered)
            {
                txtPlayCountValue.SelectAll();
                firstEntered = true;
            }
        }

        private void txtPlayCountValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void txtPlayCountValue_KeyDown(object sender, KeyEventArgs e)
        {
            // Leave focus when enter is pressed (calls the Leave event, therefore setting value)
            // Cancel input when escape is pressed
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (e.KeyCode == Keys.Escape)
                {
                    UpdateProperties();
                    txtPlayCountValue.Refresh();
                }

                this.ActiveControl = null;
            }
        }

        private void txtPlayCountValue_Leave(object sender, EventArgs e)
        {
            firstEntered = false;

            // Do nothing when there is no selection
            if (GetEntriesCount(EntryFilter.Selection) == 0)
                return;
            
            // Check if the value actually changed
            if (txtPlayCountValue.Modified)
            {
                int newValue = Convert.ToInt32(txtPlayCountValue.Text);
                var selected = GetEntries(EntryFilter.Selection);

                selected.ForEach(x => x.playCount = newValue);
                dgvEntries.InvalidateColumn(colPlayCount.Index);
                dgvEntries.Update();
            }
        }

        // Save the rating to the selection if the user changes it
        private void cbRatingValue_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbRatingValue.Focused)
            {
                var selected = GetEntries(EntryFilter.Selection);
                selected.ForEach(x => x.rating = (Rating)cbRatingValue.SelectedValue);
                dgvEntries.InvalidateColumn(colRating.Index);
                dgvEntries.Update();
            }
        }

        // --- Misc. Control Events ---

        // Event fires when the user right clicks the DataGridView header
        // Shows a context menu used to enable/disable viewing of columns
        private void columnHeaderContextMenu_Opening(object sender, CancelEventArgs e)
        {
            int i = 0;
            columnHeaderContextMenu.Items.Clear();

            // Generate and/or update entries entries
            foreach (DataGridViewColumn col in dgvEntries.Columns)
            {
                string value = !string.IsNullOrEmpty(col.HeaderText) && col.HeaderText.Length > 1 ? col.HeaderText : (string)col.Tag;
                columnHeaderContextMenu.Items.Add(value);
                var item = (ToolStripMenuItem)columnHeaderContextMenu.Items[i];
                item.Checked = col.Visible;
                item.CheckOnClick = true;
                item.CheckedChanged += (_sender, _e) =>
                {
                    if (item.Checked)   col.Visible = true;
                                else    col.Visible = false;
                };

                // Don't allow hiding the ID column (we need that column to maintain selection when sorting)
                if (item.Text == "ID")
                    item.Enabled = false;

                i++;
            }

            // Set the cancel property to false to force the context menu to open on the first time
            e.Cancel = false;
        }

        bool isScrollbarShown = false;
        bool doingAdjustment = false;

        // Resize event for the tableLayoutPanel, note that it doesn't resize to fit it's contents automatically
        // Manually control sizing the tableLayoutPanel to fit scrollbar, since Microsoft is incapable
        private void tableLayoutPanel_Resize(object sender, EventArgs e)
        {
            if (doingAdjustment) return;

            tableLayoutPanel.SuspendLayout();
            doingAdjustment = true;

            int width = tableLayoutPanel.Width;

            // Increase the width of the table to fit the scrollbar
            if (tableLayoutPanel.VerticalScroll.Visible && !isScrollbarShown)
            {
                // Set new minimum size of splitContainerVertical.Panel1 to fit scrollbar
                splitContainerVertical.Panel1MinSize = 255 + 16;
                splitContainerVertical.SplitterDistance = width + 16;

                tableLayoutPanel.Width = width + 16;
                isScrollbarShown = true;
            }

            // Decrease the width of the table to make up for the loss of the scrollbar
            else if (!tableLayoutPanel.VerticalScroll.Visible && isScrollbarShown)
            {
                splitContainerVertical.Panel1MinSize = 255;
                splitContainerVertical.SplitterDistance = width - 16;

                tableLayoutPanel.Width = width - 16;
                isScrollbarShown = false;
            }

            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            doingAdjustment = false;
        }

        int lastPanelWidth = 255;

        // Expand the tableLayoutPanel to fill width when the splitContainerHorizontal is resized
        private void splitContainerHorizontal_Resize(object sender, EventArgs e)
        {
            if (splitContainerHorizontal.Panel1.Width != lastPanelWidth)
            {
                tableLayoutPanel.Size = new Size(splitContainerHorizontal.Panel1.Width, tableLayoutPanel.Height);
                lastPanelWidth = splitContainerHorizontal.Panel1.Width;
            }
        }


        #endregion
    }
}
