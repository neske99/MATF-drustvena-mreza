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
        private readonly IFileLogger _fileLogger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger, IOptions<LoggerOptions> options, IFileLogger fileLogger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new LoggerOptions();
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
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
            // Console/standard logging
            _logger.LogError("[!] {Method} {Path} - {ErrorMessage} ({ElapsedMs}ms)", 
                context.Request.Method, context.Request.Path.Value, 
                exception.Message, elapsedMs);

            // File logging for critical errors (if enabled)
            if (_options.EnableFileLogging)
            {
                _fileLogger.LogException(
                    context.Request.Method, 
                    context.Request.Path.Value ?? "", 
                    exception, 
                    elapsedMs);
            }
        }
    }
}