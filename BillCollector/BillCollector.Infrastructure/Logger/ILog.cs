using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.Logger
{
    public interface ILog<T>
    {
        void Log(string message, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information);
        void Log(object data, string message = "", Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information);
        void Log(string message, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information, params object[] args);
        void Log(Exception ex, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information);
    }

}
