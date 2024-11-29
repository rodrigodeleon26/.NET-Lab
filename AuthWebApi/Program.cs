using DAL;
using AuthWebApi.Extensions;
using AuthWebApi.Controllers;
using DAL.Models;
using RabbitMQ.Client;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // RabbitMQ
    var factory = new ConnectionFactory { HostName = "rabbitmq" };
    var connection = await factory.CreateConnectionAsync();
    var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(queue: "Notificaciones", durable: false, exclusive: false, autoDelete: false, arguments: null);

    builder.Services.AddSingleton<IChannel>(channel);
    builder.Services.AddSingleton<IConnection>(connection);

    // Add services to the container.
    builder.Services.AddControllers();

    builder.Services.AddSwaggerExplorer()
                    .InjectDBContext()
                    .InjectDALandBL()
                    .AddEmailand2FAService()
                    .AddIdentityHandlersAndStores()
                    .ConfigureIdentityOptions()
                    .AddIdentityAuth();

    var app = builder.Build();

    app.ConfigureSwaggerExplorer()
       .ConfigureCORS()
       .AddIdentityAuthMiddlewares();
       //.UseHttpsRedirection();

    app.MapControllers();
    // Endpoints nativos de Identity
    //app.MapIdentityApi<AppUsers>();
    app.MapAuthEndpoints();
    app.MapPruebaEndpoints();


    // Este comentario se borra
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}