using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyNet6Demo.Api.Filters
{
    public class HttpGlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            if (context.Exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is ArgumentNullException)
            {
                IDictionary<string, string> message = new Dictionary<string, string>();

                message.Add("Message", context.Exception.Message);

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.HttpContext.Response.WriteAsJsonAsync(message);

                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is OperationCanceledException)
            {
                context.ExceptionHandled = true;

                return;
            }

            context.ExceptionHandled = false;

            return;
        }
    }
}