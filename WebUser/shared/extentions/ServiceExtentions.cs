using System.Text;
using LoggerService;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebUser.Data;
using WebUser.Data.Seed;
using WebUser.Domain.entities;
using WebUser.shared.extentions;
using WebUser.shared.Logger;
using WebUser.shared.RepoWrapper;
using WebUser.shared.ServiceWrapper;

namespace WebUser.common.extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureCORS(this IServiceCollection services) =>
            services.AddCors(opt => opt.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        public static void ConfigureIISIntegration(this IServiceCollection services) => services.Configure<IISOptions>(_ => { });
        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerManager, LoggerManager>();
        public static void ConfigureSqlConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DB_Context>(options => options.UseSqlServer(connectionString));
        }

        public static void ConfigurResponseCaching(this IServiceCollection services) => services.AddResponseCaching();

        public static void AddCacheProfile(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(
                (expirationOptions) =>
                {
                    expirationOptions.MaxAge = 30;
                    expirationOptions.CacheLocation = CacheLocation.Private;
                },
                (validationOptions) => validationOptions.MustRevalidate = true
            );

        public static void AddCacheConfiguration(IApplicationBuilder app) => app.UseResponseCaching();

        public static void ConfigureServiceWrapper(this IServiceCollection services) => services.AddScoped<IServiceWrapper, ServiceWrapper>();

        public static void ConfigureExceptionHandler(this WebApplication application)
        {
            var logger = application.Services.GetRequiredService<ILoggerManager>();
            application.ConfigureExceptionHandle(logger);
            if (application.Environment.IsProduction())
            {
                application.UseHsts();
            }
        }

        public static async Task UseSeeder(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var context = services.GetRequiredService<DB_Context>();
                await Seeder.Seed(context, userManager, roleManager);
            }
        }

        public static void ConfigureIdentity(this IServiceCollection services) =>
            services
                .AddIdentity<User, IdentityRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequiredLength = 10;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DB_Context>()
                .AddDefaultTokenProviders();

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["JWT:Issuer"],
                        ValidAudience = jwtSettings["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    }
                );
        }
    }
}
