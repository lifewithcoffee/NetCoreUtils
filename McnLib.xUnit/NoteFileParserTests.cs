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

        [Theory]
        [InlineData(9, "TestData/TestNote1.txt")]
        [InlineData(1, "TestData/TestNote2.txt")]
        public void Test_ParseFile(int notesCount, string file)
        {
            var parser = new NoteFileParser();
            var noteFile = parser.ParseFile(file);
            Assert.Equal(notesCount, noteFile.Notes.Count);
        }
    }
}