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

