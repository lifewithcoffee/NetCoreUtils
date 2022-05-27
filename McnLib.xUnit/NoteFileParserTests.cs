using McnLib.Parsers;
using Xunit;

namespace McnLib.xUnit
{
    public class NoteFileParserTests
    {
        [Fact]
        public void Test_ParseFolder()
        {
            var parser = new NoteFileParser();
            parser.ParseFolder("TestData");
            //parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");
            Assert.Equal(2, parser.NST.NoteFiles!.Count);
            Assert.Equal(10, parser.NST.GetAllNotes().Count);
        }

        [Fact]
        public void Test_ParseFile()
        {
            var parser = new NoteFileParser();
            var noteFile = parser.ParseFile("TestData/TestNote1.txt");
            Assert.Equal(9, noteFile.Notes.Count);

            noteFile = parser.ParseFile("TestData/TestNote2.txt");
            Assert.Equal(1, noteFile.Notes.Count);
        }
    }
}