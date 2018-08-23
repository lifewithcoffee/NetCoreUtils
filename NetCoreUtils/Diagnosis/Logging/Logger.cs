using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Diagnosis.Logging
{
    public class Logger : IDisposable
    {
        private object Locker = new object();
        private bool hasInitialized = false;
        const string timeFormat = "y-MM-dd HH:mm:ss.fff";

        private Tracker _tracker = new Tracker();

        private void Init_Internal(bool addConsoleTraceListener)
        {
            // output to visual studio output window
            _tracker.Listeners.Add(new VsOutputListener());

            // output to console
            if (addConsoleTraceListener)
                _tracker.Listeners.Add(new TerminalOutputListener());

            // output to file
            string logFileDir = Path.Combine(Directory.GetCurrentDirectory(), "xunitlogs");
            string logFileName = $"xunitlog-{DateTime.Now.ToString("yyyy-MM-dd")}.log";
            _tracker.Listeners.Add(new TextFileOutputListener(logFileDir, logFileName));

            hasInitialized = true;
            this.WriteInfo($"Logger initialized, current log file: {logFileDir}\\{logFileName}");
        }

        /// <summary>
        /// Initialize the log file and timer
        /// </summary>
        public Logger(bool addConsoleTraceListener = false)
        {
            lock(Locker) {
                if (!hasInitialized)
                {
                    Init_Internal(addConsoleTraceListener);
                    
                    var timer = new System.Timers.Timer(86400000); // 24 hours
                    timer.Elapsed += (s, e) => {
                        this.WriteInfo(string.Format("Log initialization timer event triggered. Interval: ", timer.Interval));
                        this.Init_Internal(addConsoleTraceListener);
                    };
                    timer.Enabled = true;
                }
            }
        }

        public void WriteException(Exception ex, string additionalMsg = "")
        {
            lock (Locker)
            {
                WriteError(ex.Message + "; " + additionalMsg);

                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    _tracker.WriteLine("InnerException: " + innerException.Message);
                    innerException = innerException.InnerException;
                }

                string message_with_delimiters = $"||{ex.StackTrace.Replace("\n", "\n||")}";
                _tracker.WriteLine(message_with_delimiters);
            }
        }

        public void WriteError(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    _tracker.WriteLine($"{DateTime.Now.ToString(timeFormat)}|Error  |{message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        public void WriteWarning(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    _tracker.WriteLine($"{DateTime.Now.ToString(timeFormat)}|Warning|{message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        public void WriteWarning(bool condition, string format, params object[] args)
        {
            if (condition == false)
            {
                WriteWarning(format, args);
            }
        }


        public void WriteInfo(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    _tracker.WriteLine($"{DateTime.Now.ToString(timeFormat)}|Info   |{message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        public void WriteInfo(bool enabled, string format, params object[] args)
        {
            if (enabled)
            {
                WriteInfo(format, args);
            }
        }
        
        public void WriteTrace(string format, params object[] args)
        {
#if DEBUG
            lock (Locker)
            {
                string message = string.Format(format, args);
                _tracker.WriteLine($"{DateTime.Now.ToString(timeFormat)}|Trace  |{message}");
            }
#endif
        }

        public void Dispose()
        {
            _tracker.Dispose();
        }
    }
}
