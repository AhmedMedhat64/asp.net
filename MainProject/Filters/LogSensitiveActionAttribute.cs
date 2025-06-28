using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MainProject.Filters
{
    // ActionFilterAttribute implements 
    // IActionFilter, IAsyncActionFilter, IResultFilter, IAsyncResultFilter, IOrderedFilter
    // you can use it on a specific controller or method 
    // or even can make it global in the Filter.Add -> program.cs
    public class LogSensitiveActionAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Sen Data Entered !!!!!!!!!!");
        }
    }
}
