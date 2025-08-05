using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using WebUser.common.extentions;
using WebUser.features.Cart;
using WebUser.features.Category.Interfaces;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.Swagger.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Log
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Logging and CORS
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureCORS();

// Filters and Attributes
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<PagingAttribute>();

// Authentication, Authorization, and Identity
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddAuthentication();

// Database and Caching
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.ConfigurResponseCaching();
builder.Services.AddCacheProfile();

// Dependency Injection for Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.ConfigureServiceWrapper();

// Swagger and API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfiureSwagger();

// HTTP Context and Additional Configurations
builder.Services.ConfigureIISIntegration();
builder.Services.AddControllers();

// MediatR (CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
// Global Exception Handler
app.ConfigureExceptionHandler();

// Use Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop v1");
        c.EnableDeepLinking();
        c.DefaultModelExpandDepth(0);
    });
}

// Middleware configurations
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//seed db
await app.UseSeeder();
app.MapControllers();
app.Run();
