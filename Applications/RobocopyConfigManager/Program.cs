using CoreCmd.CommandExecution;
using System;

namespace RobocopyConfigManager
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            await new AssemblyCommandExecutor().ExecuteAsync(args, s =>
            {
            });

        }
    }
}
