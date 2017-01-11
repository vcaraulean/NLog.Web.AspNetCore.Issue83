using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Internal;
using Microsoft.Extensions.Logging;

namespace WebApplication2
{
    public class CustomLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomLogger(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var user = _httpContextAccessor.HttpContext?.User?.Identity.Name;
            var message = formatter(state, exception);
            Debug.WriteLine($"================= {user ?? "<n/a>"}: {message}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Disposable(() => { });
        }
    }

    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomLoggerProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(_httpContextAccessor);
        }
    }
}
