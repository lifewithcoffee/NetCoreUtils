namespace TestApp.Cli.Commands.MessagePipeline2;

/// <summary>
/// If a previous step fails, the next step will not be excuted, i.e. the flow
/// exists immediately when one step fails.
/// </summary>
public class SequentFlow : FlowBase
{
    public override bool Execute()
    {
        foreach (var executable in this._executables)
        {
            if (!executable.Execute())
                return false;
        }

        return true;
    }
}