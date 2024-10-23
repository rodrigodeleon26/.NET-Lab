using DAL;
using AuthWebApi.Extensions;
using AuthWebApi.Controllers;

try
{
    var builder = WebApplication.CreateBuilder(args);

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
    app.MapAuthEndpoints();
    app.MapPruebaEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}