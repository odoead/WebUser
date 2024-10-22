namespace WebUser.shared.Action_filter;

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebUser.shared.RequestForming.features;

[AttributeUsage(AttributeTargets.All)]
public class PagingAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (resultContext.Result is OkObjectResult okResult)
        {
            var resultValue = okResult.Value;
            var pagedListType = resultValue?.GetType();
            if (pagedListType != null && pagedListType.IsGenericType)
            {
                var genericTypeDefinition = pagedListType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(PagedList<>))
                {
                    var pagesStatProperty = pagedListType.GetProperty("PagesStat");
                    var pagesStat = pagesStatProperty?.GetValue(resultValue);

                    if (pagesStat != null)
                    {
                        var metaData = new
                        {
                            TotalCount = pagesStat.GetType().GetProperty("TotalCount")?.GetValue(pagesStat),
                            PageSize = pagesStat.GetType().GetProperty("PageSize")?.GetValue(pagesStat),
                            CurrentPage = pagesStat.GetType().GetProperty("CurrentPage")?.GetValue(pagesStat),
                            PageCount = pagesStat.GetType().GetProperty("PageCount")?.GetValue(pagesStat)
                        };

                        resultContext.HttpContext.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));
                    }
                }
            }
        }
    }
}

