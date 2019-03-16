# if2ktool - A tool used to migrate some stats (play count, rating, date added and last played) from iTunes to foobar2000's foo_playcount component (Playback Statistics).

- [Overview](#overview)
    - [Requirements](#requirements)
    - [Third party software/libraries used](#third-party-software/libraries-used)
    - [Quick start](#quick-start)
- [Mapping to files](#mapping-to-files)
	- [Modes: Direct](#modes-direct)
	- [Modes: Library Mapping](#modes-library-mapping)
	- [Modes: Lookup](#modes-lookup)
	- [Modes: Search](#modes-search)
- [Writing the data](#writing-the-data)
	- [Method 1 - Writing tags directly](#method-1---writing-tags-directly)
	- [Method 2 - Exporting tags for use with Masstagger](#method-2---exporting-tags-for-use-with-masstagger)
		- [Method 2.1 - Sort order matching (not recommended)](#method-21---sort-order-matching-not-recommended)
		- [Method 2.2 - Cross reference with foobar2000 data (recommended)](#method-22---cross-reference-with-foobar2000-data-recommended)
- [Transferring playlists from iTunes](#transferring-playlists-from-itunes)
- [foo_texttool JSON patterns](#foo_texttool-json-patterns)
- [Preferences](#preferences)
- [Troubleshooting + other stuff](#troubleshooting--other-stuff)

## Overview

This tool is aimed at people migrating a music library from iTunes to foobar2000, and allows you to keep the various statistics and dates that are saved by iTunes. It reads these fields from your iTunes Library XML, and writes them to the file tags that are expected by the import function of foo_playcount. You can transfer playlists too.

Note that it only works in one direction! You cannot import a collection of files and have them be matched with the XML.

<b>NOTICE:</b>
This tool is very much experimental, and is still in development. By using it, you accept this fact, and you take full responsibility for whatever may happen to your library, including corruption of files, loss of data and general unpleasantness. Please make sure to back up your files before writing any data!

### Requirements
* .NET Framework Version 4.7.2 runtime - [download](https://dotnet.microsoft.com/download)
* The above requires Windows 7 SP1 or higher.

### Third party software/libraries used
* <b>TagLib# (TagLib-Sharp)</b> - Tag reading and writing<br/>
Copyright © 2019 Mono Project<br/>
Link: https://github.com/mono/taglib-sharp<br/>
Licence: https://github.com/mono/taglib-sharp/blob/master/COPYING
* <b>Newtonsoft Json .NET</b> - Deserializing JSON data<br/>
Copyright © 2019 Newtonsoft + James Newton-King<br/>
Link: https://www.newtonsoft.com/json<br/>
Licence: https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md
* <b>Mono DataConvert</b> - A better [BitConverter](https://docs.microsoft.com/en-us/dotnet/api/system.bitconverte), supporting conversion with specific endianness'<br/>
Copyright © 2019 Mono Project + Miguel de Icaza<br/>
Link: https://www.mono-project.com/archived/mono_dataconvert/<br/>
Licence: https://github.com/mono/mono/blob/master/LICENSE

Thanks
* To user cvzi on github for [itunes_smartplaylist](https://github.com/cvzi/itunes_smartplaylist), for information regarding the structure and parsing of iTunes Smart Playlists (this feature is not yet complete).

### Quick start
1. Import an iTunes Library XML (File -> Open library XML).
2. [Map](#Mapping-to-files) the XML to files (Tools -> Map entries to files).
3. [Write](#Writing-the-data) the statistics to files either:
    * By directly writing the tags in if2ktool (Tools -> Write statistics tags to files).
    * Or in foobar2000 with a Masstagger script (File -> Export masstagger mts). This requires you to map files in "Lookup" mode, instead of Direct or Mapped mode.
4. Import statistics, by right clicking the tracks in foobar -> Playback Statistics -> Import statistics from file tags.
5. Remove the tags, either by using Masstagger or with Tools -> Remove statistics tags from files.

#
## Mapping to files
The first step in the process (after importing an iTunes Library XML) is to match every entry in the XML to a file on disk. This process is called "mapping". When a track/entry is mapped, it means the tool has found a file path for that particular track. Before any playback statistics data is written, an entry needs to be mapped to a file first.

There are a couple of different modes for this. For most, it simply involves looking at the `Location` key in the XML, and writing the tags to the file that exists there. However, if you need to match an XML pulled from another machine, you can use a number of other methods to accomplish the same thing.

Mapping is done using the menu item "Tools -> Map entries to files". Depending on the mode selected, and the number of entries to process, this process may take a few seconds or several minutes.

### Modes: Direct
For tagging an iTunes library in-place. Generally most people should use this.
It assumes that the xml refers to an existing library that is accessible on the machine (or on the network), and that the files referenced by iTunes are the same exact ones to be loaded by foobar2000. Note that matching is only based on the file path, so if the path varies slightly, the file will not be matched (there are a few options that are used to mitigate this possibility).

### Modes: Library Mapping
Used to map the paths in the XML to a different library directory. This substitutes the "Music Folder" portion of the track path in the XML with a path of your choosing. This assumes that the structure of the original library folder is the same as the one provided. If you have copied your library from another computer, or to another directory, but have otherwise not changed the file structure - use this mode.

If you haven't checked "Keep iTunes Media folder organized" in iTunes, this process is very likely to have issues. The closer your file structure is to the original, the better off you'll fare.

### Modes: Lookup
Instead of mapping entries based on file path alone, if you have already imported your library into foobar2000, you can use that as a reference for mapping the tracks in the  XML to their associated files. In lookup mode, it is required that you generate a JSON file containing tags for all tracks that if2ktool can use to reliably match the data from the XML to. The tags used for matching are; `track title`, `artist`, `album`, `track number`, and `file name` - or a subset of those that are present in both the XML and JSON entry. See [below](#generating-a-lookup-json) for how to generate the data required.

Note that this requires tags to remain consistent between iTunes and foobar2000.

On every iTunes Library XML entry, we collect a list of candidate foobar2000/JSON entry matches, scoring each based on the percentage of tags (present in both entries) are equal, then choosing the candidate with the highest score. Candidates are entries with scores >= 50% (meaning 50% of the *present* tags were matched). If there are multiple candidate matches with the same score, you are prompted to resolve it yourself. 

This mode is very thorough, so it takes much longer than the other modes. It also ignores the anyExtension and fuzzy options for the file path - since we can assume that the JSON points to valid files. We do not perform fuzzy comparison on tags either, as this can introduce too many options with larger libraries.

Loading a lookup JSON will also let if2ktool know the sort order of the tracks in foobar, allowing you to export data that takes advantage of this. This opens up the possibility of writing tags using a Masstagger script (see [method 2](#Method-2---Exporting-tags-for-use-with-Masstagger)), which may be more reliable under certain circumstances.

<!--, or with a direct foo_playcount xml export (see [method 3](#Method-3---Export0a-playback-statistics-XML)). These methods may be more reliable under certain circumstances.-->

#### Generating a lookup JSON

<u>Automatically:</u>

if2ktool provides a method of automating the lookup creation process using an [AutoHotkey](https://www.autohotkey.com/) script. It can be launched using "Tools -> Generate JSON lookup", however may not work for everyone. Once created, continue with step 5 and 6 below.

<u>Manually:</u>

Use the following steps to generate track data in the form of a JSON file. You can then use it to map the XML to the associated files.

1. Make sure you have [foo_texttools](http://www.foobar2000.org/components/view/foo_texttools) installed.
2. Sort the playlist view in foobar2000 in a somewhat uniform order. It doesn't matter how they're sorted, just as long as it's sorted the same way later, when you import the mts file (assuming you take that route). Consider using the Album List (Library -> Album List) to ensure uniformity.
3. Select all tracks (if in the playlist view). From the context menu, click "Utilities" -> "Text Tools" -> "Advanced...". This will bring up a window which will generate a text output of metadata for the selected files, depending on the pattern used.
4. Input the [patterns](#foo_texttool-JSON-patterns) below to generate a JSON-formatted list of tracks, then copy the text that is generated into a new plain text file.<br/><b>NOTE:</b> Ensure that you aren't using foo_unpack, and that there are no archived tracks in your library before doing this.
5. In if2ktool (after opening an iTunes Library XML), click "Map XML to files..." under the tools menu. Set the mode to "Lookup", and browse to the location of the JSON file you exported in the previous step.
6. Click "Start". This wil take a while to match, since it's a fairly rigerous process.


<!--
### Modes: XML Key Mapping - *Work in progress*

This is similar to the way foo_fileops works, where you provide an absolute path pattern consisting of possible tags in the file, and the look for a file path based on that pattern. However, we can only use the fields available to us in the XML. Can be used in combination with either direct or library mapping to substitute the library folder portion of the path.

An end-point file name must be provided in the pattern, unless used with the "Search" mode.
If the drive portion of the pattern is omitted, we substitute the direct or library mapping folder.
	
You can use the following variables. They are named to match closely to [title formatting](https://wiki.hydrogenaud.io/index.php?title=Foobar2000:Title_Formatting_Reference). Fields that are unlikely to be used are omitted.

Variable | XML Key | Notes
--- | --- | --- 
title / tracktitle | Name | If "Name" is missing, file name is used instead
artist | Artist | Checks for values in this order "Artist", "Album Artist", "Composer", "Performer".
album artist | Album Artist | Checks for values in this order "Album Artist", "Artist", "Composer", "Performer".
album | Album
date / year | Year
genre | Genre
tracknumber | Track Number | Always padded to at least two digits.
totaltracks | Track Count | This value is calculated depending on the amount of other entries that exist
discnumber | Disc Number
disccount | Disc Count
codec | Kind | The Kind field with the "purchased/audio file" portion removed. MPEG -> MP3, Apple Lossless -> ALAC
filename | - | 	Name of file, excluding directory path and extension.
filename_ext | - | 	Name of the file, including extension but excluding directory path.
ext | - | File extension, not including '.'

Take the following entry for example:
```xml
<dict>
    <key>Track ID</key><integer>43295</integer>
    <key>Name</key><string>One More Time</string>
    <key>Artist</key><string>Daft Punk, Romanthony</string>
    <key>Album Artist</key><string>Daft Punk</string>
    <key>Composer</key><string>A. Moone/A. Moore/Guy-Manuel de Homem-Christo/Thomas Bangalter</string>
    <key>Album</key><string>Discovery</string>
    <key>Genre</key><string>Electronic</string>
    <key>Kind</key><string>MPEG audio file</string>
    <key>Size</key><integer>12976128</integer>
    <key>Total Time</key><integer>322742</integer>
    <key>Track Number</key><integer>1</integer>
    <key>Year</key><integer>2001</integer>
    <key>Date Modified</key><date>2016-02-08T13:50:43Z</date>
    <key>Date Added</key><date>2016-02-08T13:50:51Z</date>
    <key>Bit Rate</key><integer>320</integer>
    <key>Sample Rate</key><integer>44100</integer>
    <key>Skip Count</key><integer>2</integer>
    <key>Skip Date</key><date>2016-02-10T13:11:34Z</date>
    <key>Play Count</key><integer>8</integer>
    <key>Play Date</key><integer>3540073373</integer>
    <key>Play Date UTC</key><date>2016-03-05T17:42:53Z</date>
    <key>Album Rating</key><integer>60</integer>
    <key>Album Rating Computed</key><true/>
    <key>Artwork Count</key><integer>1</integer>
    <key>Persistent ID</key><string>FBB242C4712AF2FB</string>
    <key>Track Type</key><string>File</string>
    <key>Location</key>`<string>file://localhost/Users/Macklin/Music/iTunes/iTunes%20Media/Music/Daft%20Punk/Discovery/01%20One%20More%20Time.mp3</string>
    <key>File Folder Count</key><integer>5</integer>
    <key>Library Folder Count</key><integer>1</integer>
</dict>
```

Using the command

	if2ktool "library.xml" -xmlkeymapping "C:/Library/Music/%albumartist% - %album% (%year%)/%tracknumber% - %tracktitle%.%ext"

This would end up mapping to the path `C:/Library/Music/Daft Punk - Discovery (2001)/1 - One More Time.mp3`

-->
<!--
### Modes: Search
Performs a search for each and every file in the library folder provided, matching it not based on file path and name - but instead the how the tags contained within the files match with the tags present in the XML. The tags used are track title, artist, album and track number - or a subset of those that are present in the XML entry. The file must match every checked tag in order to be mapped. This mode takes the longest amount of time, and uses the most amount of memory.

This has to be used with one of the above modes to provide a search directory, or restrict to a particular directory. You can use either:

* `-searchall` to recursively look through all directories below the library folder, front-to-back. The most extensive search, and will waste a lot of time.
* `-searchendpoint` to look at the files in the endpoint directory ONLY - assuming that it exists.
* `-searchbacktofront` to look for files at the endpoint directory, stepping up a directory if no matches were found, or if the directory doesn't exist. This stops when we get to the library directory.

#### Examples
Lets assume the "Music Folder" key is `file://localhost/Users/Macklin/Music/iTunes/iTunes%20Media/` (we add "Music" to the end of this path)

and the location key for one of our tracks is `file://localhost/Users/Macklin/Music/iTunes/iTunes%20Media/Music/Daft%20Punk/Discovery/01%20One%20More%20Time.mp3`.

You can also use library mapping to substitute the music folder key with one of your choosing. With the command `if2ktool library.xml D:/Music` the track path becomes `D:/Music/Daft Punk/Discovery/01 One More Time.mp3`.

If you use tag mapping, and do not provide an endpoint
* `-searchall`<br/>
Takes the "Music Folder" key from the xml, using that as the library folder. We then build an index of all the files in "iTunes Media/Music", and then query every single file one-by-one for every entry.

* `-searchendpoint`<br/>
Searches the endpoint of the entry. Taking the above example, looks only in the folder "Discovery" for files with tags that match the ones in the XML entry.

* `-searchfronttoback`<br/>
Searches the endpoint of the entry. Taking the above example, looks in the folder "Discovery" for files with tags that match the ones in the XML entry. If the file is not found, we then move up to the "Daft Punk" folder, and so on until the "iTunes Media" folder is reached.
-->

#
## Writing the data
After mapping the iTunes Library XML to the files, the next step is to actually write the tags so that they can be read by foo_playcount.

The following table represents the tags that are written to the files and read by foo_playcount:

Tag | Plist key | Description
--- | --- | --- | ---
added_timestamp | Date&nbsp;Added | Integer - [LDAP](https://docs.microsoft.com/en-us/windows/desktop/sysinfo/file-times) timestamp (also called Windows NT time format, [FILETIME](https://docs.microsoft.com/en-us/windows/desktop/api/minwinbase/ns-minwinbase-filetime), SYSTEMTIME, NTFS file time)
last_played_timestamp | Play&nbsp;Date | Integer - Same as above
play_count | Play&nbsp;Count | Integer - Number of plays
rating | Rating | Popularimeter (POPM) tag in a range of 0-255 (byte):<br/>1 &nbsp;&nbsp;&nbsp;&nbsp;= ★<br/>64 &nbsp;&nbsp;= ★★<br/>128 = ★★★<br/>196 = ★★★★<br/>255 = ★★★★★

### Method 1 - Writing tags directly
This writes tags directly to the file using the excellent [TagLib-Sharp](https://github.com/mono/taglib-sharp) library. Writing tags with this method is much faster than doing so with Masstagger.

Note that the time that it takes to write tags depends on the size, tag complexity and file format of your library. Larger files take longer to process, and AAC files take significantly longer to write than MP3s. On average this method takes about a minute to process 1000 files on a standard HDD, and likely faster on an SSD.

A parallel version of the `TagWriterWorker` class speeds up this process by about 4 times, however it has not yet been implemented. If you want to test it out, build the executable from source - simply rename `TagWriterWorker_ParallelTest.cs.bkup` to `TagWriterWorker.cs` beforehand.

You can write tags with "Tools -> Write statistics tags to file".

#### Which formats are supported?
It supports most of the same formats that iTunes does, and a few more:
* Writing ID3v2 TXXX frames/tags to MP3/AAC/ALAC/AIFF files
* Writing MP4 "Apple annotation box"s (MPEG-4 Part 14) to ALAC files
* Writing Vorbis comments for FLAC files.

Unsupported file formats:
* WAV files are currently not supported, due to TagLib's current inability to write ID3v2 tags in these files.
* AIFF files that do not use an ID3v2 chunk are not supported. For these files you will have to manually create an ID3v2 chunk with a tool like [kid3](https://kid3.sourceforge.io/).

### Method 2 - Exporting tags for use with Masstagger

For unsupported formats, and stubborn files (or just as another option) you have the option to export the data to a masstagger script so that playback statistics tags may be written in foobar2000, instead of externally. This assumes that you have already moved your library from iTunes to foobar2000

This method can in some cases be more reliable with writing tags, as the fields, encoding, format and container of the tags written will be the same as what is expected by foobar, and therefore is guaranteed to be read. However, writing a large amount of tags will take much, much longer in masstagger than it does with if2ktool.

This uses Masstagger's "Input data" action, to write tags to tracks, where it is one line of data per track in the current context of foobar2000. Because of this, if2ktool needs to know the sort order of tracks in foobar so that it matches the exported script. This can be accomplished with two different methods.

#### Method 2.1 - Sort order matching (not recommended)
*This method is no longer recommended for matching to many files at once, please use method 2.2 instead!*

This method does not require that you map the XML to files, and is only recommended if you need to write tags for a small amount of tracks. It is NOT recommended for large or inconsistently tagged libraries.

1. In if2ktool_gui (after opening an iTunes Library XML), go "Tools" -> "Generate mts..." and select the files that you want to include. You can choose how to order the export, but by default the file generated will be sorted alphabetically by track name.
2. In foobar2000, select the target tracks in the playlist view. Make sure that the view is currently sorted in descending order, and by the same value that the exported script is sorted by.
3. From the playlist view context menu, click "Tagging" -> "Manage scripts" -> "Import from file...", browsing to the file generated earlier. After writing tags, you can import the statistics as normal using the foo_playcount context menu option.

While you no longer have to worry about mapping XML entries to file paths with this method, masstagger's input data doesn't seem to support conditional operators, so matching the data to the track is entirely based on the assumption that order of the export and the order of the playlist view are the same. This is a major concern, since if even one track is out of line or misplaced, it could cause every track after it to be written with incorrect data. I've tested this a lot, and have confirmed that the sorting method used by foobar2000 is the exact same as what is generated in if2ktool (unless you use a custom sorting method for the track title) - so it is up to you to ensure that there are no stray or misnamed files.

Additionally, there can be issues with WAVE files when using the Track title sort order. iTunes doesn't write the tags to file and instead stores tags in the XML). Since foobar2000 doesn't have this data, it will show the file name as the title field, which will muddle up the sort order.

To combat this issue, you can export the lines of data in the masstagger script in different orders. When applying the script, ensure the playlist is sorted in the same order, via a column using the associated f2k field below.

Sort&nbsp;order | f2k Field | Notes
--- | --- | ---
Track&nbsp;title | %title% | f2k's title is determined by the tag, or the file name if the track name tag is not present.
Path | %path% | Will only work if the iTunes Library XML references files at the exact same path as in foobar. The path in the XML will be formatted to match the standard Windows-form path. Note that if the XML is from a Mac, the path will begin with '/' and will not contain a drive portion - making it unsuitable for sorting.
File&nbsp;name | %filename_ext% | Probably the most reliable, assuming the file names are the same as the ones in the XML, and that you don't have duplicates which may mess up the sort order.
File size (bytes) | %filesize% | The file size will only be valid prior to additional tagging - thankfully it doesn't change the sort order while writing tags.

#### Method 2.2 - Cross reference with foobar2000 data (recommended)
By using foo_texttools to get a list of track data from foobar2000, if2ktool is able to sort the exported mts in a way that conforms to a track listing in foobar2000. This will eliminate the possibility of having offset rows and mismatched data (as with the above method). As a side effect, we also retrieve the file paths directly from foobar2000.

In the case that an XML entry can't be matched to a track in foobar2000, we use an empty line to represents its position in the exported Masstagger script.

<b>Steps:</b>

1. Firstly, create a lookup JSON as per the steps [above](#Generating-a-lookup-JSON), and map your XML to it.
2. Click "Export masstagger mts..." under the File menu. Set the sort order to "Lookup sort order", and then click Export.
3. In foobar2000, firstly ensure that the sort order is the same as when you exported the data using foo_texttools - otherwise you'll end up mapping to incorrect files. Select all tracks and then from the playlist view context menu, click "Tagging" -> "Manage scripts" -> "Import from file...", browsing to the mts file that was exported. After writing tags, you can import the statistics as normal using the foo_playcount context menu option.

#### Troubleshooting sort order
In the export window, you can check "Debug sort order". This will write the track title (instead of any iTunes data) to the mts file, and can be used to test that the sort order of the exported file matches up with foobar2000. In the masstagger window, scroll to the bottom and check that the titles match up. If they do, the data has not been offset and should work fine after exporting the full script.

#
## Transferring playlists from iTunes
If2ktool provides a simple way of copying playlists from iTunes, by creating an [.m3u](https://en.wikipedia.org/wiki/M3U) or .m3u8 from the XML for import in foobar2000.

If your library is in the exact same place as is listed in the XML, you should just be able to export an .m3u directly from iTunes, and import it straight into foobar2000 with no hassles. Otherwise if your folder structure has changed, or you just want to import them all in one go, you can use this tool.

### Steps
1. Firstly, import an iTunes Library XML and map it to the files using any one of the above methods. This is required to ensure existence of the file paths in foobar2000.
2. Click File -> Export playlist(s). This will bring up a window showing all the playlists that were imported from the XML.
3. Select the playlists you want to export (using Control / Shift to select multiple) and then click the "Export" button at the bottom, saving them to wherever. You can optionally check "Extended M3U" to add track information, though this is ignored in foobar, and will have no effect.
4. In foobar2000, either drag the created file(s) onto the window, or click File -> Load playlist... to import the playlist.

### Notes
* You currently cannot export an iTunes Smart playlist to a foobar2000 Autoplaylist (though this is a feature I'd like to add at some point). You can however still choose to export a smart playlists' results as a regular playlist.

* Some playlists are automatically skipped, including "Music", "Podcasts", and "Audiobooks".

* During the playlist creation process, if the path has any non-ASCII, special characters - the file needs to be saved as an .m3u8 and NOT an .m3u so that foobar2000 knows how to properly interpret these characters. Otherwise these paths will not be found, and the playlist will have gaps. Check either the foobar console for errors, or the number of tracks in the playlist view against the number that is listed in the export window of if2ktool.

#
## foo_texttool JSON patterns

These patterns should be used in the "Text Tools" window, of foobar2000, and will create a JSON file with some tags pertaining to each track that if2ktool use to match entries to. You can also find them under "Tools -> Generate JSON lookup".

These patterns will only escape double-quote, tab and backslash characters. I doubt many people have other control characters in their tags. If this is the case, you can add yet another `$replace` surrounding each value.

#### Formatted / pretty
* Track pattern:<br/>
```
$pad( ,4){$char(13)$char(10)$pad( ,8)"index": $sub(%list_index%,1),$char(13)$char(10)$pad( ,8)"title": "$replace($replace($replace([$meta(Title)],\,\\),",\"),$tab(),\t)",$char(13)$char(10)$pad( ,8)"artist": "$replace($replace($replace([%artist%],\,\\),",\"),$tab(),\t)",$char(13)$char(10)$pad( ,8)"album": "$replace($replace($replace([%album%],\,\\),",\"),$tab(),\t)",$char(13)$char(10)$pad( ,8)"trackNumber": $replace("[%tracknumber%]","",null),$char(13)$char(10)$pad( ,8)"path": "$replace($replace($replace([%path%],\,\\),",\"),$tab(),\t)",$char(13)$char(10)$pad( ,4)},
```

* Group header pattern:<br/>
<code>
{$char(13)$char(10)$pad( ,4)"tracks": '['
</code>

* Group footer pattern:<br/>
<code>
$pad( ,4)']'$char(13)$char(10)}
</code>

#### Unformatted / not pretty
* Track pattern:<br/>
```
{"index":$sub(%list_index%,1),"title":"$replace($replace($replace([$meta(Title)],\,\\),",\"),$tab(),\t)","artist":"$replace($replace($replace([%artist%],\,\\),",\"),$tab(),\t)","album":"$replace($replace($replace([%album%],\,\\),",\"),$tab(),\t)","trackNumber":$replace("[%tracknumber%]","",null),"path":"$replace($replace($replace([%path%],\,\\),",\"),$tab(),\t)"},
```
* Group header pattern:<br/>
<code>
{"tracks":'['
</code>
* Group footer pattern:<br/>
<code>
']'}
</code>

This generates a file like the following:
```json
{
    "tracks": [ 
    {
        "title": "High Fidelity",
        "artist": "Daft Punk",
        "album": "Homework",
        "trackNumber": 01
        "path": "D:\\Macklin\\Music\\Daft Punk\\Homework\\High Fidelity\\10 High Fidelity.mp3"
    },
    ...
    ]
}
```

## Preferences

### General

<b>Logging</b>
* <b>Show console:</b><br/>
    Show or hide the console.
* <b>Full logging:</b><br/>
    Logs much more information (including some debug information) to the console. Enabling this will slow down processes, and will consume much more memory.
* <b>Log to file:</b><br/>
    Logs all console output to a file, in the same folder as the executable. This may have a significant performance impact when logging in the same thread, especially when "Full logging" is enabled. Consider enabling ""Log in separate thread"" if this has an impact on performance. This file is closed when the application is closed, and ideally shouldn't be open while the application is open.
* <b>Log in separate thread:</b><br/>
    Performs all logging in a separate thread. This greatly speeds up processing time while logging, but means that the logging is no longer in sync with the process, which has a side-effect of not being able to pause the application by selecting the log.

### Tagging
* <b>Dry run (not implemented in GUI):</b><br/>
    Dry run, doesn't write anything to files but will simulate output. This is useful for previewing the output of if2ktool without making changes. Note that this will still ask you to write tags, but won't follow through (promise!).
* <b>ID3</b>
    * <b>Force ID3v2 version (List):</b><br/>
    Force TagLib to write ID3v2 tags in a specific version for every file, rather than in the version of the source file.
    * <b>Don't add ID3v1 tags (bool):</b><br/>
    Prevent TagLib from adding an ID3v1 tag to files without one. If this is false, an ID3v1 tag will be created on every single file processed by TagLib.
    * <b>Force remove ID3v1 (bool):</b><br/>
    Forcibly remove all ID3v1 tags from files processed.
    * <b>Use numeric genres in ID3 (bool):</b><br/>
    Disable to prevent genres from being written as indices in ID3v2 tags.

### Mapping

* <b>Any Extension:</b><br/>
    If you wish to map to files that may have different extensions. It will try to match files with the original XML entry extension, then looks in the same directory and choose the first file with the same file name (minus extension) that is supported by TagLib.

* <b>Fuzzy distance:</b><br/>
    The maximum number of characters a file name can be off in order to be mapped. This value depends on how messed up your target file names are, so if perhaps they are missing a padded zero for the track number, or have had a special character substituted with '-' instead of '_', you can set the fuzzy distance to 1 to correct this.

    Internally this calculates the [Levenshtein distance](https://en.wikipedia.org/wiki/Levenshtein_distance) on every other file *name* (NOT file path, and minus extension) in the target folder, and then selects the one with the least changes necessary to make a match.

    Do NOT use a large value for the fuzzy distance, because then every name will be mapped to every other name!
    
    <!-- This is quite dangerous to use on albums where the track names are similar, so to mitigate the possiblity of something going really bad, we perform this operation at the very end of the match phase - and discount files that have already been matched with other entries. If there are multiple file names that could take the same amount of changes to the source file name, we pick the one with changes occuring further to the left of the filename. -->

* <b>Normalize strings:</b><br/>
    "Normalizes" the file path string if a file at the mapped/original path was not found. This converts the string to a "unicode-normalized form C", helping circumvent issues that may arise through the use of combining diacritic characters, and accents being able to be represented in different ways on the file system.

    For example the visual character `ō` can be represented in the URL-encoded path as either `%C5%8D` (decoded to `ō`), or `o%CC%84` (decoded to the separate `o` character, and a combining maron diacritic&ensp;`̄ ` &ensp;). In this example, both characters are perfectly valid for file names in Windows, and therefore you can have two files that appear to have the same name, but in fact are at different paths.
    
    If your XML uses one form, and the actual file name uses another, the path in the XML will not be found. This can occur when files are moved over a network, and some applications will automatically normalize the file name - while iTunes usually keeps whatever was used originally. This process simply calls [`string.Normalize()`](https://docs.microsoft.com/en-us/dotnet/api/system.string.normalize?view=netframework-4.7.2) where applicable, converting combining characters to their single character representations, among other things.

* <b>Check for duplicates:</b><br/>
    Having this option checked will force the mapper to ignore a matching path if an entry has already been mapped to it, preventing duplicates. This occurs inherantly in Lookup mode (even without this option enabled)

* <b>Lookup minimum matching percent:</b><br/>
    This is the minimum percentage of present and matching tags that an entry needs to have in order to be matched to a track in the lookup JSON file, during Lookup matching. This is 50% by default.
    
    Low percentages can yield incorrect matches, while higher percentages will limit the margin for error and can yield less matches. Note that a 100% match where both entries only have one present tag is still a perfect match!

## Troubleshooting + other stuff

### Missing iTunes Library XML
In recent versions of iTunes, Apple have hidden the XML from the iTunes directory. It can be shown again with the setting "Share iTunes Library XML wth other applications" under Edit -> Preferences -> Advanced.

### Resetting playback statistics in foobar2000
If at any point you wish to remove the saved playback statistics in foobar, you can delete the `index-data` folder in `C:\Users\user\AppData\Roaming\foobar2000\` or `%APPDATA%\foobar2000\`. Make sure that it isn't running while doing so. Note that this doesn't delete tagged playback statistics.

### Warning regarding ID3v2.4-tagged tracks not being recognized by foobar2000
TabLib has an issue with a small amount of ID3v2.4.0 files. After writing tags in if2ktool, the tags and ID3 version will be unable to be read by foobar2000 when importing the files for the first time. As a precaution, you should import your library into foobar2000 first. If this has occured, you can try editing the tags in another application and/or converting the tags from ID3v2.4 to v2.3 using a tool like [kid3](https://kid3.sourceforge.io/).

### Help! My target files are one character off, and aren't being found by the mapper!
You can try enabling the "Normalize strings" or "Fuzzy distance" options as above.

### Help! I'm getting an IOException while writing tags!
I've encountered this issue with about 30 or so of the files in my library or 20,000 or so. The exact error is `The process cannot access the file 'C:\path\to\the\file.mp3' because it is being used by another process."`. While sometime this can be the case, if you've accidentally left a program open which is locking the file (in which case, the tool prints the program in question), other times it occurs without explanation. I have a set of files that seem to always do this, no matter what. After looking into it, it seems TagLib is having trouble creating a WriteStream for these specific files. If this is the case, the tool will retry with modified load settings until the file is loaded correctly.