using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands.MessagePipeline2;

public class SomeBL : ExecutableBase    // a test business logic object
{
    SequentFlow Flow { get; set; } = new();
    public override bool Execute()
    {
        try
        {
            Flow.AddStep(BLStepA)
                .AddStep("SomeBL's custom step 1", s => Console.WriteLine("SomeBL's custom step 1 executed."))
                .AddStep(BLStepB)
                .AddStep("SomeBL's custom step 2", s => Console.WriteLine("SomeBL's custom step 2 executed."))
                ;
            return Flow.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public void BLStepA(StepState state)
    {
        Console.WriteLine("BLStepA executed.");
    }

    public void BLStepB(StepState state)
    {
        Console.WriteLine("BLStepB executed.");
        state.Result = false;
    }
}
public class Pipeline2Command
{
    public void Do()
    {
        //Console.WriteLine("Pipeline2 command executed.");
        new SomeBL().Execute();
    }
}
