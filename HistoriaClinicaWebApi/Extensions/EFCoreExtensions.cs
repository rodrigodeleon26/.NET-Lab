using DAL;
using DAL.IDALs;
using DAL.DALs;
using BL.IBLs;
using BL.BLs;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;

namespace HistoriaClinicaWebApi.Extensions
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
            services.AddTransient<IDAL_HistoriasClinicas, DAL_HistoriasClinicas_EF>();
            services.AddTransient<IDAL_Administrativo_Service, DAL_Administrativo_Service>();
            services.AddTransient<IDAL_CitasMedicas_Service, DAL_CitasMedicas_Service>();

            //  BLs
            services.AddTransient<IBL_HistoriasClinicas, BL_HistoriasClinicas>();

            return services;
        }
    }
}
