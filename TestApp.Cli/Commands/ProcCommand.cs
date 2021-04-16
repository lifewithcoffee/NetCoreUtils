using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NetCoreUtils.MethodCall;
using System.Threading;
using System.Reflection;
using System.IO;
using CoreCmd.Attributes;

namespace TestApp.Cli.Commands
{
    public class ProcUtil
    {
        // return the "dotnet.exe" path
        public string GetDotnetExeFullPath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        public string GetDllDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public string GetDllFullPath()
        {
            return Assembly.GetAssembly(this.GetType()).Location;
        }
    }

    class ProcCommand
    {
        [Help("Spawn a new process")]
        public void Spawn(int ms)
        {
            try
            {
                const string logPrefix = "Test :>";
                var procUtil = new ProcUtil();
                Console.WriteLine($"{logPrefix} Start {Process.GetCurrentProcess().Id}");

                var info = procUtil.GetDotnetExeFullPath(); // or just use "dotnet" directly

                // start self
                // ==========
                var proc = Process.Start(info, $@"{procUtil.GetDllFullPath()} proc work {ms}");
                //var proc = Process.Start(info,$@"{procUtil.GetDllFullPath()} proc dll")

                // start a different dll
                // =====================
                //var proc = Process.Start(info, $@"I:\rp\git\CoreCmd\DependentConsoleApp\bin\Debug\netcoreapp3.1\DependentConsoleApp.dll demo progress-bar");

                /** NOTE
                 * If not call WaitForExit(), when the program will:
                 * - execute asynchronously
                 * - wait for console input to quit when the spawned process finishes ---> don't know why
                 */
                proc.WaitForExit();
                Console.WriteLine($"{logPrefix} Finish");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Help("Print current process's assembly info")]
        public void Dll()
        {
            Console.WriteLine(new ProcUtil().GetDllDir());
            Console.WriteLine(new ProcUtil().GetDllFullPath());
            Console.WriteLine(new ProcUtil().GetDotnetExeFullPath());
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

        [Help("Get process by ID")]
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

        [Help("Use the OS's default browser to open a https URL")]
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
