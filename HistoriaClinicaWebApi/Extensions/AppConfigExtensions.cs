using Shared;

namespace HistoriaClinicaWebApi.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication ConfigureCORS(this WebApplication app)
        {
            app.UseCors(options =>
                options.WithOrigins(GlobalFunctions.GetAllowedOrigins())
                       .AllowAnyHeader()
                       .AllowAnyMethod());
            return app;
        }
    }
}
