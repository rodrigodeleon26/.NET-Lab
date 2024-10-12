using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            //DBContext.UpdateDatabase();

            try
            {
                    
                Console.WriteLine("Esto se usa para pruebas...");

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
                }
            }
        }
    }
}
