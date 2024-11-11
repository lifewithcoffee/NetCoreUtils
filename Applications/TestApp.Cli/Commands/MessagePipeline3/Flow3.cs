using System;

namespace TestApp.Cli.Commands.MessagePipeline3;

public class Flow3
{
    public Flow2 ParentMsg { get; set; }

    public Flow3 DoAction3_1() {
        Console.WriteLine("DoAction3_1");
        return this; 
    }
    public Flow3 DoAction3_2()
    { 
        Console.WriteLine("DoAction3_2");
        return this;
    }
    public Flow3 DoAction3_3()
    {
        Console.WriteLine("DoAction3_3");
        return this;
    }
}
