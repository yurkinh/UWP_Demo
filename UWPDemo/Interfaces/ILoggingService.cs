using MetroLog;
using System;

namespace UWPDemo.Interfaces
{
    public interface ILoggingService
    {
        void WriteLine<T>(string message, LogLevel logLevel = LogLevel.Trace, Exception exception = null);
    }
}
