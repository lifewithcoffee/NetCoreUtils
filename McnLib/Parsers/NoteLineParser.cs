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

                if (trimmedText.StartsWith(BlockConfig.Begin))
                {
                    if (state.CurrentBlock != null && !state.CurrentBlock.IsBare)
                    {
                        // TODO: the previous block doesn't close, raise parser warning
                        state.CurrentFile.Blocks.Add(state.CurrentBlock);
                    }

                    state.CurrentBlock = new Block();
                    state.CurrentBlock.FileLines.Add(line);
                }
                else if (trimmedText.EndsWith(BlockConfig.End))
                {
                    if (state.CurrentBlock != null && !state.CurrentBlock.IsBare)
                    {
                        state.CurrentBlock.FileLines.Add(line);
                        state.CurrentFile.Blocks.Add(state.CurrentBlock);
                        state.CurrentBlock = null;
                    }
                    // TODO: else raise parser warning
                }
                else
                {
                    /**
                     * TODO: parse for sections, section/block meta data
                     */

                    if (state.CurrentBlock == null)
                    {
                        state.CurrentBlock = new Block { IsBare = true };
                    }
                    state.CurrentBlock.FileLines.Add(line);
                }
            }
        }
    }
}
