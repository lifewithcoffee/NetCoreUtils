using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using NetCoreUtils.MethodCall;

namespace NetCoreUtils.ProcessUtils
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

        /// <summary>
        /// NOTE: works in Windows 10, not tested in linux
        /// </summary>
        /// <returns>Determine if the specified terminal commnad exists.</returns>
        static public bool Exists(string command)
        {
            try
            {
                string systemCommand = "where";
                char separator = '\\';
                bool caseSensitive = false;
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    systemCommand = "which";
                    separator = '/';
                    caseSensitive = true;
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
                    //Console.WriteLine($"DEBUG: {line} | {separator} | {line.Split(separator).Last()}");

                    string commandFound = line.Split(separator).Last();

                    bool result = false;
                    if(caseSensitive)
                    {
                        result = commandFound.StartsWith(command.Trim()); ;
                    }
                    else
                    {
                        result = commandFound.ToLower().StartsWith(command.ToLower().Trim());
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        static public void Run(string command, string arguments="")
        {
            SafeCall.Execute(() => {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    Console.WriteLine(line);
                }

                while (!proc.StandardError.EndOfStream)
                {
                    string line = proc.StandardError.ReadLine();
                    Console.Error.WriteLine(line);
                }
            });
        }
    }
}
