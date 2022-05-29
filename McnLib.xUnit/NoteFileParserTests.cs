using McnLib.Parsers;
using McnLib.States;
using System.Text.RegularExpressions;
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
        [InlineData(6, "TestData/TestNote1.txt")]
        [InlineData(1, "TestData/TestNote2.txt")]
        [InlineData(1, "TestData/TestNote3.txt")]
        [InlineData(1, "TestData/TestNote4.txt")]
        [InlineData(4, "TestData/TestNote5.txt")]
        [InlineData(2, "TestData/TestNote6.txt")]
        [InlineData(2, "TestData/TestNote7.txt")]
        public void Test_ParseFile(int notesCount, string file)
        {
            var noteFile = new NoteFileParser().ParseFile(file);
            Assert.Equal(notesCount, noteFile.Notes.Count);
        }
    }
}