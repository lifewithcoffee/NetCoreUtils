using TestApp.Cli.Commands.MessagePipeline2;

namespace TestApp.Cli.XunitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        new SomeBL().Execute();
    }
}