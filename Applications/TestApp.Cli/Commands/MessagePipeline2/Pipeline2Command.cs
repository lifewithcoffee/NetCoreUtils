using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands.MessagePipeline2;

public class SomeBL2 : IExecutable
{
    ConcurrentFlow _flow = new ConcurrentFlow();
    public bool Execute()
    {
        this._flow
            .AddStep(BLStepA)
            .AddStep("BL2 custom step 1", s => Console.WriteLine("BL2 custom step 1 executed."))
            .AddStep(BLStepB)
            .AddStep("BL2 custom step 2", s => Console.WriteLine("BL2 custom step 2 executed."))
            ;
        return _flow.Execute();
    }

    public void BLStepA(StepState state)
    {
        Console.WriteLine("BL2 StepA executed.");
        state.Result = false;
        throw new Exception("bla bla");
    }

    public void BLStepB(StepState state)
    {
        Console.WriteLine("BL2 StepB executed.");
        state.Result = false;
    }
}

public class SomeBL1 : IExecutable    // a test business logic object
{
    SequentFlow _flow = new();
    public bool Execute()
    {
        try
        {
            this._flow
                .AddStep(BLStepA)
                .AddStep("BL1 custom step 1", s => Console.WriteLine("BL1 custom step 1 executed."))
                .AddExecutable(new SomeBL2())
                .AddStep(BLStepB)
                .AddStep("BL1 custom step 2", s => Console.WriteLine("BL1 custom step 2 executed."))
                ;
            return _flow.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public void BLStepA(StepState state)
    {
        Console.WriteLine("BL1 StepA executed.");
    }

    public void BLStepB(StepState state)
    {
        Console.WriteLine("BL1 StepB executed.");
        //state.Result = false;
    }
}
public class Pipeline2Command
{
    public void Do()
    {
        //Console.WriteLine("Pipeline2 command executed.");
        new SomeBL1().Execute();
    }
}
