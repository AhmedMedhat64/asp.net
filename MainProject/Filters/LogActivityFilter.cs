using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace MainProject.Filters
{
    public class LogActivityFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // context.Result = new NotFoundResult(); terminates with 404 response
            _logger.LogInformation($"Execution of {context.ActionDescriptor.DisplayName} on controller {context.Controller} with Arguments {JsonSerializer.Serialize(context.ActionArguments)}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} finished Execution on controller {context.Controller}");
            // context.Result = new NotFoundResult(); terminates with 404 response
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"(Asnyc) Execution of {context.ActionDescriptor.DisplayName} on controller {context.Controller} with Arguments {JsonSerializer.Serialize(context.ActionArguments)}");
            await next();
            _logger.LogInformation($"(Asnyc) Action {context.ActionDescriptor.DisplayName} finished Execution on controller {context.Controller}");

        }
    }
}
