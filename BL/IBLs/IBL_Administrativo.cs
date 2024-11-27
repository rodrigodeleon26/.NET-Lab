using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BL.IBLs
{
	public interface IBL_Administrativo
	{
		// Pacientes
		List<Paciente> getPacientes();
		Paciente getPacienteById(long id);
		Paciente getPacienteByDNI(string dni);
        void addPaciente(Paciente paciente);
		void updatePaciente(Paciente paciente);
		void deletePaciente(long id);
		void ContratarSeguroMedico(long idPaciente, long idSeguroMedico);
		List <Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro);
		public bool emailDuplicado(string email);
		public bool cedulaDuplicada(string cedula);
        List<Notificacion> getNotificaciones(long id, int pageNumber, int pageSize);
		int CountNotificaciones(long id);

        // Seguros Medicos
        List<SeguroMedico> getSegurosMedicos();
		SeguroMedico getSeguroMedicoById(long id);
		void addSeguroMedico(SeguroMedico seguroMedico);
		void updateSeguroMedico(SeguroMedico seguroMedico);
		void deleteSeguroMedico(long id);

		// Contratos
		List<Contrato> getContratos();
		Contrato getContratoById(long id);
		void addContrato(Contrato contrato);
		void updateContrato(Contrato contrato);
		void deleteContrato(long id);
		//void activarContrato(long id);
		List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro);
		void cambiarContrato(Contrato contrato, SeguroMedico seguroMedico);
        bool puedeRenovarContrato(long id);
		List<Factura> ObtenerUltimasFacturasDelContrato(long contratoId, int cantidad);
        float ObtenerDeudaDeContrato(long contratoId);
		void reactivarContrato(long contratoId, int cantidadCuotas, int interes);
		bool contratoEnRefinanciacion(long contratoId);

        // Precios
        List<Precio> getPrecios();
		Precio getPrecioById(long id);
		Precio GetPrecioBySeguro(long id);

        void addPrecio(Precio precio);
		void updatePrecio(Precio precio);
		void deletePrecio(long id);

		// Copagos
		List<Copago> getCopagos();
		Copago getCopagoById(long id);
		void addCopago(Copago copago);
		void updateCopago(Copago copago);
		void deleteCopago(long id);

		// Facturas
		List<Factura> getFacturas();

		List<Factura> getFacturasPaginadas(int numPagina, string? pacienteString, bool fechaAsc, bool? estaPago);
		Factura getFacturaById(long id);
		void addFactura(Factura factura);
		void updateFactura(Factura factura);
		void deleteFactura(long id);
        MemoryStream GenerarFactura(long id);
        MemoryStream GenerarFacturaListada(List<long> ids);

		Task GenerarFacturasAutomaticas();

        // Medicos
        List<Medico> getMedicos();
		Medico getMedicoById(long id);
		Medico getMedicoByDocumento(string ci);
		void addMedico(Medico medico);
		void updateMedico(Medico medico);
		void deleteMedico(long id);
		void asignarEspecialidad(long medId, long espId);
        List<Medico>getMedicosPaginadosYFiltrados(int numPagina, string filtro);

        // Citas Medicas
        List<CitaMedica> getCitasMedicas();
		CitaMedica getCitaMedicaById(long id);
		void addCitaMedica(CitaMedica citaMedica);
		void updateCitaMedica(CitaMedica citaMedica);
		void deleteCitaMedica(long id);

        // Calendarios
        List<Calendario> getCalendarios();
		Calendario getCalendarioById(long id);
		void addCalendario(Calendario calendario);
		void updateCalendario(Calendario calendario);
		void deleteCalendario(long id);
		void crearCalendario(long medId, long espId, long conId, TimeSpan horaInicio, TimeSpan horaFin, int tiempo, int cant, string[] dias);
		bool checkOcupacionConsultorio(Calendario calendario);
		bool validarEspecialidadesParaBorrar(long medicoId, List<Especialidad> especialidades);
        Task borrarCalendariosIncompatiblesAsync(long medicoId, List<Especialidad> especialidades);
        List<Calendario> getCalendariosFiltrados(long medicoId, string filtroEspecialidad, string filtroDia, string filtroHoraInicio);

        // Consultorios
        List<Consultorio> getConsultorios();
		Consultorio getConsultorioById(long id);
		void addConsultorio(Consultorio consultorio);
		void updateConsultorio(Consultorio consultorio);
		void deleteConsultorio(long id);

        // Especialidades
		List<Especialidad> getEspecialidades();
		Especialidad getEspecialidadById(long id);
		void addEspecialidad(Especialidad especialidad);
		void updateEspecialidad(Especialidad especialidad);
		void deleteEspecialidad(long id);

		// Articulos
		List<Articulo> getArticulos();
		Articulo getArticuloById(long id);
		void addArticulo(Articulo articulo);
		void updateArticulo(Articulo articulo);
		void deleteArticulo(long id);
		List<Articulo> getArticulosFiltrados(string filtro);

		// Pago PayPal
		List<PagoPayPal> GetPaypalPagos();
        PagoPayPal GetPaypalPagoById(long id);
        void AddPaypalPago(PagoPayPal nuevoPago);
    }
}
