using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;

namespace Celani.TTYD.Randomizer.API.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateOriginFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ValidateOrigin(context.HttpContext.Request.Headers.Origin))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private static bool ValidateOrigin(StringValues origin)
        {
            if (origin.Count == 0)
            {
                return false;
            }

            var originStr = origin[0];
            UriCreationOptions options = default;

            var didCreate = Uri.TryCreate(originStr, in options, out var originUri);
            return didCreate && (originUri.Host == "localhost" || originUri.Host == "gamesfreaksa.info");
        }
    }
}
