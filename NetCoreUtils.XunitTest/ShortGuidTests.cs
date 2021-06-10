using NetCoreUtils.Text.String;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NetCoreUtils.XunitTest
{
    public class ShortGuidTests
    {
        private readonly ITestOutputHelper output;
        public ShortGuidTests(ITestOutputHelper output) { this.output = output; }

        [Fact]
        public void DoTest()
        {
            string shortGuid = ShortGuid.NewGuid();
            output.WriteLine($"shortGuid\t: {shortGuid}");

            string decodedGuid = ShortGuid.Decode(shortGuid).ToString();
            output.WriteLine($"decodedGuid\t: {decodedGuid}");

            Assert.Equal(shortGuid, ShortGuid.Encode(decodedGuid));
        }
    }
}
