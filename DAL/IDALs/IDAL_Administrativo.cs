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
        public Paciente GetPacienteById(int id);
        public void AddPaciente(Paciente paciente);
        public void UpdatePaciente(Paciente paciente);
        public void DeletePaciente(int id);

        // Seguros Medicos
        public List<SeguroMedico> GetSegurosMedicos();
        public SeguroMedico GetSeguroMedicoById(int id);
        public void AddSeguroMedico(SeguroMedico seguroMedico);
        public void UpdateSeguroMedico(SeguroMedico seguroMedico);
        public void DeleteSeguroMedico(int id);
    }
}
