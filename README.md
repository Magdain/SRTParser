SRTParser
=========
A very basic .SRT (SubRip) file parser. Project includes a WPF application with example usage, but Subtitle.cs is a standalone file that can be used in your own project.

```c#
List<Subtitle> subtitles = SRTParser.Subtitle.Parse("myfile.srt");
for (var i = 0; i < subtitles.Count; ++i)
{
  // Manipulate your subtitles.
}
```

Subtitle Properties
=========
int Index: The number of the caption group, 1-indexed.

TimeSpan Start: The start of the caption group in HH:MM:SS.MS format.

TimeSpan End: The end of the caption group in HH:MM:SS.MS format.

TimeSpan Duration: How long the caption group persists. (Equal to End - Start)

string Content: The actual text displayed for the caption group.

int Size: The character count of Content.
