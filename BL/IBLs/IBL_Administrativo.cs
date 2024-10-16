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
    }
}
