using Serilog.Context;
using System.Diagnostics;

public class TraceIdEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public TraceIdEnricherMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.Value!.Contains("openapi") | !context.Request.Path.Value!.Contains("scalar"))
        {
            var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
            LogContext.PushProperty("TraceId", traceId);
        }
        await _next(context);
    }
}
