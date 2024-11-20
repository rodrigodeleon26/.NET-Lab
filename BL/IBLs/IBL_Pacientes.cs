using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBLs
{
    public interface IBL_Pacientes
    {
        List <Paciente> getPacientes();

        public void addPaciente(Paciente paciente);

        public Paciente getXDocumento(string documento);

        public Paciente GetPaciente(long id);

        object GetHistoriaClinica(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
    }
}
