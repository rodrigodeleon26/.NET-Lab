using AuthWebApi.Extensions;
using BL.BLs;
using BL.IBLs;
using DAL;
using DAL.DALs;
using DAL.IDALs;
using PacienteWebApi;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using HistoriaClinicaWebApi.Extensions;
using Shared;

try 
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200", "https://localhost:443")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });


    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddSwaggerExplorer()
                   .InjectDBContext()
                   .InjectDALandBL()
                   .AddIdentityHandlersAndStores()
                   .ConfigureIdentityOptions()
                   .AddIdentityAuth();

    //inyeccion de dependencias
    //Rabbit

    builder.Services.AddHostedService<RabbitNotificacionConsumer>();
    builder.Services.AddHttpClient<RabbitNotificacionConsumer>();

    //fin de rabbit

    var app = builder.Build();

    app.ConfigureSwaggerExplorer()
        .ConfigureCORS()
        .AddIdentityAuthMiddlewares();
        //.UseHttpsRedirection();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
