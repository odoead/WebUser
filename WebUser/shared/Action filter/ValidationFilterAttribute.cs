using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebUser.shared
{
    [AttributeUsage(AttributeTargets.All)]
    public class ValidationFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            foreach (var arg in context.ActionArguments)
            {
                var param = arg.Value;

                if (param == null)
                {
                    context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}, parameter: {arg.Key}");
                    return;
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                return;
            }

            await next();
        }
    }
}
