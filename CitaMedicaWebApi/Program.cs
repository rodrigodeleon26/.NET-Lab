using BL.BLs;
using BL.IBLs;
using DAL;
using DAL.DALs;
using DAL.IDALs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CitaMedicaWebApi.Extensions;
using DotNetEnv;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Environment Variables
    Env.Load();

    // Add services to the container.
    DBContext.UpdateDatabase();

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
    // Endpoints nativos de Identity
    //app.MapIdentityApi<AppUsers>();


    // Este comentario se borra
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
