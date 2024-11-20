using DAL.IDALs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public DAL_Administrativo_Service(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

                string url = $"https://administrativowebapi:8081/api/Pacientes/{id}";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var paciente = JsonSerializer.Deserialize<Paciente>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
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

                string url = $"https://administrativowebapi:8081/api/Pacientes/dni/{dni}";

                var response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var paciente = JsonSerializer.Deserialize<Paciente>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
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
            throw new NotImplementedException();
        }

        public void DeletePaciente(long id)
        {
            throw new NotImplementedException();
        }

        public bool nuevaCedulaOcupada(string nuevaCi, long pacienteId)
        {
            throw new NotImplementedException();
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
    }
}
