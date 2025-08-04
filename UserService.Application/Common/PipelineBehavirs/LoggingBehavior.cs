using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Text.Json;

namespace UserService.Application.Common.PipelineBehavirs
{

    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private static readonly ActivitySource ActivitySource = new ActivitySource("UserService");

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var activity = ActivitySource.StartActivity(typeof(TRequest).Name, ActivityKind.Internal);

            LogContext.PushProperty("Request", JsonSerializer.Serialize(request), false);

            if (activity != null)
            {
                var serializedRequest = JsonSerializer.Serialize(request);
                activity.SetTag("request.payload", serializedRequest);
            }

            try
            {
                var response = await next();

                if (activity != null)
                {
                    activity.SetStatus(ActivityStatusCode.Ok);
                }

                return response;
            }
            catch (Exception ex)
            {
                if (activity != null)
                {
                    activity.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity.AddException(ex);
                }

                var serializedRequest = JsonSerializer.Serialize(request);
                _logger.LogError(ex, "Error handling {RequestName} with payload {Payload}", typeof(TRequest).Name, serializedRequest);

                throw;
            }
        }
    }
}

