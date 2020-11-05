using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NetCoreUtils.MethodCall;
using System.Threading;
using System.Reflection;
using System.IO;

namespace TestApp.Cli.Commands
{
    public class ProcUtil
    {
        public string GetDotnetExeFullPath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        public string GetDllDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }

    class ProcCommand
    {
        /// <summary>
        /// Test creating new process
        /// </summary>
        public void Test(int ms)
        {
            try
            {
                const string logPrefix = "Test :>";
                var procUtil = new ProcUtil();

                Console.WriteLine($"{logPrefix} Start {Process.GetCurrentProcess().Id}");

                var info = procUtil.GetDotnetExeFullPath(); // or just use "dotnet" directly
                //Process.Start(info,$@"{procUtil.GetDllDir()}\TestApp.Cli.dll proc work {ms}");
                Process.Start(info, $@"I:\rp\git\CoreCmd\DependentConsoleApp\bin\Debug\netcoreapp3.1\DependentConsoleApp.dll demo progress-bar");
                Console.WriteLine($"{logPrefix} Finish");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Test2()
        {
            Console.WriteLine(new ProcUtil().GetDllDir());
        }

        public void Work(int ms)
        {
            try
            {
                const string logPrefix = "Work :>";
                Console.WriteLine($"{logPrefix} Start {Process.GetCurrentProcess().Id}");
                int counter = 0;
                while (counter < 10)
                {
                    Console.WriteLine($"{logPrefix} Counter = {counter++}");
                    Thread.Sleep(ms);
                }
                Console.WriteLine($"{logPrefix} Finish");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }

        public void Id()
        {
            Console.WriteLine(Process.GetCurrentProcess().Id);
            this.Work(1000);
        }

        /// <summary>
        /// Get process by ID
        /// </summary>
        public void Get(int id)
        {
            try
            {
                Console.WriteLine(Process.GetProcessById(id).ProcessName);
            }
            catch(Exception)
            {
                Console.WriteLine($"Can't find process with PID as {id}");
            }
        }

        /// <summary>
        /// Use the OS's default browser to open a https URL
        /// </summary>
        public void Https(string address)
        {
            try
            {
                /** NOTE:
                 * The default value of ProcessStartInfo.UseShellExecute is true in .Net Framework
                 * but false in .Net Core, so it needs to be specified explicitly
                 **/
                Process.Start(new ProcessStartInfo($"https://{address}"){ UseShellExecute = true });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
