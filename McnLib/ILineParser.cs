using McnLib.FileReader;

namespace McnLib
{
    public interface ILineParser
    {
        void ProcessLine(FileLine line);
    }
}