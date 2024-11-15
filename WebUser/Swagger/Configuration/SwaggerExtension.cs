namespace WebUser.Swagger.Configuration;

using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

public static class SwaggerExtension
{
    public static void ConfiureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerExamples();
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Shop API" });




            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                },
                new List<string>()
            }
        });



            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "WebShop.xml");
            options.IncludeXmlComments(xmlPath);

        });
    }
}
