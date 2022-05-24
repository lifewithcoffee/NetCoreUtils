using McnLib.FileProcessing;

namespace McnLib.Parsers
{
    public interface ILineParser
    {
        bool ProcessLine(FileLine line);
    }
}