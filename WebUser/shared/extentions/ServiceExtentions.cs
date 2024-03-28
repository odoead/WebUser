
using LoggerService;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.shared.extentions;
using WebUser.shared.Logger;
using WebUser.shared.RepoWrapper;
namespace WebUser.common.extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            }
            );
        }
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(_ => { });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureSqlConnection(this IServiceCollection services, IConfiguration configuration) {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DB_Context>(options => options.UseSqlServer(connectionString));
        }
        public static void ConfigureServiceWrapper(this IServiceCollection services)
            => services.AddScoped<IRepoWrapper, RepoWrapper>(); 

        public static void ConfigureExceptionHandler(this WebApplication application)
        {
            var logger= application.Services.GetRequiredService<ILoggerManager>();
            application.ConfigureExceptionHandle(logger);
            if (application.Environment.IsProduction())
                application.UseHsts();
        }
    }
}
