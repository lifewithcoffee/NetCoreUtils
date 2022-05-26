using McnLib.States;
using McnLib.Structures;

namespace McnLib.Parsers
{
    public class NoteLineParser
    {
        ParsingState state = new ParsingState ();

        public void ResetState(NoteFile currentFile)
        {
            state = new ParsingState { CurrentFile = currentFile };
        }

        public void ParseLines(List<NoteLine> lines)
        {
            if (state.CurrentFile == null)
                throw new Exception("Invalid current file");

            foreach (var line in lines)
            {
                var trimmedText = line.Text.Trim();

                if (trimmedText.StartsWith(Configs.NoteBegin))
                {
                    if (state.CurrentNote != null && !state.CurrentNote.IsBare)
                    {
                        // TODO: the previous note doesn't close, raise parser warning
                        state.CurrentFile.Notes.Add(state.CurrentNote);
                    }

                    state.CurrentNote = new Note();
                    state.CurrentNote.FileLines.Add(line);
                }
                else if (trimmedText.EndsWith(Configs.NoteEnd))
                {
                    if (state.CurrentNote != null && !state.CurrentNote.IsBare)
                    {
                        state.CurrentNote.FileLines.Add(line);
                        state.CurrentFile.Notes.Add(state.CurrentNote);
                        state.CurrentNote = null;
                    }
                    // TODO: else raise parser warning
                }
                else
                {
                    /**
                     * TODO: parse for sections, section/note meta data
                     */

                    if (state.CurrentNote == null)
                    {
                        state.CurrentNote = new Note { IsBare = true };
                    }
                    state.CurrentNote.FileLines.Add(line);
                }
            }
        }
    }
}
