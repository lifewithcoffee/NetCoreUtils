using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands.MessagePipeline3;
public class Flow1
{
    public Msg1 Msg1 { get; set; }

    public Flow1 DoAction1_1()
    {
        Console.WriteLine("DoAction1_1");
        return this;
    }

    public Flow1 DoAction1_2()
    {
        Console.WriteLine("DoAction1_2");
        return this;
    }

    public Flow2 DoAction1_3()
    {
        Console.WriteLine("DoAction1_3");
        return new Flow2 { ParentMsg = this };
    }
}

public class Msg1
{
    public int Value { get; set; }
}
