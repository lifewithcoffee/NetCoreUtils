using McnLib.FileReader;

namespace McnLib.Blocks
{
    public class BlockParser : ILineParser
    {
        Block? nextBlock = null;
        List<Block> blocks = new List<Block>();
        public List<Block> Blocks { get { return blocks; } }

        public void ProcessLine(FileLine line)
        {
            var trimmedText = line.Text.Trim();
            if (trimmedText.StartsWith(BlockConfig.Begin))
            {
                nextBlock = new Block();
                nextBlock.FileLines.Add(line);
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
            }
        }
    }
}