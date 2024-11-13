using AdministrativoWebApi;
using AuthWebApi.Extensions;
using BL.BLs;
using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services.AddSwaggerExplorer()
                    .InjectDBContext()
                    .InjectDALandBL()
                    .AddEmailService()
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