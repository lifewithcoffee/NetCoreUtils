using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NetCoreUtils.Shell
{
    public class ShellExecutor
    {
        public (string,string,int) Batch(string cmd)
        {
            return ExecuteTerminalCommand("cmd", "/c", cmd);
        }

        public string BatchPrint(string cmd)
        {
            (string output, string error, int exitCode) = Batch(cmd);
            return $"== command: '{cmd}'\n-- output:\n{output}-- error:\n{error}-- exit code:\n{exitCode}";
        }

        // **NOTE**: this method is not tested
        public (string, string, int) Bash(string cmd)
        {
            return ExecuteTerminalCommand("/bin/bash", "-c", cmd);
        }

        /// <summary>
        /// From:
        ///     Executing Batch File in C# - Stack Overflow 
        ///     https://stackoverflow.com/questions/5519328/executing-batch-file-in-c-sharp
        /// 
        /// Quote:
        ///     It turns out that if the streams are read synchronously, a deadlock can occur either by:
        ///     
        ///     1. reading synchronously before WaitForExit or by 
        ///     2. reading both stderr and stdout synchronously one after the other.
        ///     
        ///     This should not happen if using the asynchronous read methods instead.
        /// 
        /// See also (search "deadlock"):
        ///     ProcessStartInfo.RedirectStandardOutput Property (System.Diagnostics)
        ///     https://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo.redirectstandardoutput%28v=vs.110%29.aspx
        /// </summary>
        /// <param name="shell">"cmd" for Windows; "/bin/bash" for Linux</param>
        /// <param name="shellOption">"/c" for Windows cmd; "-c" for Linux bash</param>
        /// <param name="cmd">The terminal actual command, e.g. "git status"</param>
        /// <returns>A tuple of: string output, string error, int exitCode</returns>
        private (string, string, int) ExecuteTerminalCommand(string shell, string shellOption, string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = $"{shellOption} \"{escapedArgs}\"",

                    CreateNoWindow = true,
                    UseShellExecute = false,

                    /** redirect output **/
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            StringBuilder sb_output = new StringBuilder();
            StringBuilder sb_error = new StringBuilder();

            process.Start();

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => sb_output.AppendLine(e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => sb_error.AppendLine(e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            var exitCode = process.ExitCode;
            process.Close();

            return (sb_output.ToString(), sb_error.ToString(), exitCode);
        }
    }
}
