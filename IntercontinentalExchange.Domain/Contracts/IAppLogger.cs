using System;
using System.Runtime.CompilerServices;

namespace IntercontinentalExchange.Domain.Contracts
{
    public interface IAppLogger : IDisposable
    {
        Action<string> TitleCallBack { get; set; }
        Action<string> DebugCallBack { get; set; }
        Action<string> InfoCallBack { get; set; }
        Action<string> SuccessCallBack { get; set; }
        Action<string> WarningCallBack { get; set; }
        Action<string, Exception> ErrorCallBack { get; set; }
        Action<string> ValidationErrorCallBack { get; set; }

        void Title(string message, [CallerMemberName] string callerName = "");
        void Debug(string message, [CallerMemberName] string callerName = "");
        void Info(string message, [CallerMemberName] string callerName = "");
        void Success(string message, [CallerMemberName] string callerName = "");
        void Warning(string message, Exception exception = null, [CallerMemberName] string callerName = "");
        void Error(string message, Exception exception = null, [CallerMemberName] string callerName = "");
        void ValidationError(string message);
    }
}
