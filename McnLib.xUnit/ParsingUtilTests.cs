using McnLib.Parsers;
using Xunit;

namespace McnLib.xUnit
{
    public class ParsingUtilTests
    { 
        [Fact]
        public void Test_IsHeaderLine()
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

        [Theory]
        [InlineData('-')]
        [InlineData('=')]
        public void Test_IsSectionHeader2(char lineCharacter)
        {
            var util = new ParsingUtil();

            string tripple = new string(lineCharacter, 3);
            string single = $"{lineCharacter}";

            Assert.False(util.IsSectionHeader(" ", tripple, "some header", null));
            Assert.False(util.IsSectionHeader(" ", tripple, "some header", single));

            Assert.False(util.IsSectionHeader(" ", tripple, "", single));
            Assert.False(util.IsSectionHeader(" ", tripple, " ", single));

            Assert.False(util.IsSectionHeader(" ", tripple, "", tripple));
            Assert.False(util.IsSectionHeader(" ", tripple, " ", tripple));

            Assert.False(util.IsSectionHeader(" ", "a", "a", tripple));
            Assert.True(util.IsSectionHeader(" ", "", "a", tripple));
            Assert.True(util.IsSectionHeader(" ", "  ", "a", tripple));

            Assert.False(util.IsSectionHeader("a", tripple, "a", tripple));
            Assert.True(util.IsSectionHeader(" ", tripple, "a", tripple));

            // begin of a file
            Assert.True(util.IsSectionHeader(null, tripple, "a", tripple));
            Assert.True(util.IsSectionHeader(null, null, "a", tripple));
            Assert.True(util.IsSectionHeader(null, "a", tripple,"b"));
        }

    }
}