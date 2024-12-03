using DAL;
using DAL.IDALs;
using DAL.DALs;
using BL.IBLs;
using BL.BLs;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;

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
            services.AddScoped<IDAL_Administrativo, DAL_Administrativo_EF>();
            services.AddScoped<IDAL_Pacientes, DAL_Paciente_Service>();
            services.AddHttpClient<IDAL_Pacientes, DAL_Paciente_Service>();
            services.AddTransient<IDAL_HistoriasClinicas, DAL_HistoriasClinicas_Service>();
            services.AddTransient<IDAL_CitasMedicas, DAL_CitasMedicas_Service>();
            services.AddHttpClient<DAL_CitasMedicas_Service>();
            services.AddHttpClient<DAL_HistoriasClinicas_Service>();
            services.AddHttpContextAccessor();

            //  BLs
            services.AddTransient<IBL_Administrativo, BL_Administrativo>();

            // Services
            services.AddScoped<PayPalService>();


            return services;
        }

        public static IServiceCollection AddEmailService(this IServiceCollection services) {
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<EmailService>();
            return services;
        }
    }
}
