using NetCoreUtils.MethodCall.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Cli.Commands.MessagePipeline.ChainStyleImpl;
using TestApp.Cli.Commands.MessagePipeline.DemoImpls;

namespace TestApp.Cli.Commands.MessagePipeline
{
    class PipelineCommand
    {
        public void Do()
        {
            new ChainProcedure<Message1>()
                .AddStep(new Step1())
                .AddStep(new Step2())
                .AddStep(new Step3())
                .AddStep(new Step4())
                .AddStep(msg => {
                    Console.WriteLine($"Do Fun<,> step, data = {msg.Data}");
                    return true;
                })
                .Execute(new Message1());
        }

        public void Do2()
        {
            new Procedure<Message1>()
                .AddStep(msg =>
                {
                    Console.WriteLine(msg.Data);
                    msg.Data = "111";
                    return true;
                })
                .AddStep(msg =>
                {
                    Console.WriteLine(msg.Data);
                    msg.Data = "222";
                    return true;
                })
                .AddStep(new step2a())
                .AddStep(new step2b())
                .AddStep(msg =>
                {
                    Console.WriteLine(msg.Data);
                    msg.Data = "333";
                    return true;
                })
                .Execute(new Message1 { Data = "init" });
        }
    }
}
