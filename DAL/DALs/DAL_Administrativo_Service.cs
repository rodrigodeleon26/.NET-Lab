using DAL.IDALs;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_Administrativo_Service : IDAL_Administrativo_Service
    {
        private readonly HttpClient _httpClient;

        public DAL_Administrativo_Service(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public Paciente GetPacienteById(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var paciente = _dbContext.Pacientes.Find(id);
                if (paciente != null)
                {
                    var contrato = _dbContext.Contratos
                        .Include(c => c.SeguroMedico)
                        .FirstOrDefault(c => c.PacienteId == paciente.Id);

                    return new Paciente
                    {
                        Id = paciente.Id,
                        Nombres = paciente.Nombres,
                        Apellidos = paciente.Apellidos,
                        Documento = paciente.Documento,
                        FechaDeNacimiento = paciente.FechaDeNacimiento,
                        Direccion = paciente.Direccion,
                        Telefono = paciente.Telefono,
                        Email = paciente.Email,
                        Contrato = contrato != null ? new Contrato
                        {
                            Id = contrato.Id,
                            FechaInicio = contrato.FechaInicio,
                            Activo = contrato.Activo,
                            SeguroMedico = new SeguroMedico
                            {
                                Id = contrato.SeguroMedico.Id,
                                Nombre = contrato.SeguroMedico.Nombre,
                                Descripcion = contrato.SeguroMedico.Descripcion
                            }
                        } : null
                    };
                }
                return null;
            }
        }

        public Paciente GetPacienteByDNI(string dni)
        {
            using (var _dbContext = new DBContext())
            {
                var paciente = _dbContext.Pacientes
                    .Include(p => p.Contrato)
                    .ThenInclude(c => c.SeguroMedico)
                    .FirstOrDefault(p => p.Documento == dni);

                if (paciente != null)
                {
                    return new Paciente
                    {
                        Id = paciente.Id,
                        Nombres = paciente.Nombres,
                        Apellidos = paciente.Apellidos,
                        Documento = paciente.Documento,
                        FechaDeNacimiento = paciente.FechaDeNacimiento,
                        Direccion = paciente.Direccion,
                        Telefono = paciente.Telefono,
                        Email = paciente.Email,
                        Contrato = paciente.Contrato != null ? new Contrato
                        {
                            Id = paciente.Contrato.Id,
                            FechaInicio = paciente.Contrato.FechaInicio,
                            Activo = paciente.Contrato.Activo,
                            SeguroMedico = new SeguroMedico
                            {
                                Id = paciente.Contrato.SeguroMedico.Id,
                                Nombre = paciente.Contrato.SeguroMedico.Nombre,
                                Descripcion = paciente.Contrato.SeguroMedico.Descripcion
                            }
                        } : null
                    };
                }
                return null;
            }
        }

        public async Task<bool> AddNotificacionService(Notificacion notificacion, long idPaciente)
        {
            try
            {
                // Definir la URL del endpoint
                string url = $"http://pacientewebapi:8080/api/Notificaciones/{idPaciente}";

                // Serializar la notificación a JSON
                var json = JsonSerializer.Serialize(notificacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST al endpoint
                var response = await _httpClient.PostAsync(url, content);

                // Verificar si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Manejar errores (opcionalmente, registra el error)
                Console.WriteLine($"Error al agregar notificación: {ex.Message}");
                return false;
            }
        }
    }
}
