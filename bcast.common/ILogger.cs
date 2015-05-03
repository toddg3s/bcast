using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public interface ILogger
    {
        void Initialize();
        void Error(string account, string message, Exception ex);
        void Warning(string account, string message);
        void Info(string account, string message);
        void Debug(string account, string message);
        bool ShouldLogWarning { get; }
        bool ShouldLogInfo { get; }
        bool ShouldLogDebug { get; }
        void Flush();
    }
}
