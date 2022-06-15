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

            Assert.False(util.IsHeaderLine("  ================"));
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

            Assert.Equal(0, util.IsSectionHeader(null, null, "a", tripple));        // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, tripple, "a"));        // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, null, tripple));       // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, null, "a"));           // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, tripple, null));       // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, tripple, tripple));    // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, null, "a"));       // shouldn't happen, but method should work
            Assert.Equal(0, util.IsSectionHeader(null, null, null, null));      // shouldn't happen, but method should work

            Assert.Equal(3, util.IsSectionHeader(null, tripple, "a", tripple)); // begin of a file
            Assert.Equal(2, util.IsSectionHeader(null, "a", tripple,"b"));      // begin of a file

            Assert.Equal(0, util.IsSectionHeader(" ", tripple, "some header", null));
            Assert.Equal(0, util.IsSectionHeader(" ", tripple, "some header", single));

            Assert.Equal(0, util.IsSectionHeader(" ", tripple, "", single));
            Assert.Equal(0, util.IsSectionHeader(" ", tripple, " ", single));
            Assert.Equal(0, util.IsSectionHeader(" ", tripple, "", tripple));
            Assert.Equal(0, util.IsSectionHeader(" ", tripple, " ", tripple));

            Assert.Equal(0, util.IsSectionHeader(" ", "a", "a", tripple));
            Assert.Equal(0, util.IsSectionHeader(" ", "", "a", tripple));
            Assert.Equal(0, util.IsSectionHeader(" ", "  ", "a", tripple));

            Assert.Equal(2, util.IsSectionHeader(" ", "a", tripple, ""));
            Assert.Equal(2, util.IsSectionHeader(" ", "a", tripple, null)); // null: end of file
            Assert.Equal(2, util.IsSectionHeader(" ", "a", tripple, "a"));

            Assert.Equal(0, util.IsSectionHeader("a", tripple, "a", tripple));
            Assert.Equal(3, util.IsSectionHeader(" ", tripple, "a", tripple));

            // line0 must be a blank line or begin of file,i.e. null
            Assert.Equal(0, util.IsSectionHeader(tripple, "a", tripple, " "));
            Assert.Equal(0, util.IsSectionHeader(tripple, "a", tripple, "a"));
            Assert.Equal(0, util.IsSectionHeader(tripple, "a", tripple, null));
        }

    }
}