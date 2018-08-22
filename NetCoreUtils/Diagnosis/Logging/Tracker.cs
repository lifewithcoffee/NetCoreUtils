using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    class Tracker : IDisposable
    {
        public List<IOutput> Listeners { get; } = new List<IOutput>();

        public void Write(string message)
        {
            foreach (var listener in Listeners)
                listener.Write(message);
        }

        public void WriteLine(string message)
        {
            foreach (var listener in Listeners)
                listener.WriteLine(message);
        }

        public void Dispose()
        {
            foreach (var listener in Listeners)
                listener.Dispose();
        }
    }
}
