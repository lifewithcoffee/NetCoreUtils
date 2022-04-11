using McnLib.FileProcessing;

namespace McnLib
{
    public interface ILineParser
    {
        bool ProcessLine(FileLine line);
    }
}