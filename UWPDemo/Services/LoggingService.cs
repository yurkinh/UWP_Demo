using MetroLog;
using System;
using System.Diagnostics;
using UWPDemo.Interfaces;

namespace UWPDemo.Services
{
    public class LoggingService : ILoggingService
    {
        public void WriteLine<T>(string message, LogLevel logLevel = LogLevel.Trace, Exception exception = null)
        {
            var logger = LogManagerFactory.DefaultLogManager.GetLogger<T>();
            if (logLevel == LogLevel.Trace && logger.IsTraceEnabled)
            {
                logger.Trace(message);
            }

            if (logLevel == LogLevel.Debug && logger.IsDebugEnabled)
            {
                Debug.WriteLine($"{DateTime.Now.TimeOfDay}{message}");
                logger.Debug(message);
            }

            if (logLevel == LogLevel.Error && logger.IsErrorEnabled)
            {                
                logger.Error(message);
            }

            if (logLevel == LogLevel.Fatal && logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }

            if (logLevel == LogLevel.Info && logger.IsInfoEnabled)
            {
                logger.Info(message);
            }

            if (logLevel == LogLevel.Warn && logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }
    }
}
