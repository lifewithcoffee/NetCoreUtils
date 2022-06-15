using McnLib.Parsers;
using McnLib.States;
using System.Text.RegularExpressions;
using System.Linq;
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
            Assert.Equal(32, parser.NST.GetAllNotes().Count);
        }

        [Theory]
        [InlineData(11, "TestData/Parsing/TestNote1.txt")]
        [InlineData(4, "TestData/Parsing/TestNote2.txt")]
        [InlineData(3, "TestData/Parsing/TestNote3.txt")]
        [InlineData(3, "TestData/Parsing/TestNote4.txt")]
        [InlineData(4, "TestData/Parsing/TestNote5.txt")]
        [InlineData(3, "TestData/Parsing/TestNote6.txt")]
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
            Assert.Equal(foundLineCount, found[0].NotesFound.SelectMany(n => n.LinesFound).Count());
        }

        [Fact]
        public void Test_Parse_note_title_and_id()
        {
            var noteFile = new NoteFileParser().ParseFile( "TestData/Parsing/TestNote11.txt");
            Assert.Equal(11, noteFile.Notes.Count);

            Assert.Null(noteFile.Notes[0].Title);
            Assert.Null(noteFile.Notes[0].Id);

            Assert.Null(noteFile.Notes[1].Title);
            Assert.Null(noteFile.Notes[1].Id);

            Assert.Null(noteFile.Notes[3].Title);
            Assert.Null(noteFile.Notes[3].Id);

            Assert.Null(noteFile.Notes[5].Title);
            Assert.Null(noteFile.Notes[5].Id);

            Assert.Null(noteFile.Notes[6].Title);
            Assert.Null(noteFile.Notes[6].Id);

            Assert.Null(noteFile.Notes[9].Title);
            Assert.Null(noteFile.Notes[9].Id);

            Assert.Null(noteFile.Notes[10].Title);
            Assert.Null(noteFile.Notes[10].Id);

            Assert.Equal("Normal note 1", noteFile.Notes[2].Title);
            Assert.Equal("2022_0526_234506", noteFile.Notes[2].Id);

            Assert.Equal("test title 2", noteFile.Notes[4].Title);
            Assert.Equal("2022_1526_233617", noteFile.Notes[4].Id);

            Assert.Equal("test title 3", noteFile.Notes[7].Title);
            Assert.Equal("2042_0526_234506", noteFile.Notes[7].Id);

            Assert.Equal("test title 4", noteFile.Notes[8].Title);
            Assert.Equal("2192_0526_234617", noteFile.Notes[8].Id);
        }
    }
}