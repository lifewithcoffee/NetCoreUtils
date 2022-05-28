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

            for(int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var trimmedText = line.Text.Trim();

                if (trimmedText.StartsWith(Configs.NoteBegin))
                {
                    state.CurrentNote = new Note();
                    state.CurrentFile.Notes.Add(state.CurrentNote);
                }
                else if (trimmedText.EndsWith(Configs.NoteEnd))
                {
                    state.CurrentNote = null;
                }
                else
                {
                    int skipOverLines = 3;
                    string? line0 = line.Text;
                    string? line1 = i <= lines.Count - 2 ? lines[i + 1].Text : null;
                    string? line2 = i <= lines.Count - 3 ? lines[i + 2].Text : null;
                    string? line3 = i <= lines.Count - 4 ? lines[i + 3].Text : null;

                    // special adjustment for the 1st line of the file
                    if(i == 0)
                    {
                        skipOverLines = 2;
                        line3 = line2;
                        line2 = line1;
                        line1 = line0;
                        line0 = null;
                    }

                    if(util.IsSectionHeader(line0, line1, line2, line3))
                    {
                        state.CurrentNote = null;
                        i += skipOverLines;
                        continue;
                    }

                    /**
                     * TODO: parse for sections, section/note meta data
                     */
                    if (state.CurrentNote == null)
                    {
                        state.CurrentNote = new Note { IsBare = true };
                        state.CurrentFile.Notes.Add(state.CurrentNote);
                    }
                    state.CurrentNote!.FileLines.Add(line);
                }
            }
            return currentFile;
        }
    }
}
