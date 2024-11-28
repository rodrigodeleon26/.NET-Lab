using DAL.IDALs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_HistoriasClinicas_Service : IDAL_HistoriasClinicas
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DAL_HistoriasClinicas_Service(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public ConsultaMedica addEstudio(int idConsultaMedica, Estudio estudio)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica addReceta(int idConsultaMedica, Receta receta)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica createConsultaMedicaSD(long consultaMedicaId)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica deleteConsultaMedica(int id)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica getConsultaMedica(long id)
        {
            Console.WriteLine("Obteniendo consulta médica...");
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No se encontró el token de autorización.");
                    return null;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string url = $"https://historiaclinicawebapi:8081/api/HistoriasClinicas/{id}";

                var response = _httpClient.GetAsync(url).Result;

                Console.WriteLine($"Respuesta: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var consultaMedica = JsonSerializer.Deserialize<ConsultaMedica>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    Console.WriteLine($"Consulta médica encontrada: {consultaMedica.Id}");
                    return consultaMedica;
                }
                else
                {
                    Console.WriteLine($"Error al obtener la consulta: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener consulta: {ex.Message}");
                return null;
            }
        }

        public List<ConsultaMedica> getConsultasMedicas()
        {
            throw new NotImplementedException();
        }

        public List<Medicamento> getMedicamentos()
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio)
        {
            throw new NotImplementedException();
        }

        public ConsultaMedica updateReceta(int idConsultaMedica, Receta receta)
        {
            throw new NotImplementedException();
        }
    }
}
