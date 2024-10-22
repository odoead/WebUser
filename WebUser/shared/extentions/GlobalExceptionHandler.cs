using Microsoft.AspNetCore.Diagnostics;
using WebUser.Domain;
using WebUser.Domain.exceptions;
using WebUser.shared.Logger;

namespace WebUser.shared.extentions
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandle(this WebApplication application, ILoggerManager loggerManager) =>
            application.UseExceptionHandler(options =>
                options.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        switch (contextFeature.Error)
                        {
                            case BadRequestException:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync("Error ocurred: " + contextFeature.Error);
                                break;
                            }
                            case NotFoundException:
                            {
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                break;
                            }
                            default:
                            {
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                break;
                            }
                        }
                        loggerManager.LogError($"Error!: {contextFeature.Error}");
                        await context.Response.WriteAsync(
                            new ErrorModel() { StatusCode = context.Response.StatusCode, Message = contextFeature.Error.Message }.ToString()
                        );
                    }
                })
            );
    }
}
