using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using CoreCmd.CommandExecution;
using System.IO;

namespace NetCoreUtils.TestCli
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }
    }
}
