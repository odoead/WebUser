using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using WebUser.common.extentions;
using WebUser.shared;
using WebUser.shared.Action_filter;
using WebUser.Swagger.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCORS();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<PagingAttribute>();

builder.Services.ConfigureIISIntegration();
builder.Services.ConfiureSwagger();
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.ConfigurResponseCaching();
builder.Services.AddCacheProfile();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.ConfigureServiceWrapper();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddControllers();
var app = builder.Build();
app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop v1");
        c.EnableDeepLinking();
        c.DefaultModelExpandDepth(0);
    });
}
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseCors("CORSPolicy");
app.UseResponseCaching();

//app.ca
app.UseHttpsRedirection();
app.UseAuthorization();
await app.UseSeeder();
app.MapControllers();
app.Run();
