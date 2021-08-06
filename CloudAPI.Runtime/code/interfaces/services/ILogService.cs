using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface ILogService
    {
        LogLevel LogLevel {get; set;}
        LogProvider LogProvider {get; set;}
        string LogDetail {get; set;}
        bool AddConsoleLog {get; set;}

        void LogMessage(LogLevel logLevel, string message);
        void LogMessage(LogLevel logLevel, string source, string message);

    }
}
