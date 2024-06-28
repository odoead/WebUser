using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using WebUser.common.extentions;
using WebUser.shared;
using WebUser.Swagger.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureCORS();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfiureSwagger();
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.ConfigurResponseCaching();
builder.Services.AddCacheProfile();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureServiceWrapper();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

//builder.Services.ConfigureRepositoryWrapper();
builder.Services.AddControllers();
var app = builder.Build();
ServiceExtentions.ConfigureExceptionHandler(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop v1"));
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
