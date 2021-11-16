using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyNet6Demo.Domain.Exceptions;

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

            if (context.Exception is ResourceNotFoundException)
            {
                ProblemDetails details = new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "The specified resource was not found.",
                    Detail = context.Exception.Message
                };

                context.Result = new NotFoundObjectResult(details);

                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is ResourceAlreadyExistException)
            {
                ProblemDetails details = new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "The specified resource was already exist",
                    Detail = context.Exception.Message
                };

                context.Result = new BadRequestObjectResult(details);

                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is CustomException)
            {
                IDictionary<string, string> message = new Dictionary<string, string>();

                message.Add("Message", context.Exception.Message);

                context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;

                await context.HttpContext.Response.WriteAsJsonAsync(message);

                context.ExceptionHandled = true;

                return;
            }

            // if (context.Exception is UnauthorizedAccessException)
            // {
            //     context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            //     context.ExceptionHandled = true;

            //     return;
            // }

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

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            IDictionary<string, string> messages = new Dictionary<string, string>();

            messages.Add("Message", "Something went wrong!!");

            await context.HttpContext.Response.WriteAsJsonAsync(messages);

            context.ExceptionHandled = true;

            return;
        }
    }
}