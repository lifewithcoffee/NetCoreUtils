using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    public class TerminalTraceListener : IOutput
    {
        public void Dispose() { }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
