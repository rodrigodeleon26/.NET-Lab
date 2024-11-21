using RabbitMQ.Client;
using System.Text;


var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "Notificaciones", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

for(int i = 0;  i < 60; i++)
{
    string message = $"Esta sera una notificacion! {i}";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "Notificaciones", body: body);
    Console.WriteLine($" [x] Sent {message}");
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


