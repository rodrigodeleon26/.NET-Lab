using BL.IBLs;
using DAL.IDALs;
using Shared;

namespace BL.BLs
{
    public class BL_Pacientes : IBL_Pacientes
    {
        private readonly IDAL_Pacientes dal;

        public BL_Pacientes(IDAL_Pacientes dal)
        {
            this.dal = dal;
        }

        public List<Paciente> getPacientes()
        {
            return dal.getPacientes();
        }
    }
}
