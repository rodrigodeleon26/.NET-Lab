using DAL.IDALs;
using DAL.Models;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    public class DAL_CitasMedicas_Service : IDAL_CitasMedicas
    {
        public CitaMedica getCitaMedicaById(long id)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/{id}";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<CitaMedica>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener la cita médica");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la cita médica: {ex.Message}");
                return null;
            }
        }

        public void updateCitaMedica(CitaMedicaDTO citaActualizada)
        {
            try
            {                 
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/{citaActualizada.Id}";

                var citaMedicaJson = JsonConvert.SerializeObject(citaActualizada);
                var content = new StringContent(citaMedicaJson, Encoding.UTF8, "application/json");

                var response = _httpClient.PutAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error al actualizar la cita médica");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la cita médica: {ex.Message}");
            }
        }

        public List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                // Construcción de la URL con parámetros de la cadena de consulta (query string)
                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/paciente/{pacienteId}";

                // Construcción de los parámetros de la cadena de consulta
                var queryParams = new List<string>
                {
                    $"pageNumber={pageNumber}",
                    $"pageSize={pageSize}",
                    $"orden={orden}"
                };

                if (fechaInicio.HasValue)
                {
                    queryParams.Add($"fechaInicio={fechaInicio.Value.ToString("yyyy-MM-dd")}");
                }

                if (fechaFin.HasValue)
                {
                    queryParams.Add($"fechaFin={fechaFin.Value.ToString("yyyy-MM-dd")}");
                }

                if (especialidadesIds != null && especialidadesIds.Any())
                {
                    var especialidadesStr = string.Join(",", especialidadesIds);
                    queryParams.Add($"especialidadesIds={especialidadesStr}");
                }

                // Unir los parámetros a la URL
                var queryString = string.Join("&", queryParams);
                var fullUrl = $"{url}?{queryString}";

                Console.WriteLine($"URL de la solicitud: {fullUrl}");

                var response = _httpClient.GetAsync(fullUrl).Result;

                Console.WriteLine($"Respuesta del servidor: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<CitaMedica>>(responseData);
                }
                else
                {
                    // Manejar el error según sea necesario
                    throw new Exception("Error al obtener las citas médicas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                return null;
            }
        }

        public int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                // Construcción de la URL con parámetros de la cadena de consulta (query string)
                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/cant/{pacienteId}";

                // Construcción de los parámetros de la cadena de consulta
                var queryParams = new List<string>
                {
                    $"orden={orden}"
                };

                if (fechaInicio.HasValue)
                {
                    queryParams.Add($"fechaInicio={fechaInicio.Value.ToString("yyyy-MM-dd")}");
                }

                if (fechaFin.HasValue)
                {
                    queryParams.Add($"fechaFin={fechaFin.Value.ToString("yyyy-MM-dd")}");
                }

                if (especialidadesIds != null && especialidadesIds.Any())
                {
                    var especialidadesStr = string.Join(",", especialidadesIds);
                    queryParams.Add($"especialidadesIds={especialidadesStr}");
                }

                // Unir los parámetros a la URL
                var queryString = string.Join("&", queryParams);
                var fullUrl = $"{url}?{queryString}";

                Console.WriteLine($"URL de la solicitud: {fullUrl}");

                var response = _httpClient.GetAsync(fullUrl).Result;

                Console.WriteLine($"Respuesta del servidor: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(responseData);
                }
                else
                {
                    // Manejar el error según sea necesario
                    throw new Exception("Error al obtener las citas médicas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                return 0;
            }
        }

        public List<CitaMedica> getCitasMedicas()
        {
            throw new NotImplementedException();
        }

        public List<CitaMedica> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha)
        {
            throw new NotImplementedException();
        }

        public bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId)
        {
            throw new NotImplementedException();
        }

        public void deleteCitaMedica(int id)
        {
            throw new NotImplementedException();
        }

        public List<CitaMedica> GetCitasMedicasAgendadasDelPaciente(long id)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                // Construcción de la URL con parámetros de la cadena de consulta (query string)
                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/paciente/{id}/misCitas";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<CitaMedica>>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener las citas médicas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                return null;
            }
        }

        public bool CancelarCita(string dni, long id)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var _httpClient = new HttpClient(handler);

                var url = $"https://citasmedicaswebapi:8081/api/CitasMedicas/{id}/paciente/{dni}";

                var response = _httpClient.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode) {
                    Console.WriteLine("Cita cancelada con éxito");
                    return true;
                }
                else
                {
                    throw new Exception("Error al cancelar la cita médica");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cancelar la cita médica: {ex.Message}");
                return false;
            }
        }
    }
}
