using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PacienteWebApi
{
    public class RabbitNotificacionConsumer : BackgroundService
    {
        private IConnection? connection;
        private IChannel? channel;
        private readonly HttpClient _httpClient;

        public RabbitNotificacionConsumer(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq" };

            // Crear conexión y canal de manera asincrónica
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "Notificaciones", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine(" [*] Esperando mensajes en la cola 'Notificaciones'.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Recibido: {mensaje}");

                // Procesar el mensaje
                await ProcesarMensajeAsync(mensaje);

                // Confirmar el mensaje
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync("Notificaciones", autoAck: false, consumer: consumer);

            // Mantener el servicio ejecutándose mientras no se cancele
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task<Task> ProcesarMensajeAsync(string mensaje)
        {
            // Procesamiento de la notificacion

            try
            {
                //accedo al atributo paciente de la notificacion json
                var notificacion = JsonSerializer.Deserialize<Notificacion>(mensaje);
                var pacienteId = notificacion.Paciente.Id;
                // Definir la URL del endpoint
                string url = $"http://pacientewebapi:8080/api/Notificaciones/{pacienteId}";

                // mandar el mensaje que ya estaba en json
                var content = new StringContent(mensaje, Encoding.UTF8, "application/json");
                Console.WriteLine("HACIENDO SOLICITUD");
                // Enviar la solicitud POST al endpoint
                var response = await _httpClient.PostAsync(url, content);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar notificación: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel?.CloseAsync();
            connection?.CloseAsync();
            base.Dispose();
        }
    }
}
