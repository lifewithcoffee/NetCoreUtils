using Microsoft.Extensions.Logging;
using NetCoreUtils.Diagnosis.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.Logging
{
    public class NetCoreUtilsLogger : ILogger
    {
        Logger logger;

        public NetCoreUtilsLogger(LoggerConfig config = null)
        {
            logger = new Logger(config);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    logger.WriteError(message);
                    break;
                case LogLevel.Warning:
                    logger.WriteWarning(message);
                    break;
                case LogLevel.Information:
                    logger.WriteInfo(message);
                    break;
            }
        }
    }

    public class NetCoreUtilsLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, NetCoreUtilsLogger> _loggers = new ConcurrentDictionary<string, NetCoreUtilsLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd("logger", name => new NetCoreUtilsLogger());
        }

        public void Dispose() { }
    }

    static public class NetCoreUtilsExtension
    {
        static public ILoggerFactory AddNetCoreUtilsLogger(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new NetCoreUtilsLoggerProvider());
            return loggerFactory;
        }
    }
}
