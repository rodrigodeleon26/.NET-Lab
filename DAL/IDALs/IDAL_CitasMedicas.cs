using Shared;
using System.Collections.Generic;

namespace DAL.IDALs
{
    public interface IDAL_CitasMedicas
    {
        // Obtener todas las citas médicas
        List<CitaMedica> getCitasMedicas();

        // Obtener citas medicas por especialidad
        List<CitaMedica> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha);

        bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha);

        // Obtener una cita médica por ID
        CitaMedica getCitaMedicaById(long id);

        // Crear una nueva cita médica
        CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId);

        // Actualizar una cita médica existente
        void updateCitaMedica(CitaMedicaDTO citaActualizada);

        // Eliminar una cita médica por ID
        void deleteCitaMedica(int id);

        List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
        int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
    }
}