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
		Paciente getPacienteById(int id);
		void addPaciente(Paciente paciente);
		void updatePaciente(Paciente paciente);
		void deletePaciente(int id);

		// Seguros Medicos
		List<SeguroMedico> getSegurosMedicos();
		SeguroMedico getSeguroMedicoById(int id);
		void addSeguroMedico(SeguroMedico seguroMedico);
		void updateSeguroMedico(SeguroMedico seguroMedico);
		void deleteSeguroMedico(int id);

		// Contratos
		List<Contrato> getContratos();
		Contrato getContratoById(int id);
		void addContrato(Contrato contrato);
		void updateContrato(Contrato contrato);
		void deleteContrato(int id);

		// Precios
		List<Precio> getPrecios();
		Precio getPrecioById(int id);
		void addPrecio(Precio precio);
		void updatePrecio(Precio precio);
		void deletePrecio(int id);

		// Copagos
		List<Copago> getCopagos();
		Copago getCopagoById(int id);
		void addCopago(Copago copago);
		void updateCopago(Copago copago);
		void deleteCopago(int id);

		// Facturas
		List<Factura> getFacturas();
		Factura getFacturaById(int id);
		void addFactura(Factura factura);
		void updateFactura(Factura factura);
		void deleteFactura(int id);
	}
}
