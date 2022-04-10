using McnLib.FileReader;

namespace McnLib
{
    public interface ILineParser
    {
        bool ProcessLine(FileLine line);
    }
}