using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using DAL.IDALs;
using DAL.Models;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X500;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_Administrativo_Service : IDAL_Administrativo
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DAL_Administrativo_Service(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

        }

        public Paciente GetPacienteById(long id)
        {
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


                string url = $"https://administrativowebapi:8081/api/Pacientes/{id}";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var paciente = JsonConvert.DeserializeObject<Paciente>(json);
                    return paciente;
                }
                else
                {
                    Console.WriteLine($"Error al obtener paciente: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                return null;
            }
        }

        public Paciente GetPacienteByDNI(string dni)
        {
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

                string url = $"https://administrativowebapi:8081/api/Pacientes/dni/{dni}";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var paciente = JsonConvert.DeserializeObject<Paciente>(json);   
                    return paciente;
                }
                else
                {
                    Console.WriteLine($"Error al obtener paciente: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                return null;
            }
        }

        public List<Paciente> GetPacientes()
        {
            throw new NotImplementedException();
        }

        public void AddPaciente(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public void UpdatePaciente(Paciente paciente)
        {
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
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                string url = $"https://administrativowebapi:8081/api/Pacientes/{paciente.Id}";

                var content = new StringContent(JsonConvert.SerializeObject(paciente), Encoding.UTF8, "application/json");

                var response = _httpClient.PutAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    paciente = JsonConvert.DeserializeObject<Paciente>(json);
                    //return paciente;
                }
                else
                {
                    Console.WriteLine($"Error al obtener paciente: {response.StatusCode} - {response.ReasonPhrase}");
                    //return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener paciente: {ex.Message}");
                //return null;
            }
        }

        public void DeletePaciente(long id)
        {
            throw new NotImplementedException();
        }

        public bool nuevaCedulaOcupada(string nuevaCi, long pacienteId)
        {
            throw new NotImplementedException();
        }

        public List<Notificacion> getNotificaciones(long id, int pageNumber, int pageSize)
        {
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

                var url = $"https://administrativowebapi:8081/api/Pacientes/{id}/notificaciones";

                var queryParams = new List<string>
                {
                    $"pageNumber={pageNumber}",
                    $"pageSize={pageSize}"
                };

                var queryString = string.Join("&", queryParams);
                var fullUrl = $"{url}?{queryString}";

                var response = _httpClient.GetAsync(fullUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Notificacion>>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener las notificaciones");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las notificaciones: {ex.Message}");
                return null;
            }
        }

        public int CountNotificaciones(long id)
        {
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
                    return 0;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = $"https://administrativowebapi:8081/api/Pacientes/{id}/notificaciones/count";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener las notificaciones");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las notificaciones: {ex.Message}");
                return 0;
            }
        }

        public List<SeguroMedico> GetSegurosMedicos()
        {
            throw new NotImplementedException();
        }

        public SeguroMedico GetSeguroMedicoById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddSeguroMedico(SeguroMedico seguroMedico)
        {
            throw new NotImplementedException();
        }

        public void UpdateSeguroMedico(SeguroMedico seguroMedico)
        {
            throw new NotImplementedException();
        }

        public void DeleteSeguroMedico(long id)
        {
            throw new NotImplementedException();
        }

        public List<Contrato> GetContratos()
        {
            throw new NotImplementedException();
        }

        public Contrato GetContratoById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddContrato(Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public void UpdateContrato(Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public void DeleteContrato(long id)
        {
            throw new NotImplementedException();
        }

        public List<Precio> GetPrecios()
        {
            throw new NotImplementedException();
        }

        public Precio GetPrecioById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddPrecio(Precio precio)
        {
            throw new NotImplementedException();
        }

        public void UpdatePrecio(Precio precio)
        {
            throw new NotImplementedException();
        }

        public void DeletePrecio(long id)
        {
            throw new NotImplementedException();
        }

        public List<Copago> GetCopagos()
        {
            throw new NotImplementedException();
        }

        public Copago GetCopagoById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddCopago(Copago copago)
        {
            throw new NotImplementedException();
        }

        public void UpdateCopago(Copago copago)
        {
            throw new NotImplementedException();
        }

        public void DeleteCopago(long id)
        {
            throw new NotImplementedException();
        }

        public long getIdByFilds(Copago copago)
        {
            throw new NotImplementedException();
        }

        public List<Factura> GetFacturas()
        {
            throw new NotImplementedException();
        }

        public Factura GetFacturaById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddFactura(Factura factura)
        {
            throw new NotImplementedException();
        }

        public void UpdateFactura(Factura factura)
        {
            throw new NotImplementedException();
        }

        public void DeleteFactura(long id)
        {
            throw new NotImplementedException();
        }

        public List<Medico> GetMedicos()
        {
            throw new NotImplementedException();
        }

        public Medico GetMedicoById(long id)
        {
            throw new NotImplementedException();
        }

        public Medico GetMedicoByDocumento(string ci)
        {
            throw new NotImplementedException();
        }

        public void AddMedico(Medico medico)
        {
            throw new NotImplementedException();
        }

        public void UpdateMedico(Medico medico)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedico(long id)
        {
            throw new NotImplementedException();
        }

        public List<Medico> GetMedicosPaginadosYFiltrados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public List<CitaMedica> GetCitasMedicas()
        {
            throw new NotImplementedException();
        }

        public CitaMedica GetCitasMedicasById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddCitasMedicas(CitaMedica citaMedica)
        {
            throw new NotImplementedException();
        }

        public void UpdateCitasMedicas(CitaMedica citaMedica)
        {
            throw new NotImplementedException();
        }

        public void DeleteCitasMedicas(long id)
        {
            throw new NotImplementedException();
        }

        public List<Calendario> GetCalendarios()
        {
            throw new NotImplementedException();
        }

        public Calendario GetCalendarioById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddCalendario(Calendario calendario)
        {
            throw new NotImplementedException();
        }

        public void UpdateCalendario(Calendario calendario)
        {
            throw new NotImplementedException();
        }

        public void DeleteCalendario(long id)
        {
            throw new NotImplementedException();
        }

        public List<Consultorio> GetConsultorios()
        {
            throw new NotImplementedException();
        }

        public Consultorio GetConsultorioById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddConsultorio(Consultorio consultorio)
        {
            throw new NotImplementedException();
        }

        public void UpdateConsultorio(Consultorio consultorio)
        {
            throw new NotImplementedException();
        }

        public void DeleteConsultorio(long id)
        {
            throw new NotImplementedException();
        }

        public List<Especialidad> GetEspecialidades()
        {
            throw new NotImplementedException();
        }

        public Especialidad GetEspecialidadById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddEspecialidad(Especialidad especialidad)
        {
            throw new NotImplementedException();
        }

        public void UpdateEspecialidad(Especialidad especialidad)
        {
            throw new NotImplementedException();
        }

        public void DeleteEspecialidad(long id)
        {
            throw new NotImplementedException();
        }

        public List<Articulo> GetArticulos()
        {
            throw new NotImplementedException();
        }

        public Articulo GetArticuloById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddArticulo(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public void UpdateArticulo(Articulo articulo)
        {
            throw new NotImplementedException();
        }

        public void DeleteArticulo(long id)
        {
            throw new NotImplementedException();
        }

        public List<Articulo> GetArticulosFiltrados(string filtro)
        {
            throw new NotImplementedException();
        }

        public List<Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public bool cedulaDuplicada(string cedula)
        {
            throw new NotImplementedException();
        }

        public bool emailDuplicado(string email)
        {
            throw new NotImplementedException();
        }

        public List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro)
        {
            throw new NotImplementedException();
        }

        public Precio GetPrecioBySeguro(long id)
        {
            throw new NotImplementedException();
        }

        public List<Factura> ObtenerUltimasFacturasDelContrato(long contratoId, int cantidad)
        {
            throw new NotImplementedException();
        }

        public bool ExisteFacturaParaPacienteEnMes(long pacienteId, int mes, int año)
        {
            throw new NotImplementedException();
        }

        public List<Factura> GetFacturasPaginadas(int numPagina, string? pacienteString, bool fechaAsc, bool? estaPago)
        {
            throw new NotImplementedException();
        }

        public List<Factura> getHistorialFacturacion(long pacienteId, int pageNumber, int pageSize)
        {
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

                var url = $"https://administrativowebapi:8081/api/Pacientes/{pacienteId}/historialFacturacion";

                var queryParams = new List<string>
                {
                    $"pageNumber={pageNumber}",
                    $"pageSize={pageSize}"
                };

                var queryString = string.Join("&", queryParams);
                var fullUrl = $"{url}?{queryString}";

                var response = _httpClient.GetAsync(fullUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Factura>>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener las facturas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las facturas: {ex.Message}");
                return null;
            }
        }

        public int countFacturas(long id)
        {
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
                    return 0;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = $"https://administrativowebapi:8081/api/Pacientes/{id}/historialFacturacion/count";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(responseData);
                }
                else
                {
                    throw new Exception("Error al obtener las facturas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las facturas: {ex.Message}");
                return 0;
            }
        }

        public IEnumerable<Contrato> GetContratosActivos()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public List<Calendario> GetCalendariosFiltrados(long medicoId, string filtroEspecialidad, string filtroDia, string filtroHoraInicio)
        {
            throw new NotImplementedException();
        }

        public List<PagoPayPal> GetPaypalPagos()
        {
            throw new NotImplementedException();
        }

        public PagoPayPal GetPaypalPagoById(long id)
        {
            throw new NotImplementedException();
        }

        public void AddPaypalPago(PagoPayPal nuevoPago)
        {
            throw new NotImplementedException();
        }
    }
}
