using Serilog;
using System;

namespace TestApp.Cli.Commands.MessagePipeline2;

/// <summary>
/// The result of the previous step won't affect the next step, even if an
/// exception is thrown out in the previous step, i.e. ensure that every step
/// will be executed.
/// </summary>
public class ConcurrentFlow : FlowBase
{
    public override bool Execute()
    {
        foreach (var executable in this._executables)
        {
            try
            {
                executable.Execute();
            }
            catch (Exception ex)
            {
                //Log.Debug(ex.ToString());
                Console.WriteLine(ex);
            }
        }

        return true;
    }
}