namespace TestApp.Cli.Commands.MessagePipeline3;

public class Pipeline3Command
{
    public void Do()
    {
        new Flow1()
            .DoAction1_1()?
            .DoAction1_2()?
            .DoAction1_3()?
            .DoAction2_1()?
            .DoAction2_2()?
            .DoAction2_3()?
            .DoAction3_1()?
            .DoAction3_2()?
            .DoAction3_3()
            ;
    }
}
