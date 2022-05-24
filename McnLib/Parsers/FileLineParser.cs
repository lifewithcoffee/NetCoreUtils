using McnLib.FileProcessing;
using McnLib.Structures;

namespace McnLib.Parsers
{
    public class FileLineParser
    {
        ILineParser[] parsers = { new SectionParser(), new BlockParser() };
        public void ParseLines(List<FileLine> lines)
        {
            foreach (var line in lines)
            {
                foreach (var parser in parsers)
                {
                    if (parser.ProcessLine(line))
                        continue;
                }
            }
        }

        public List<Block> GetBlocks()
        {
            var blockParser = parsers.Where(parser => parser is BlockParser).First();
            return ((BlockParser)blockParser).Blocks;
        }
    }
}
