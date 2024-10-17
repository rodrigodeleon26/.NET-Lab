using BL.IBLs;
using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLs
{
	public class BL_Administrativo : IBL_Administrativo
	{
		private readonly IDAL_Administrativo dal;

		public BL_Administrativo(IDAL_Administrativo dal)
		{
			this.dal = dal;
		}

		//Pacientes 
		#region PACIENTES

		public void addPaciente(Paciente paciente)
		{
			dal.AddPaciente(paciente);
		}

		public void deletePaciente(int id)
		{
			dal.DeletePaciente(id);
		}

		public Paciente getPacienteById(int id)
		{
			return dal.GetPacienteById(id);
		}

		public List<Paciente> getPacientes()
		{
			return dal.GetPacientes();
		}

		public void updatePaciente(Paciente paciente)
		{
			dal.UpdatePaciente(paciente);
		}

		#endregion

		//Seguros Medicos
		#region SEGUROS MEDICOS

		public void addSeguroMedico(SeguroMedico seguroMedico)
		{
			dal.AddSeguroMedico(seguroMedico);
		}

		public void deleteSeguroMedico(int id)
		{
			dal.DeleteSeguroMedico(id);
		}

		public SeguroMedico getSeguroMedicoById(int id)
		{
			return dal.GetSeguroMedicoById(id);
		}

		public List<SeguroMedico> getSegurosMedicos()
		{
			return dal.GetSegurosMedicos();
		}

		public void updateSeguroMedico(SeguroMedico seguroMedico)
		{
			dal.UpdateSeguroMedico(seguroMedico);
		}

		#endregion

		//Contratos
		#region CONTRATOS

		public List<Contrato> getContratos()
		{
			return dal.GetContratos();
		}

		public Contrato getContratoById(int id)
		{
			return dal.GetContratoById(id);
		}

		public void addContrato(Contrato contrato)
		{
			dal.AddContrato(contrato);
		}

		public void updateContrato(Contrato contrato)
		{
			dal.UpdateContrato(contrato);
		}

		public void deleteContrato(int id)
		{
			dal.DeleteContrato(id);
		}

		#endregion

		//Precios
		#region PRECIOS


		public List<Precio> getPrecios()
		{
			return dal.GetPrecios();
		}

		public Precio getPrecioById(int id)
		{
			return dal.GetPrecioById(id);
		}

		public void addPrecio(Precio precio)
		{
			dal.AddPrecio(precio);
		}

		public void updatePrecio(Precio precio)
		{
			dal.UpdatePrecio(precio);
		}

		public void deletePrecio(int id)
		{
			dal.DeletePrecio(id);
		}

		#endregion

		//Copagos
		#region COPAGOS

		public List<Copago> getCopagos()
		{
			return dal.GetCopagos();
		}

		public Copago getCopagoById(int id)
		{
			return dal.GetCopagoById(id);
		}

		public void addCopago(Copago copago)
		{
			dal.AddCopago(copago);
		}

		public void updateCopago(Copago copago)
		{
			dal.UpdateCopago(copago);
		}

		public void deleteCopago(int id)
		{
			dal.DeleteCopago(id);
		}

        #endregion

        //Facturas
        #region FACTURAS

        public List<Factura> getFacturas()
        {
			return dal.GetFacturas();
        }

        public Factura getFacturaById(int id)
        {
			return dal.GetFacturaById(id);
        }

        public void addFactura(Factura factura)
        {
			dal.AddFactura(factura);
        }

        public void updateFactura(Factura factura)
        {
			dal.UpdateFactura(factura);
        }

        public void deleteFactura(int id)
        {
			dal.DeleteFactura(id);
        }

        #endregion
    }
}
