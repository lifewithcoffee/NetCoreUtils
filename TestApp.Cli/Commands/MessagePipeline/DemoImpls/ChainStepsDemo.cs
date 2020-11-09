using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl;

namespace TestApp.Cli.Commands.MessagePipeline.DemoImpls
{
    public class Message1
    {
        public string Data { get; set; }
    }

    public class Step1 : ChainStep<Message1>
    {
        protected override bool Handle(Message1 msg)
        {
            msg.Data = "data1";
            Console.WriteLine($"Do step1 data = {msg.Data}");
            return true;
        }
    }

    public class Step2 : ChainStep<Message1>
    {
        protected override bool Handle(Message1 msg)
        {
            msg.Data = "data2";
            Console.WriteLine($"Do step2 data = {msg.Data}");
            return true;
        }
    }

    public class Step3 : ChainStep<Message1>
    {
        protected override bool Handle(Message1 msg)
        {
            msg.Data = "data3";
            Console.WriteLine($"Do step3 data = {msg.Data}");
            return true;
        }
    }

    public class Step4 : ChainStep<Message1>
    {
        protected override bool Handle(Message1 msg)
        {
            msg.Data = "data4";
            Console.WriteLine($"Do step4 data = {msg.Data}");
            return true;
        }
    }
}