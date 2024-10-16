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
    }
}
