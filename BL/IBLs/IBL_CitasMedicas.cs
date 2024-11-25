using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBLs
{
    public interface IBL_CitasMedicas
    {
        // Método para obtener todas las citas médicas
        List<CitaMedicaDTO> getCitasMedicas();

        // Metodo citas medicas por especialidad
        List<CitaMedicaDTO> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha);

        bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha);

        // Método para obtener una cita médica por su ID
        CitaMedicaDTO getCitaMedicaById(long id);

        // Método para crear una nueva cita médica
        CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId);

        // Método para actualizar una cita médica existente
        void updateCitaMedica(CitaMedicaDTO citaActualizada);

        // Método para eliminar una cita médica por su ID
        void deleteCitaMedica(int id);

        List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
        int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
        List<CitaMedica> GetCitasMedicasAgendadas(long id);
        bool CancelarCita(string documento, long id);
        Paciente getPacienteByCedula(string cedula);
        Calendario getCalendarioById(long id);
        long getCopagoBySeguroEspecialidadArticulo(Copago copago);
    }
}
