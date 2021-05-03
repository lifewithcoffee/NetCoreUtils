using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NetCoreUtils.MethodCall;
using System.Threading;
using System.Reflection;
using System.IO;
using CoreCmd.Attributes;
using System.Runtime.InteropServices;

namespace TestApp.Cli.Commands
{
    static public class ProcUtil
    {
        /// <returns>The path of currently executing "dotnet.exe".</returns>
        static public string GetDotnetExeFullPath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        /// <returns>The path of the currently executing DLL.</returns>
        static public string GetDllDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <returns>The path of the specified type's DLL file.</returns>
        static public string GetDllFullPath(Type type)
        {
            return Assembly.GetAssembly(type).Location;
        }

        /// <returns>Determine if the specified terminal commnad exists.</returns>
        static public bool Exists(string command)
        {
            try
            {
                string systemCommand = "where";
                char separator = '\\';
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    systemCommand = "which";
                    separator = '/';
                }

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = systemCommand,
                        Arguments = command,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    if(line.Split(separator).Last().StartsWith(command))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
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
                Console.WriteLine($"{logPrefix} Start {Process.GetCurrentProcess().Id}");

                var info = ProcUtil.GetDotnetExeFullPath(); // or just use "dotnet" directly

                // start self
                // ==========
                var proc = Process.Start(info, $@"{ProcUtil.GetDllFullPath(this.GetType())} proc work {ms}");
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
            Console.WriteLine(ProcUtil.GetDllDir());
            Console.WriteLine(ProcUtil.GetDllFullPath(this.GetType()));
            Console.WriteLine(ProcUtil.GetDotnetExeFullPath());
        }

        [Help("Simulate a job processing by printing some info every specified ms")]
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

        [Help("Print the current process's ID")]
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

        [Help("Determine if the specified command exists")]
        public void Exist(string command)
        {
            if (ProcUtil.Exists(command))
                Console.WriteLine($"{command} exists");
            else
                Console.WriteLine($"{command} does not exists");
        }
    }
}
