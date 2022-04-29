using McnLib.Blocks;
using McnLib.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McnLib.FileProcessing
{
    public class TextFileParser
    {
        TextFileReader fileReader = new TextFileReader();
        FileLineParser lineParser = new FileLineParser();
        public void ParseFile(string filePath)
        {
            lineParser.ParseLines(fileReader.ReadFile(filePath));
        }
    }

    public class FileLineParser
    {
        ILineParser[] parsers = { new SectionParser(), new BlockParser() };
        public void ParseLines(List<FileLine> lines)
        {
            foreach(var line in lines)
            {
                foreach(var parser in parsers)
                {
                    if(parser.ProcessLine(line))
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
