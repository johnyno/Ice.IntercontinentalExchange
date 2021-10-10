using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using IntercontinentalExchange.Domain.Contracts;
using IntercontinentalExchange.Domain.Model;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace IntercontinentalExchange.Infrastructure.Services
{
    internal class AppLogger : IAppLogger
    {
        public Action<string> TitleCallBack { get; set; } = (log) => { };
        public Action<string> DebugCallBack { get; set; } = (log) => { };
        public Action<string> InfoCallBack { get; set; } = (log) => { };
        public Action<string> SuccessCallBack { get; set; } = (log) => { };
        public Action<string> WarningCallBack { get; set; } = (log) => { };
        public Action<string, Exception> ErrorCallBack { get; set; } = (log, ex) => { };
        public Action<string> ValidationErrorCallBack { get; set; } = (log) => { };


        public AppLogger(IHttpContextAccessor httpContextAccessor)
        {
            var method = httpContextAccessor.HttpContext?.Request?.Method ?? string.Empty;
            var path = httpContextAccessor.HttpContext?.Request?.Path ?? string.Empty;
            var requestPrefix = method + "_" + path.Value;
            requestPrefix = requestPrefix.Trim() == "_" ? "log" : requestPrefix.Replace("/", "_");

            var logId = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "") + string.Format("{0:HH:mm:ss}", DateTime.Now).Replace(":", "");
            logId = logId + "_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 3);
            var logfile = $"{Paths.ApplicationLogDirectory}\\{requestPrefix}_{logId}.json";

            var frameworkSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", frameworkSwitch)
                .WriteTo.File(formatter: new JsonFormatter(), path: logfile, restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            // Some time later, when Information events are required:
            //frameworkSwitch.MinimumLevel = LogEventLevel.Information;
        }


        public void Error(string message, Exception exception = null, [CallerMemberName] string callerName = "")
        {
            Log.Error(message);
            if (ErrorCallBack != null) ErrorCallBack(message, exception);
        }

        public void ValidationError(string message)
        {
            Log.Error(message);
            if (ValidationErrorCallBack != null) ValidationErrorCallBack(message);
        }

        public void Debug(string message, [CallerMemberName] string callerName = "")
        {
            var logMessage = $"{BuildPrefix(callerName)} => {message}";
            Log.Debug(logMessage);

            if (DebugCallBack != null) DebugCallBack(message);
        }

        public void Info(string message, [CallerMemberName] string callerName = "")
        {
            var logMessage = $"{BuildPrefix(callerName)} => {message}";
            Log.Information(logMessage);
            if (InfoCallBack != null) InfoCallBack(message);
        }

        public void Success(string message, [CallerMemberName] string callerName = "")
        {
            var logMessage = $"{BuildPrefix(callerName)} => {message}";
            Log.Information(logMessage);
            if (SuccessCallBack != null) SuccessCallBack(message);
        }

        public void Title(string message, [CallerMemberName] string callerName = "")
        {
            Log.Information(message);
            if (TitleCallBack != null) TitleCallBack(message);
        }

        public void Warning(string message, Exception exception = null, [CallerMemberName] string callerName = "")
        {
            var logMessage = $"{BuildPrefix(callerName)} => {message}";
            Log.Warning(logMessage, exception);
            if (WarningCallBack != null) WarningCallBack(message);
        }

        private static string BuildPrefix(string callerName)
        {
            string fullName;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            var caller = string.IsNullOrWhiteSpace(callerName) ? string.Empty : $".{callerName}";
            return $"[{DateTime.Now.ToString("h:mm:ss tt")}] [{fullName.Replace("DeveloperLabTool.", "")}]{caller}";
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}
