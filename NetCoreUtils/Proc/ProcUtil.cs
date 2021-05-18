using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace NetCoreUtils.Proc
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
                    if (line.Split(separator).Last().StartsWith(command))
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
}
