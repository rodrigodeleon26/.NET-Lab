using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_Paciente_Service : IDAL_Pacientes
    {
        private readonly HttpClient _httpClient;

        public DAL_Paciente_Service(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async void AddNotificacion(Notificacion notificacion, long idPaciente)
        {
            try
            {
                // Definir la URL del endpoint
                string url = $"http://pacientewebapi:8080/api/Notificaciones/{idPaciente}";

                // Serializar la notificación a JSON
                var json = JsonSerializer.Serialize(notificacion);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine("HACIENDO SOLICITUD");
                // Enviar la solicitud POST al endpoint
                var response = await _httpClient.PostAsync(url, content);

            }
            catch (Exception ex)
            {
                // Manejar errores (opcionalmente, registra el error)
                Console.WriteLine($"Error al agregar notificación: {ex.Message}");
            }
        }

        public void addPaciente(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public Paciente GetPaciente(long id)
        {
            throw new NotImplementedException();
        }

        public List<Paciente> getPacientes()
        {
            throw new NotImplementedException();
        }

        public Paciente getXDocumento(string documento)
        {
            throw new NotImplementedException();
        }
    }
}
