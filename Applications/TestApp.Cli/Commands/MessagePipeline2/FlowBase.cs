using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands.MessagePipeline2;
public abstract class FlowBase : IExecutable
{
    string ExecutableType { get; } = nameof(FlowBase);  // for debugging
    string Name { get; set; }

    protected List<IExecutable> _executables = new List<IExecutable>();

    public FlowBase AddStep(Action<StepState> action)
    {
        this._executables.Add(new Step(action.Method.Name, action));
        return this;
    }

    public FlowBase AddStep(string name, Action<StepState> action)
    {
        this._executables.Add(new Step(name, action));
        return this;
    }

    //public Flow AddSubFlow(Flow flow)
    //{
    //    this._executables.Add(flow);
    //    return this;
    //}

    public FlowBase AddExecutable(IExecutable executable)
    {
        this._executables.Add(executable);
        return this;
    }

    /// <summary>
    /// Similar to Mule's Scatter-Gather, the added executables will be execute
    /// concurrently, one failure won't affect others.
    /// </summary>
    public FlowBase AddConcurrentFlow(params IExecutable[] executables)
    {
        // TODO: need a ConcurrentFlow class
        throw new NotImplementedException();
    }

    public abstract bool Execute();
}
