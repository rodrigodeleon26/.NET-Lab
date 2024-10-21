using Shared;

namespace DAL.IDALs
{
    public interface IDAL_Pacientes
    {
        public List<Paciente> getPacientes();

        public void addPaciente(Paciente paciente);

        public Paciente getXDocumento(string documento);

        public Paciente GetPaciente(long id);
    }
}
