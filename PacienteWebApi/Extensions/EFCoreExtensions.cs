using DAL;
using DAL.IDALs;
using DAL.DALs;
using BL.IBLs;
using BL.BLs;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace PacienteWebApi.Extensions
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
            services.AddTransient<IDAL_HistoriasClinicas, DAL_HistoriasClinicas_Service>();
            services.AddTransient<IDAL_CitasMedicas, DAL_CitasMedicas_Service>();
            services.AddTransient<IDAL_Administrativo, DAL_Administrativo_Service>();
            services.AddHttpClient<DAL_Administrativo_Service>();
            services.AddHttpClient<DAL_CitasMedicas_Service>();
            services.AddHttpClient<DAL_HistoriasClinicas_Service>();
            services.AddHttpContextAccessor();


            //  BLs
            services.AddTransient<IBL_Pacientes, BL_Pacientes>();

            return services;
        }
    }
}