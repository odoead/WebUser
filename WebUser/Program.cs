using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using WebUser.common.extentions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCORS();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureServiceWrapper();
//builder.Services.ConfigureRepositoryWrapper();
builder.Services.AddControllers();
var app = builder.Build();
ServiceExtentions.ConfigureExceptionHandler(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
