using HCMS.UI.Contracts;
using HCMS.UI.UIServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HCMS.UI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("BackendApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7248/api/");
            });
            return services;
        }

        public static IServiceCollection ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var jwt = context.HttpContext.Session.GetString("JWT");

                        if (!string.IsNullOrEmpty(jwt))
                        {
                            context.Token = jwt;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureUIServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAccountUIService, AccountUIService>();
            services.AddScoped<IEmployeeUIService, EmployeeUIService>();
            services.AddScoped<IDepartmentUIService, DepartmentUIService>();
            return services;
        }
    }
}
