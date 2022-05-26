using McnLib.Parsers;
using Xunit;

namespace McnLib.xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void NoteLineReader_should_work()
        {
            var reader = new NoteLineReader();
            var lines = reader.ReadFile("TestData/Testnote1.txt");
            Assert.NotEmpty(lines);
            Assert.Equal(1, lines[0].LineNumber);
            Assert.True(lines[1].LineNumber > 1);
        }
    }
}