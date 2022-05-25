using McnLib.States;

namespace McnLib.Parsers
{
    public class NoteLineReader
    {
        // TODO: handle UnauthorizedAccessException & PathTooLongException
        //       see https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readlines?redirectedfrom=MSDN&view=net-6.0#overloads
        public List<NoteLine> ReadFile(string filePath)
        {
            var result = new List<NoteLine>();

            int lineNumber = 1;
            foreach (var line in File.ReadLines(filePath))
            {
                result.Add(new NoteLine { LineNumber = lineNumber++, Text = line });
            }

            return result;
        }
    }
}