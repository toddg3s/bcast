using bcast.common;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using l4n = log4net;

namespace bcast.log4net
{
    public class Logger : ILogger
    {
        private l4n.ILog _log;
        private const string format = "{0}: {1}";
        public l4n.ILog Log
        {
            get { 
                if(_log==null)
                    _log = l4n.LogManager.GetLogger("default");
                return _log;
            }
        }

        public void Initialize()
        {
            l4n.Config.BasicConfigurator.Configure();
        }
        public void Error(string account, string message, Exception ex)
        {
            Log.Error(String.Format(format, account, message), ex);
        }

        public void Warning(string account, string message)
        {
            Log.WarnFormat(format, account, message);
        }

        public void Info(string account, string message)
        {
            Log.InfoFormat(format, account, message);
        }

        public void Debug(string account, string message)
        {
            Log.DebugFormat(format, account, message);
        }

        public bool ShouldLogWarning
        {
            get { return Log.IsWarnEnabled; }
        }

        public bool ShouldLogInfo
        {
            get { return Log.IsInfoEnabled; }
        }

        public bool ShouldLogDebug
        {
            get { return Log.IsDebugEnabled; }
        }

        public void Flush()
        {
            try
            {
                foreach (var appender in l4n.LogManager.GetRepository().GetAppenders().OfType<BufferingAppenderSkeleton>())
                {
                    appender.Flush();
                }
            }
            catch { }
        }
    }
}
