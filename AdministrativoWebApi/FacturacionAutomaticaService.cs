using BL.IBLs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class FacturacionAutomaticaService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FacturacionAutomaticaService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                DateTime now = DateTime.Now;

                // Verifica si es el primer día del mes
                if (now.Day == 1)
                {
                    // Crear un alcance para obtener el servicio con ciclo de vida Scoped
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var blAdministrativo = scope.ServiceProvider.GetRequiredService<IBL_Administrativo>();

                        // Genera facturas automáticamente
                        await blAdministrativo.GenerarFacturasAutomaticas();
                    }

                    Console.WriteLine("Es 1, facturas generadas, ahora no vuelve a entrar aca hasta el siguiente 1.");

                    // Espera 25 días antes de volver a verificar
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                else
                {
                    Console.WriteLine("Todavía no es 1, esperando 24 horas hasta el siguiente chequeo de fecha.");

                    // Espera 24 horas antes de la próxima verificación
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (log de errores, por ejemplo)
                Console.WriteLine($"Error en la generación automática de facturas: {ex.Message}");
            }
        }
    }
}
