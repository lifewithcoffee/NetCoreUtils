using NetCoreUtils.MethodCall.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Cli.Commands.MessagePipeline.DemoImpls
{
    class step2a : IStep<Message1>
    {
        public bool Execute(Message1 msg)
        {
            Console.WriteLine(msg.Data);
            msg.Data = "step2a";
            return true;
        }
    }

    class step2b : IStep<Message1>
    {
        public bool Execute(Message1 msg)
        {
            Console.WriteLine(msg.Data);
            msg.Data = "step2b";
            return false;
        }
    }
}
