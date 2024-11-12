using DAL;
using DAL.IDALs;
using DAL.DALs;
using BL.IBLs;
using BL.BLs;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;
using AuthWebApi.Services;

namespace AuthWebApi.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDBContext(this IServiceCollection services)
        {
            services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(GlobalFunctions.GetConnectionString()));
            return services;
        }

        public static IServiceCollection InjectDALandBL(this IServiceCollection services)
        {
            //  DALs
            services.AddTransient<IDAL_Pacientes, DAL_Pacientes_EF>();
            services.AddTransient<IDAL_Administrativo, DAL_Administrativo_EF>();
            //  BLs
            services.AddTransient<IBL_Pacientes, BL_Pacientes>();
            services.AddTransient<IBL_Administrativo, BL_Administrativo>();

            return services;
        }

        public static IServiceCollection AddEmailand2FAService(this IServiceCollection services) {
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<EmailService>();
            services.AddScoped<TwoFactorAuthService>();
            return services;
        }
    }
}
