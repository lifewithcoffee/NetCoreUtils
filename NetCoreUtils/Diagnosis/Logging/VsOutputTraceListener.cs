using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    class VsOutputTraceListener : IOutput
    {
        public void Dispose() { }

        public void Write(string message)
        {
            Debug.Write(message);
        }

        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
