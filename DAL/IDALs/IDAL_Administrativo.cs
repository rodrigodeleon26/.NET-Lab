using Amazon.S3.Model;
using DAL.DALs;
using DAL.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IDALs
{
	public interface IDAL_Administrativo
	{
		// Pacientes
		public List<Paciente> GetPacientes();
		public Paciente GetPacienteById(long id);
		public Paciente GetPacienteByDNI(string dni);
        public void AddPaciente(Paciente paciente);
		public void UpdatePaciente(Paciente paciente);
		public void DeletePaciente(long id);
		public bool nuevaCedulaOcupada(string nuevaCi, long pacienteId);
		public List<Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro);
		public List<Notificacion> getNotificaciones(long id, int pageNumber, int pageSize);
		public int CountNotificaciones(long id);
        public bool cedulaDuplicada(string cedula);
        public bool emailDuplicado(string email);

        // Seguros Medicos
        public List<SeguroMedico> GetSegurosMedicos();
		public SeguroMedico GetSeguroMedicoById(long id);
		public void AddSeguroMedico(SeguroMedico seguroMedico);
		public void UpdateSeguroMedico(SeguroMedico seguroMedico);
		public void DeleteSeguroMedico(long id);

		// Contratos
		public List<Contrato> GetContratos();
		public Contrato GetContratoById(long id);
		public void AddContrato(Contrato contrato);
		public void UpdateContrato(Contrato contrato);
		public void DeleteContrato(long id);
		public List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro);

        // Precios
        public List<Precio> GetPrecios();
		public Precio GetPrecioById(long id);
		public Precio GetPrecioBySeguro(long id);
		public void AddPrecio(Precio precio);
		public void UpdatePrecio(Precio precio);
		public void DeletePrecio(long id);

		// Copagos
		public List<Copago> GetCopagos();
		public Copago GetCopagoById(long id);
		public void AddCopago(Copago copago);
		public void UpdateCopago(Copago copago);
		public void DeleteCopago(long id);
		public long getIdByFilds(Copago copago);

        // Facturas
        public List<Factura> GetFacturas();
		public List<Factura> ObtenerUltimasFacturasDelContrato(long contratoId, int cantidad);

        public bool ExisteFacturaParaPacienteEnMes(long pacienteId, int mes, int año);
        public List<Factura> GetFacturasPaginadas(int numPagina, string? pacienteString, bool fechaAsc, bool? estaPago);
		public Factura GetFacturaById(long id);
		public void AddFactura(Factura factura);
		public void UpdateFactura(Factura factura);
		public void DeleteFactura(long id);
        IEnumerable<Contrato> GetContratosActivos();
        Task SaveChangesAsync();
		public Factura ObtenerFacturaParaPacienteEnMes(long pacienteId, int mes, int año);
		public List<Factura> ObtenerFacturasEnRangoFechas(long pacienteId, DateTime fechaInicio, DateTime fechaFin);
		public List<Factura> ObtenerFacturasNoPagadasParaPaciente(long pacienteId);


        // Medicos
        public List<Medico> GetMedicos();
		public Medico GetMedicoById(long id);
		public Medico GetMedicoByDocumento(string ci);
		public void AddMedico(Medico medico);
		public void UpdateMedico(Medico medico);
		public void DeleteMedico(long id);
		public List<Medico> GetMedicosPaginadosYFiltrados(int numPagina, string filtro);

        // CitasMedicas
        public List<CitaMedica> GetCitasMedicas();
		public CitaMedica GetCitasMedicasById(long id);
		public void AddCitasMedicas(CitaMedica citaMedica);
		public void UpdateCitasMedicas(CitaMedica citaMedica);
		public void DeleteCitasMedicas(long id);

		// Calendarios
		public List<Calendario> GetCalendarios();
		public Calendario GetCalendarioById(long id);
		public void AddCalendario(Calendario calendario);
		public void UpdateCalendario(Calendario calendario);
		public void DeleteCalendario(long id);
        public List<Calendario> GetCalendariosFiltrados(long medicoId, string filtroEspecialidad, string filtroDia, string filtroHoraInicio);

		// Consultorios
		public List<Consultorio> GetConsultorios();
		public Consultorio GetConsultorioById(long id);
		public void AddConsultorio(Consultorio consultorio);
		public void UpdateConsultorio(Consultorio consultorio);
		public void DeleteConsultorio(long id);

		// Especialidades
		public List<Especialidad> GetEspecialidades();
		public Especialidad GetEspecialidadById(long id);
		public void AddEspecialidad(Especialidad especialidad);
		public void UpdateEspecialidad(Especialidad especialidad);
		public void DeleteEspecialidad(long id);

		// Articulo
		public List<Articulo> GetArticulos();
		public Articulo GetArticuloById(long id);
		public void AddArticulo(Articulo articulo);
		public void UpdateArticulo(Articulo articulo);
		public void DeleteArticulo(long id);
		public List<Articulo> GetArticulosFiltrados(string filtro);

		// Pago PayPal
		public List<PagoPayPal> GetPaypalPagos();
        public PagoPayPal GetPaypalPagoById(long id);
        public PagoPayPal GetPaypalPagoByOrdenId(string id);
        public void AddPaypalPago(PagoPayPal nuevoPago);
		public Task<PayPalOrderResponse> GetOrderDetailsAsync(string orderId);
		public Task<PayPalCaptureResponse> CaptureOrderAsync(string orderId);
		public Task<PayPalOrderResponse> CreateOrderAsync(
		List<PayPalPurchaseUnit> purchaseUnits,
		string currency,
		string returnUrl,
		string cancelUrl);
		public Task<string> GetAccessTokenAsync();

    }
}
