namespace WebUser.Swagger.Configuration;

using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

public static class SwaggerExtension
{
    public static void ConfiureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerExamples();
        //services.AddSwaggerExamplesFromAssemblyOf<swaggerError>();
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Shop API", });
            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "WebShop.xml");
            options.IncludeXmlComments(xmlPath);
        });
        services.AddSwaggerGen(options => options.ExampleFilters());
    }
}
