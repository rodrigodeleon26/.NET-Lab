using DAL;
using HistoriaClinicaWebApi.Extensions;
using HistoriaClinicaWebApi.Controllers;
using DAL.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    //DBContext.UpdateDatabase();

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
       //.UseHttpsRedirection();

    app.MapControllers();
    // Endpoints nativos de Identity
    //app.MapIdentityApi<AppUsers>();


    // Este comentario se borra
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
