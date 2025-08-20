using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Common.Logger
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;
        private readonly LoggerOptions _options;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger, IOptions<LoggerOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new LoggerOptions();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip logging for excluded paths
            if (_options.ExcludedPaths.Any(path => 
                context.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            // Log simple request
            LogRequest(context);

            try
            {
                await _next(context);
                stopwatch.Stop();

                // Log simple response
                LogResponse(context, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogException(context, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;
            _logger.LogInformation("--> {Method} {Path}", 
                request.Method, request.Path.Value);
        }

        private void LogResponse(HttpContext context, long elapsedMs)
        {
            var response = context.Response;
            var indicator = response.StatusCode >= 400 ? "[ERR]" : "[OK]";
            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            
            _logger.Log(logLevel, "<-- {Indicator} {StatusCode} ({ElapsedMs}ms)", 
                indicator, response.StatusCode, elapsedMs);
        }

        private void LogException(HttpContext context, long elapsedMs, Exception exception)
        {
            _logger.LogError("[!] {Method} {Path} - {ErrorMessage} ({ElapsedMs}ms)", 
                context.Request.Method, context.Request.Path.Value, 
                exception.Message, elapsedMs);
        }
    }
}