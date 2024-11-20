using AuthWebApi.Extensions;
using BL.BLs;
using BL.IBLs;
using DAL;
using DAL.DALs;
using DAL.IDALs;
using HistoriaClinicaWebApi.Extensions;

try 
{ 
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddSwaggerExplorer()
                   .InjectDBContext()
                   .InjectDALandBL()
                   .AddIdentityHandlersAndStores()
                   .ConfigureIdentityOptions()
                   .AddIdentityAuth();

    var app = builder.Build();

    app.ConfigureSwaggerExplorer()
       .ConfigureCORS()
       .AddIdentityAuthMiddlewares()
       .UseHttpsRedirection();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
