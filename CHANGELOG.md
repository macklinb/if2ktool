
# Changelog
All notable changes to this project will be documented in this file.

## v1.0.7026.21004 (28/03/2019)

### Added:
* Added better support for WAV files by writing other data (title, artist, etc) to them, as iTunes does not. This is disabled by default, but can be enabled in Preferences -> Tagging. See the readme (under "Which formats are supported?") for the reasoning. To facilitate this:
	* Expanded fields collected from XML to include Genre, Kind, Track Count, Disc Number, Disc Count, Comments. These fields do not appear in the DataGridView, as they aren't considered as identifying fields.
	* Added functions in TagWriterWorker to write and remove native frames (i.e. text information frames on ID3).
    * Expanded masstagger export option to allow export of file info (not limited to WAV).
	* Option to force ID3v2 frame encoding type. This will only apply to new frames (not to existing frames, and not to playback statistics frames). Bulk changing the encoding on existing frames is not a good idea, so it is not supported.
	* Tags will be written in ID3v2 only.
* When writing tags, the tags created "as a convenience" by TagLib will be removed before doing anything, leaving the file as close to the original as possible. Instead of always writing to the tag types that are created, the data will be written to tag types that may already be present on the disk. If a compatible tag type isn't present on the disk, one will be created depending on the input file type. If multiple tag types are present on the disk, one will be chosen as preference.
* TagWriterWorker has been switched over to a parallel implementation. This slightly improves tag writing speed at the cost of disk IO (though this cost is situational).
	* Added a preference to control the amount of available threads, which can be set to one to essentially disable parallel processing - there is still slight overhead however.
* Added "Dry run" setting to tag writer, which will preview output without writing any information to the file.
* Added search box to main form. Search is within context of the selected column. Cycle results with Enter, reverse cycle with Shift-Enter. Tested on a library of 25,000 and it seems fast enough.
* Option to ignore other missing files in a directory when mapping, rather than just all.
* Option to ignore all, or ignore other files in album when mapping in lookup mode, rather than just the single track.
* Option to not prompt to confirm fuzzily-matched entries while mapping.

### Changed

* No longer substitute "Unknown Artist" and "Unknown Album" for empty artist and album fields (except for TreeView).
* Track number and disc number no longer use padded zeroes in any data fields (they may still be displayed with padded zeroes).
* Playback statistics tags are now always written in ISO 8859-1.
* Default workerDelayStart (delay before mapping and writing tags) is now false.
* Changed double-click action of mapped path text box in properties pane to open in explorer, adding a dedicated button to pick a new path.
* TagWriterWorker TagLib interface code moved to TagWriterUtility class.

### Removed

* Removed "Don't add ID3v1 tag" setting.

### Fixes
* Sorting the DataGridView now maintains the selected sort column, instead of incorrectly always using the first column.
* Fixed ID3v2Version combobox not showing ID3v2_4.
* Fixed properties window not updating after adding new column or changing column width.
* Tooltips now wrap a bit more cleanly in preferences (though it still is not perfect).
* Export playlists form no longer disables the export button when multiple playlists are selected.
* Exporting playlists will ensure the file name is valid before saving (i.e. it contains no '\', ':', etc).
* Fixed "Scroll with selection" preference causing functions that set row selection to not work when it is set to false. This now only affects this behaviour during processing.
