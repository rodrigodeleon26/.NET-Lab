using DAL;
using Microsoft.EntityFrameworkCore;
using Shared;

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
    }
}
