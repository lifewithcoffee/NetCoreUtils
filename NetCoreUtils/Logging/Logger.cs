using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.Logging
{
    class ConsoleTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Logger
    {
        static private object Locker = new object();
        static private bool hasInitialized = false;
        const string timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        static public void Init_Internal(bool addConsoleTraceListener)
        {
            System.Diagnostics.Trace.AutoFlush = true;

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new System.Diagnostics.DefaultTraceListener()); // system default
            if (addConsoleTraceListener)
                Trace.Listeners.Add(new ConsoleTraceListener());

            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            //string logFile = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
            string logFile = string.Format("{0}sclog-{1}{2}", logFilePath, DateTime.Now.ToString("yyyy-MM-dd"), ".log");
            Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(logFile));

            hasInitialized = true;

            Logger.WriteInfo(string.Format("Logger initialized, current log file: {0}", logFile));
        }

        /// <summary>
        /// Initialize the log file and timer
        /// </summary>
        static public void Init(bool addConsoleTraceListener = false)
        {
            lock(Locker) {
                if (!hasInitialized)
                {
                    Init_Internal(addConsoleTraceListener);
                    
                    var timer = new System.Timers.Timer(86400000); // 24 hours
                    timer.Elapsed += (s, e) => {
                        Logger.WriteInfo(string.Format("Log initialization timer event triggered. Interval: ", timer.Interval));
                        Logger.Init_Internal(addConsoleTraceListener);
                    };
                    timer.Enabled = true;
                }
            }
        }

        static public void WriteException(Exception ex, string additionalMsg = "")
        {
            lock (Locker)
            {
                WriteError(ex.Message + " | " + additionalMsg);

                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    Trace.WriteLine("InnerException: " + innerException.Message);
                    innerException = innerException.InnerException;
                }

                Trace.WriteLine(ex.StackTrace);
            }
        }

        static public void WriteError(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine($"{DateTime.Now.ToString(timeFormat)} | Error   | {message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        static public void WriteWarning(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine($"{DateTime.Now.ToString(timeFormat)} | Warning | {message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        static public void WriteWarning(bool condition, string format, params object[] args)
        {
            if (condition == false)
            {
                WriteWarning(format, args);
            }
        }


        static public void WriteInfo(string format, params object[] args)
        {
            lock (Locker)
            {
                try
                {
                    string message = string.Format(format, args);
                    Trace.WriteLine($"{DateTime.Now.ToString(timeFormat)} | Info    | {message}");
                }
                catch (Exception ex)
                {
                    WriteException(ex);
                }
            }
        }

        static public void WriteInfo(bool enabled, string format, params object[] args)
        {
            if (enabled)
            {
                WriteInfo(format, args);
            }
        }
        
        static public void WriteTrace(string format, params object[] args)
        {
#if DEBUG
            lock (Locker)
            {
                string message = string.Format(format, args);
                Trace.WriteLine($"{DateTime.Now.ToString(timeFormat)} | Trace   | {message}");
            }
#endif
        }
    }
}
