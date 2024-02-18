using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Celani.TTYD.Randomizer.API.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class WebsocketsOnlyFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
