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

        public void addPaciente(Paciente paciente)
        {
            dal.addPaciente(paciente);
        }

        public Paciente getXDocumento(string documento)
        {
            return dal.getXDocumento(documento);
        }

        public Paciente GetPaciente(long id)
        {
            return dal.GetPaciente(id);
        }

        public void AddNotificacion(Notificacion notificacion, long idPaciente)
        {
            dal.AddNotificacion(notificacion, idPaciente);
        }
    }
}
