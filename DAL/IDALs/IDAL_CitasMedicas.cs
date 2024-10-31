using Shared;
using System.Collections.Generic;

namespace DAL.IDALs
{
    public interface IDAL_CitasMedicas
    {
        // Obtener todas las citas médicas
        List<CitaMedica> getCitasMedicas();

        // Obtener una cita médica por ID
        CitaMedica getCitaMedicaById(int id);

        // Crear una nueva cita médica
        CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId);

        // Actualizar una cita médica existente
        void updateCitaMedica(CitaMedica citaActualizada);

        // Eliminar una cita médica por ID
        void deleteCitaMedica(int id);
    }
}