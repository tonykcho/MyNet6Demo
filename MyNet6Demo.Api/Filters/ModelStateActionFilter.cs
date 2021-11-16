using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyNet6Demo.Api.Filters
{
    public class ModelStateActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<ModelStateActionFilter> _logger;

        public ModelStateActionFilter(ILogger<ModelStateActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid == false)
            {
                ValidationProblemDetails details = new ValidationProblemDetails(context.ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };

                context.Result = new BadRequestObjectResult(details);

                return;
            }

            await next();
        }
    }
}