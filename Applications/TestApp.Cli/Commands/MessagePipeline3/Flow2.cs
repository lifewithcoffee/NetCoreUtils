using System;

namespace TestApp.Cli.Commands.MessagePipeline3;

public class Flow2
{
    public Flow1 ParentMsg { get; set; }
    public Flow3 ChildMsg { get; set; }

    public Flow2 DoAction2_1()
    {
        Console.WriteLine("DoAction2_1");
        return null;
    }
    public Flow2 DoAction2_2()
    { 
        Console.WriteLine("DoAction2_2");
        return this; 
    }
    public Flow3 DoAction2_3()
    {
        Console.WriteLine("DoAction2_3");
        return new Flow3 { ParentMsg = this };
    }
}
