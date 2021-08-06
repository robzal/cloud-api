using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public class LogService : BaseService, ILogService
    {

        public LogLevel LogLevel {get; set;}
        public LogProvider LogProvider {get; set;}
        public string LogDetail {get; set;}
        public bool AddConsoleLog {get; set;}

        public LogService() 
        {
            this.ServiceType = ServiceType.LOG;
            this.LogProvider = LogProvider.NONE;
            this.LogLevel = LogLevel.INFO;
            this.AddConsoleLog = true;
        }

        public void LogMessage(LogLevel logLevel, string message)
        {
            // make sure we want the message
            if (this.LogLevel <= logLevel)
            {
                // add timestamp to the message
                string logMessage = string.Format("{0} - {1}", DateTime.Now.ToString(), message);
                if (this.AddConsoleLog)
                {
                    Console.WriteLine (logMessage);
                }
                // TODO implement logging providers
                if (this.LogProvider != LogProvider.NONE)
                {

                }

            }
        }
        public void LogMessage(LogLevel logLevel, string source, string message)
        {
            this.LogMessage (logLevel, string.Format("{0} - {1}", source, message));
        }

    }
}