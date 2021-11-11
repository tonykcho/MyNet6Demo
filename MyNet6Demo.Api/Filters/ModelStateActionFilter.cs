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
                IDictionary<string, ICollection<string>> message = new Dictionary<string, ICollection<string>>();

                foreach (var item in context.ModelState)
                {
                    message.Add(item.Key, item.Value.Errors.Select(error => error.ErrorMessage).ToList());
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.HttpContext.Response.WriteAsJsonAsync(message);

                return;
            }

            await next();
        }
    }
}