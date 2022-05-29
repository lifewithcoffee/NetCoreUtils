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
                    state.CurrentNote = new Note();
                    state.CurrentFile.Notes.Add(state.CurrentNote);
                }
                else if (trimmedText != null && trimmedText.EndsWith(Configs.NoteEnd))
                {
                    state.CurrentNote = new Note { IsBare = true };
                    state.CurrentFile.Notes.Add(state.CurrentNote);
                }
                else
                {
                    const int skipOverLines = 3;
                    string? line0 = line != null ? line.Text : null;
                    string? line1 = i <= lines.Count - 2 ? lines[i + 1].Text : null;
                    string? line2 = i <= lines.Count - 3 ? lines[i + 2].Text : null;
                    string? line3 = i <= lines.Count - 4 ? lines[i + 3].Text : null;

                    // parse header sections and skip over the header lines
                    if(util.IsSectionHeader(line0, line1, line2, line3))
                    {
                        state.CurrentNote = new Note { IsBare = true };
                        state.CurrentFile.Notes.Add(state.CurrentNote);
                        i += skipOverLines;
                        continue;
                    }

                    if(state.CurrentNote == null)
                    {
                        state.CurrentNote = new Note { IsBare = true };
                        state.CurrentFile.Notes.Add(state.CurrentNote);
                    }
                    else
                    {
                        if (line == null)
                            throw new Exception($"Null reference for line ${i + 1} in file ${currentFile.FullPath}");

                        // skip over the first blank lines
                        if(state.CurrentNote.FileLines.Count == 0 && string.IsNullOrWhiteSpace(line.Text))
                            continue;

                        state.CurrentNote.FileLines.Add(line);
                    }
                }
            }

            // remote empty notes
            state.CurrentFile.Notes.RemoveAll(n => n.FileLines.Count == 0);

            return currentFile;
        }
    }
}
