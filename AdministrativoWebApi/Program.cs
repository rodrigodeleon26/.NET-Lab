using AdministrativoWebApi;
using AuthWebApi.Extensions;
using BL.BLs;
using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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