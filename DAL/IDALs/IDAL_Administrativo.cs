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
		public void AddPaciente(Paciente paciente);
		public void UpdatePaciente(Paciente paciente);
		public void DeletePaciente(long id);

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

		// Precios
		public List<Precio> GetPrecios();
		public Precio GetPrecioById(long id);
		public void AddPrecio(Precio precio);
		public void UpdatePrecio(Precio precio);
		public void DeletePrecio(long id);

		// Copagos
		public List<Copago> GetCopagos();
		public Copago GetCopagoById(long id);
		public void AddCopago(Copago copago);
		public void UpdateCopago(Copago copago);
		public void DeleteCopago(long id);

		// Facturas
		public List<Factura> GetFacturas();
		public Factura GetFacturaById(long id);
		public void AddFactura(Factura factura);
		public void UpdateFactura(Factura factura);
		public void DeleteFactura(long id);

		// Medicos
		public List<Medico> GetMedicos();
		public Medico GetMedicoById(long id);
		public void AddMedico(Medico medico);
		public void UpdateMedico(Medico medico);
		public void DeleteMedico(long id);

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
	}
}
