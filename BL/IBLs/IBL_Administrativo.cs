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
	}
}
