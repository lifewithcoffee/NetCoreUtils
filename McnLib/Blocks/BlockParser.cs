using McnLib.FileReader;

namespace McnLib.Blocks
{
    public class BlockParser : ILineParser
    {
        Block? nextBlock = null;
        List<Block> blocks = new List<Block>();
        public List<Block> Blocks { get { return blocks; } }

        public bool ProcessLine(FileLine line)
        {
            var trimmedText = line.Text.Trim();
            if (trimmedText.StartsWith(BlockConfig.Begin))
            {
                nextBlock = new Block();
                nextBlock.FileLines.Add(line);
                return true;
            }
            else if (trimmedText.EndsWith(BlockConfig.End))
            {
                if (nextBlock != null)
                {
                    nextBlock.FileLines.Add(line);
                    blocks.Add(nextBlock);
                    nextBlock = null;
                }
                // TODO: else raise parser warning
                return true;
            }
            return false;
        }
    }
}