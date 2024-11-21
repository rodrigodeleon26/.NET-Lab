using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "Notificaciones", durable: false, exclusive: false, autoDelete: false, arguments: null);
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    Task.Delay(3000).Wait();
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($" [x] Received {message}");

    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
    //return Task.CompletedTask;
};

await channel.BasicConsumeAsync("Notificaciones", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();