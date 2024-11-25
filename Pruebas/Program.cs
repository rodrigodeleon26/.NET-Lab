using Shared;
using Shared.Services;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var emailService = new EmailService();

            // IDs originales de los pacientes
            var pacientesIds = new List<string> { "1", "2", "3", "4", "1" };

            // Generar IDs encriptados
            var encriptados = new List<string>();
            foreach (var id in pacientesIds)
            {
                var encrypted = AES.Encrypt(id);
                encriptados.Add(encrypted);
            }

            // Mostrar los valores encriptados
            Console.WriteLine("IDs encriptados:");
            for (int i = 0; i < encriptados.Count; i++)
            {
                Console.WriteLine($"PacienteId {i + 1}: {encriptados[i]}");
            }

            var email = "rd6209965@gmail.com";
            var subject = "Test Subject";
            var htmlMessage = "<h1>Test Message</h1>";

            try
            {
                await emailService.SendEmailAsync(email, subject, htmlMessage);
                Console.WriteLine("Email enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el email: {ex.Message}");
            }
        }
    }
}

