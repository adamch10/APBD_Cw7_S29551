using System.Diagnostics;

namespace APBD_Cw7_S29551.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            await _next(context); // Przekazanie żądania dalej

            sw.Stop();
            _logger.LogInformation("HTTP {Method} {Path} executed in {ElapsedMilliseconds} ms",
                context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds);
        }
    }
}
