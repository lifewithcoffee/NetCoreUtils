using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    interface IOutput : IDisposable
    {
        void Write(string message);
        void WriteLine(string message);
    }
}
