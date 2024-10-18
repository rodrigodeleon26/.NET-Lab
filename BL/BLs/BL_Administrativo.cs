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

		public void deletePaciente(long id)
		{
			dal.DeletePaciente(id);
		}

		public Paciente getPacienteById(long id)
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

		public void ContratarSeguroMedico(long idPaciente, long idSeguroMedico)
        {
            var paciente = getPacienteById(idPaciente);
			var seguroMedico = getSeguroMedicoById(idSeguroMedico);
			if (paciente != null && seguroMedico != null)
			{
				Contrato contrato = new Contrato()
                {
                    Paciente = paciente,
                    SeguroMedico = seguroMedico,
                    FechaInicio = DateTime.Now,
					Activo = false,
                };
				addContrato(contrato);

				paciente.Contrato = contrato;
				updatePaciente(paciente);

				seguroMedico.Contratos.Add(contrato);
				updateSeguroMedico(seguroMedico);
			}
        }

		#endregion

		//Seguros Medicos
		#region SEGUROS MEDICOS

		public void addSeguroMedico(SeguroMedico seguroMedico)
		{
			dal.AddSeguroMedico(seguroMedico);
		}

		public void deleteSeguroMedico(long id)
		{
			dal.DeleteSeguroMedico(id);
		}

		public SeguroMedico getSeguroMedicoById(long id)
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

		public Contrato getContratoById(long id)
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

		public void deleteContrato(long id)
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

		public Precio getPrecioById(long id)
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

		public void deletePrecio(long id)
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

		public Copago getCopagoById(long id)
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

		public void deleteCopago(long id)
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

        public Factura getFacturaById(long id)
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

        public void deleteFactura(long id)
        {
			dal.DeleteFactura(id);
        }

        #endregion
    }
}
