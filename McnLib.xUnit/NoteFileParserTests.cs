using McnLib.Parsers;
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
        [InlineData(0, "TestData/TestNote2.txt")]
        public void Test_ParseFile(int notesCount, string file)
        {
            var parser = new NoteFileParser();
            var noteFile = parser.ParseFile(file);
            Assert.Equal(notesCount, noteFile.Notes.Count);
        }

        [Fact]
        public void Temp()
        {
            var util = new ParsingUtil();
            Assert.True(util.IsHeaderLine("-"));
            Assert.True(util.IsHeaderLine("----------         "));
            Assert.True(util.IsHeaderLine("-------------------"));

            Assert.True(util.IsHeaderLine("="));
            Assert.True(util.IsHeaderLine("==========         "));
            Assert.True(util.IsHeaderLine("==================="));

            Assert.False(util.IsHeaderLine("------ x  "));
            Assert.False(util.IsHeaderLine("- -"));
            Assert.False(util.IsHeaderLine("--==="));
            Assert.False(util.IsHeaderLine("===abc"));
            Assert.False(util.IsHeaderLine("---abc"));
        }
    }
}