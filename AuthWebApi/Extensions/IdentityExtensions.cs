using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Text;

namespace AuthWebApi.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services
                .AddIdentityApiEndpoints<AppUsers>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DBContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Número máximo de intentos de inicio de sesión fallidos
                options.Lockout.MaxFailedAccessAttempts = 5;
                // Tiempo que la cuenta queda bloqueada después de alcanzar el número máximo de intentos de inicio de sesión fallidos
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                // Requerimientos de la password
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                // Requerimientos del usuario
                options.User.RequireUniqueEmail = true;
            });
            return services;
        }

        //  Auth = Authentication + Authorization
        public static IServiceCollection AddIdentityAuth(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options2 =>
            {
                options2.SaveToken = false;
                options2.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalFunctions.GetSecretKey())),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());

                //  Acá agrego politicas en caso de que precise

            return services;
        }

        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication()
               .UseAuthorization();
            return app;
        }
    }
}
