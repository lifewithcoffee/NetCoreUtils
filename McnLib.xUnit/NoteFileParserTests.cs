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
            parser.ParseFolder("TestData/Parsing");
            //parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");
            Assert.Equal(9, parser.NST.NoteFiles!.Count);
            Assert.Equal(19, parser.NST.GetAllNotes().Count);
        }

        [Theory]
        [InlineData(6, "TestData/Parsing/TestNote1.txt")]
        [InlineData(1, "TestData/Parsing/TestNote2.txt")]
        [InlineData(1, "TestData/Parsing/TestNote3.txt")]
        [InlineData(1, "TestData/Parsing/TestNote4.txt")]
        [InlineData(4, "TestData/Parsing/TestNote5.txt")]
        [InlineData(2, "TestData/Parsing/TestNote6.txt")]
        [InlineData(2, "TestData/Parsing/TestNote7.txt")]
        [InlineData(1, "TestData/Parsing/TestNote8.txt")]
        [InlineData(1, "TestData/Parsing/TestNote9.txt")]
        public void Test_ParseFile(int notesCount, string file)
        {
            var noteFile = new NoteFileParser().ParseFile(file);
            Assert.Equal(notesCount, noteFile.Notes.Count);
        }

        [Theory]
        [InlineData(23, new string[] { "the" })]
        [InlineData(5, new string[] { "that", "him" })] 
        [InlineData(1, new string[] { "BJay", "former" })]      // two keywords in the same line
        [InlineData(5, new string[] { "that", "him", "   THAT   ", "him"})]     // duplicated & non-trimmed keywords
        public void Test_FindNotes(int foundLineCount, string[] keywords)
        {
            var parser = new NoteFileParser();
            parser.ParseFolder("TestData/Search");

            var found = parser.NST.FindNotes(keywords);
            Assert.Equal(foundLineCount, found[0].LinesFound.Count);
        }
    }
}