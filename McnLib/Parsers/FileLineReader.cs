using McnLib.States;

namespace McnLib.Parsers
{
    public class FileLineReader
    {
        // TODO: handle UnauthorizedAccessException & PathTooLongException
        //       see https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readlines?redirectedfrom=MSDN&view=net-6.0#overloads
        public List<FileLine> ReadFile(string filePath)
        {
            var result = new List<FileLine>();

            int lineNumber = 1;
            foreach (var line in File.ReadLines(filePath))
            {
                result.Add(new FileLine { LineNumber = lineNumber++, Text = line });
            }

            return result;
        }
    }
}