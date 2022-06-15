using McnLib.States;
using McnLib.Structures;

namespace McnLib.Parsers
{
    public class NoteLineParser
    {
        ParsingState state = new ParsingState ();
        ParsingUtil util = new ParsingUtil ();

        private List<NoteLine> ReadFile(string[] fileContent)
        {
            var result = new List<NoteLine>();

            int lineNumber = 1;
            foreach (var line in fileContent)
            {
                result.Add(new NoteLine { LineNumber = lineNumber++, Text = line });
            }

            return result;
        }

        public NoteFile ParseLines(NoteFile currentFile)
        {
            state = new ParsingState { CurrentFile = currentFile };

            if (state.CurrentFile == null)
                throw new Exception("Invalid current file");

            List<NoteLine> lines = ReadFile(state.CurrentFile.Content);

            NoteLine? line = null;
            string? trimmedText = null;

            // as util.IsSectionHeader() requires a blank line before a header, so start from -1
            for(int i = -1; i < lines.Count; i++)
            {
                // parsing the first line
                if(i >= 0)
                {
                    line = lines[i];
                    trimmedText = line.Text.Trim();
                }

                if (trimmedText != null && trimmedText.StartsWith(Configs.NoteBegin))
                {
                    state.ResetCurrentNote();
                }
                else if (trimmedText != null && trimmedText.EndsWith(Configs.NoteEnd))
                {
                    state.ResetCurrentNote(true);
                }
                else
                {
                    string? line0 = line != null ? line.Text : null;
                    string? line1 = i <= lines.Count - 2 ? lines[i + 1].Text : null;
                    string? line2 = i <= lines.Count - 3 ? lines[i + 2].Text : null;
                    string? line3 = i <= lines.Count - 4 ? lines[i + 3].Text : null;

                    // parse header sections and skip over the header lines
                    int skipOverLines = util.IsSectionHeader(line0, line1, line2, line3);
                    if(skipOverLines != 0)
                    {
                        state.ResetCurrentNote(true);
                        i += skipOverLines;
                        continue;
                    }

                    if(state.CurrentNote == null)   // for content on the top of a file
                    {
                        state.ResetCurrentNote(true);
                    }

                    if (line != null)
                    {
                        state.AddLine(line);  // including blank lines

                        // only get title info from the note's 1st line
                        if (state.CurrentNoteLineCount == 1 && string.IsNullOrWhiteSpace(state.CurrentNote!.Title))
                            state.CurrentNote.ParseTitle(line);

                        // only get id info from the note's 2nd line
                        if (state.CurrentNoteLineCount == 2 && string.IsNullOrWhiteSpace(state.CurrentNote!.Id))
                            state.CurrentNote.ParseId(line);
                    }
                }
            }

            // remove empty notes
            state.CurrentFile.Notes.RemoveAll(n => n.FileLines.Count == 0);

            return currentFile;
        }
    }
}
