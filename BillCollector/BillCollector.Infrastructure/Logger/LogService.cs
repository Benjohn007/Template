using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.Logger
{

    public class LogService<T> : ILog<T>
    {
        private readonly ILogger _logger;

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information)
        {
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Write(logType, message));
        }

        public void Log(object data, string message = "", Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Write(logType, $"{message} {serializedData}"));
        }

        public void Log(string message, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information, params object[] args)
        {
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Write(logType, message, args));
        }

        public void Log(Exception ex, Serilog.Events.LogEventLevel logType = Serilog.Events.LogEventLevel.Information)
        {
            var serializedException = JsonConvert.SerializeObject(ex);
            LogInternal(logType, log => log.WithClassAndMethodNames<T>().Write(logType, ex, $"EXCEPTION {serializedException}"));
        }

        private void LogInternal(Serilog.Events.LogEventLevel logType, Action<ILogger> logAction)
        {
            logAction(_logger);
        }
    }

    public static class LoggerExtensions
    {
        public static ILogger WithClassAndMethodNames<T>(this ILogger logger, [CallerMemberName] string methodName = "")
        {
            var className = typeof(T).Name;
            return logger.ForContext("ClassName", $"[{className}]::").ForContext("MethodName", $"[{methodName}]");
        }
    }


}
