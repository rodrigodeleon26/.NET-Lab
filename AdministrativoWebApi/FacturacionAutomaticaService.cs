using BL.IBLs;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class FacturacionAutomaticaService : BackgroundService
{
    private readonly IBL_Administrativo _blAdministrativo;

    public FacturacionAutomaticaService(IBL_Administrativo blAdministrativo)
    {
        _blAdministrativo = blAdministrativo;
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
                    // Genera facturas automáticamente
                    await _blAdministrativo.GenerarFacturasAutomaticas();

                    Console.WriteLine("Es 1, facturas generadas, ahora espera 25 dias hasta el siguiente checkeo");

                    await Task.Delay(TimeSpan.FromDays(25), stoppingToken);
                }
                else
                {
                    Console.WriteLine("Todavia no es 1, esperando 24 horas hasta el siguiente checkeo");
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