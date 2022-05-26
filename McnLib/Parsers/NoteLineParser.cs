using McnLib.States;
using McnLib.Structures;

namespace McnLib.Parsers
{
    public class NoteLineParser
    {
        ParsingState state = new ParsingState ();

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
                    var next1stLine = (i <= lines.Count - 2 ? lines[i + 1] : null);
                    var next2ndLine = (i <= lines.Count - 3 ? lines[i + 2] : null);

                    if(next1stLine == null)
                    {
                       if(IsSectionHeader(line, next1stLine, next2ndLine))
                        {
                            state.CurrentNote = null;

                            // skip over the section header lines
                            if (next2ndLine != null)
                                i += 2;
                            else
                                i += 1;

                            continue;
                        }
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

        private bool IsSectionHeader(NoteLine current, NoteLine next1st, NoteLine next2nd)
        {
            return false;
        }
    }
}
