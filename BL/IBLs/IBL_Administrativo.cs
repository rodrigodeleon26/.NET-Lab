using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		void activarContrato(long id);

		// Precios
		List<Precio> getPrecios();
		Precio getPrecioById(long id);
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
		Factura getFacturaById(long id);
		void addFactura(Factura factura);
		void updateFactura(Factura factura);
		void deleteFactura(long id);

		// Medicos
		List<Medico> getMedicos();
		Medico getMedicoById(long id);
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
    }
}
