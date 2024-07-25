using System;

namespace TestApp.Cli.Commands.MessagePipeline2;

public class StepState
{
    public bool Started { get; set; } = false;
    public bool Finished { get; set; } = false;
    public bool Result { get; set; } = true;
    public Type Type { get; private set; }
    public Object Data { get; private set; }

    public void AddData(Type type, Object data)
    {
        Type = type;
        Data = data;
    }
}
