using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands.MessagePipeline3;
public class Message1
{
    public Message2 ChildMsg { get; set; }

    public Message1 DoAction1_1()
    {
        return this;
    }

    public Message1 DoAction1_2()
    {
        return this;
    }

    public Message2 DoAction1_3()
    {
        return new Message2 { ParentMsg = this };
    }
}

public class Message2
{
    public Message1 ParentMsg { get; set; }
    public Message3 ChildMsg { get; set; }

    public Message2 DoAction2_1() { return this; }
    public Message2 DoAction2_2() { return this; }
    public Message3 DoAction2_3()
    {
        return new Message3 { ParentMsg = this };
    }
}

public class Message3
{
    public Message2 ParentMsg { get; set; }

    public Message3 DoAction3_1() { return this; }
    public Message3 DoAction3_2() { return this; }
    public Message3 DoAction3_3() { return this; }
}

public class BL1
{
    public void Do()
    {
        new Message1()
            .DoAction1_1()
            .DoAction1_2()
            .DoAction1_3()
            .DoAction2_1()
            .DoAction2_2()
            .DoAction2_3()
            .DoAction3_1()
            .DoAction3_2()
            .DoAction3_3()
            ;
    }
}
