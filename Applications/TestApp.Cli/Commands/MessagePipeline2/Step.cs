using System;

namespace TestApp.Cli.Commands.MessagePipeline2;

public class Step : IExecutable
{
    string ExecutableType { get; } = nameof(Step);  // for debugging
    public StepState State { get;set; } = new StepState();
    public string Name { get; set; }
    Action<StepState> _doAction { get; } = state => { };
    public bool Condition { get; private set; } = true;
    public Step(string name, Action<StepState> doAction)
    {
        Name = name;
        _doAction = doAction;
    }
    public Step(string name, Action<StepState> doAction, Func<bool> condition = null)
    {
        this.Name = name;
        this._doAction = doAction;
        this.Condition = condition == null ? true : condition();
    }

    public bool Execute()
    {
        if (this.Condition)
        {
            State.Started = true;

            _doAction(this.State);

            if (!this.State.Result)
                return false;

            State.Finished = true;
        }

        return true;
    }
}
