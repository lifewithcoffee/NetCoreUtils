using McnLib.Blocks;
using McnLib.FileProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McnLib.Parsers
{
    public class TextFileParser
    {
        TextFileReader fileReader = new TextFileReader();
        FileLineParser lineParser = new FileLineParser();
        public void ParseFile(string filePath)
        {
            var lines = fileReader.ReadFile(filePath);
            lineParser.ParseLines(lines);       // TODO: (working) need to input an AST object
        }
    }
}
