; This script creates a JSON file using foobar2000's "Text Tools" plugin for use in if2ktool, containing information on each track:
; 1. Read the patterns from the json_patterns.txt file into some variables (it's easier than declaring them here)
; 2. Open foobar2000 to the "Album List" window (this component should be installed by default). We use album list to provide a context for all music.
; 3. Right click "All Music" to show the context menu
; 4. Navigate to "Utilities -> Text Tools -> Advanced" to open the Text Tools window
; 5. Enter all the patterns into their associated fields.
; 6. Write the text in the output field to stdout (or the clipboard)

; It takes the following optional arguments:
; /FoobarExe - Path to foobar exe, to override the default as below
; /ToClipboard - Copies the output to the clipboard (default)
; /ToStdout - Writes the output to stdout
; /Formatted - Uses the second set of patterns in json_patterns.txt

; Note that generating a JSON file with this script will mean that you need to sort foobar2000 by album (or more ideally use the Album List) when using an exported masstagger script.

#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
SetControlDelay, -1

winAlbumList := "ahk_class {483DF8E3-09E3-40d2-BEB8-67284CE3559F}"
winTextTools := "ahk_class #32770"
foobarPath := "C:\Program Files (x86)\foobar2000\foobar2000.exe"
textFileOffset := 3

; Parse command-line arguments (%0% is the amount of arguments passed)
Loop, %0%
{
	index := A_Index
	
	if (%index% = "/FoobarPath")
	{
		index := ++index
		foobarPath := %index%
	}
	else if (%index% = "/ToClipboard")
		to := "clipboard"
	else if (%index% = "/ToStdout")
		to := "stdout"
	else if (%index% = "/Formatted")
		textFileOffset := 0
}

FileReadLine, trackPattern, json_patterns.txt, 1 + textFileOffset
FileReadLine, groupHeader, json_patterns.txt, 2 + textFileOffset
FileReadLine, groupFooter, json_patterns.txt, 3 + textFileOffset

; Show the "Album List" in foobar2000, by executing it with a /command flag (see foobar2000 docs)
Run, %foobarPath% /command:"Album List"

; Wait for the above window to exist
WinWait, %winAlbumList%, , 5

; Bring the window to the front (just in case)
WinActivate, %winAlbumList%

; Right click "All Music" to open the context menu where the context is all items
ControlClick, x20 y40, %winAlbumList%, , R, ,

; In the context menu press U "Utilities", T "Text Tools", D "Advanced"
ControlSend, ahk_parent, utd, %winAlbumList%

; Wait for the "Text Tools" window to open, then activate it
WinWait, %winTextTools%, , 5
WinActivate, %winTextTools%

; Check both of the group checkboxes
Control, Check, , Button1, %winTextTools%
Control, Check, , Button2, %winTextTools%

; Uncheck "Skip/duplicate repeating lines" just in case
Control, Uncheck, , Button4, %winTextTools%

; Fill the form out (ControlSetText doesn't trigger refresh, to we have to pase the text in instead)

; First clear any text already in the boxes
ControlSetText, Edit1, , %winTextTools%
ControlSetText, Edit2, , %winTextTools%
ControlSetText, Edit3, , %winTextTools%
ControlSetText, Edit3, , %winTextTools% 

; Next paste the patterns in
Control, EditPaste, %trackPattern%, Edit1, %winTextTools%
Control, EditPaste, %groupHeader%, Edit2, %winTextTools%
Control, EditPaste, %groupFooter%, Edit3, %winTextTools%

; Wait for the results to be generated
Sleep, 1000

; Read all the text from the output field
ControlGetText, outputJsonText, Edit4, %winTextTools%

; Close opened windows
WinClose, %winTextTools%
WinClose, %winAlbumList%

; Open stdout stream and write json to it
if (to == "stdout")
{
	FileEncoding, UTF-8
	stdout := FileOpen("*", "w `n")

	; Write the result text to the ouput stream
	stdout.Write(outputJsonText)
	stdout.Close()
}

; Copy the text to the clipboard
else 
{
	clipboard = %outputJsonText%
	MsgBox, The output text was successfully copied to the clipboard, though no check was made to ensure its validity.
}