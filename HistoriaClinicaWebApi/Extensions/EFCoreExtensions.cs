using BL.BLs;
using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace HistoriaClinicaWebApi.Extensions
{
    public static class EFCoreExtensions
    {
        //public static IServiceCollection InjectDBContext(this IServiceCollection services)
        //{
        //    services.AddDbContext<DBContext>(options =>
        //        options.UseSqlServer(GlobalFunctions.GetConnectionString()));
        //    return services;
        //}

        public static IServiceCollection InjectDALandBL(this IServiceCollection services)
        {
            //  DALs
            services.AddTransient<IDAL_HistoriasClinicas, DAL_HistoriasClinicas_EF>();

            //  BLs
            services.AddTransient<IBL_HistoriasClinicas, BL_HistoriasClinicas>();

            return services;
        }
    }
}
