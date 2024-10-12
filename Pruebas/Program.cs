using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Practico1
{
    class Program
    {
        static void Main(string[] args)
        {
            DBContext.UpdateDatabase();
            using (var context = new DBContext())
            {
                try
                {
                    // Verificar y crear una especialidad si no existe
                    Especialidades especialidad = context.Especialidades.FirstOrDefault();
                    if (especialidad == null)
                    {
                        especialidad = new Especialidades
                        {
                            Nombre = "Especialidad General",
                            Descripcion = "Descripción de la especialidad general"
                        };
                        context.Especialidades.Add(especialidad);
                        context.SaveChanges();
                        Console.WriteLine("Especialidad creada.");
                    }

                    // Verificar y crear un médico si no existe
                    Medicos medico = context.Medicos.FirstOrDefault();
                    if (medico == null)
                    {
                        medico = new Medicos
                        {
                            Nombres = "Nombre Médico",
                            Apellidos = "Apellido Médico",
                            Documento = "12345678",
                            Email = "medico@example.com",
                            Telefono = "123456789"
                        };
                        context.Medicos.Add(medico);
                        context.SaveChanges();
                        Console.WriteLine("Médico creado.");
                    }

                    // Ejemplo 1: Calendario de lunes a viernes
                    Calendarios calendario1 = new Calendarios
                    {
                        HoraInicio = new TimeSpan(9, 0, 0),
                        HoraFin = new TimeSpan(17, 0, 0),
                        TiempoCita = 45,
                        CantidadCitas = 8,
                        DiasSemana = "Lunes,Viernes",
                        EspecialidadId = especialidad.Id,
                        Especialidad = especialidad,
                        MedicoId = medico.Id,
                        Medico = medico
                    };

                    context.Calendarios.Add(calendario1);
                    context.SaveChanges();
                    Console.WriteLine("Calendario 1 guardado.");

                    // Ejemplo 2: Calendario solo los fines de semana
                    Calendarios calendario2 = new Calendarios
                    {
                        HoraInicio = new TimeSpan(10, 0, 0),
                        HoraFin = new TimeSpan(14, 0, 0),
                        TiempoCita = 60,
                        CantidadCitas = 4,
                        DiasSemana = "Sábado,Domingo",
                        EspecialidadId = especialidad.Id,
                        Especialidad = especialidad,
                        MedicoId = medico.Id,
                        Medico = medico
                    };

                    context.Calendarios.Add(calendario2);
                    context.SaveChanges();
                    Console.WriteLine("Calendario 2 guardado.");

                    // Ejemplo 3: Calendario de martes y jueves
                    Calendarios calendario3 = new Calendarios
                    {
                        HoraInicio = new TimeSpan(8, 0, 0),
                        HoraFin = new TimeSpan(12, 0, 0),
                        TiempoCita = 30,
                        CantidadCitas = 8,
                        DiasSemana = "Martes,Jueves",
                        EspecialidadId = especialidad.Id,
                        Especialidad = especialidad,
                        MedicoId = medico.Id,
                        Medico = medico
                    };

                    context.Calendarios.Add(calendario3);
                    context.SaveChanges();
                    Console.WriteLine("Calendario 3 guardado.");

                    // Imprimir los calendarios guardados
                    ImprimirCalendario(context, calendario1.Id);
                    ImprimirCalendario(context, calendario2.Id);
                    ImprimirCalendario(context, calendario3.Id);
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

        static void ImprimirCalendario(DBContext context, long calendarioId)
        {
            var calendarioObtenido = context.Calendarios
                .Include(c => c.Especialidad)
                .Include(c => c.Medico)
                .FirstOrDefault(c => c.Id == calendarioId);

            if (calendarioObtenido != null)
            {
                // Mostrar el calendario
                Console.WriteLine($"Calendario ID: {calendarioObtenido.Id}");
                Console.WriteLine($"Hora de inicio: {calendarioObtenido.HoraInicio}");
                Console.WriteLine($"Hora de fin: {calendarioObtenido.HoraFin}");
                Console.WriteLine($"Tiempo de cita: {calendarioObtenido.TiempoCita}");
                Console.WriteLine($"Cantidad de citas: {calendarioObtenido.CantidadCitas}");
                Console.WriteLine($"Días de repetición: {calendarioObtenido.DiasSemana}");
                Console.WriteLine($"Especialidad: {calendarioObtenido.Especialidad.Nombre}");
                Console.WriteLine($"Médico: {calendarioObtenido.Medico.Nombres} {calendarioObtenido.Medico.Apellidos}");
            }
            else
            {
                Console.WriteLine("No se encontró el calendario.");
            }
        }
    }
}
