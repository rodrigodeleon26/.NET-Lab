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
        List<CitaMedica> getCitasMedicas();

        // Método para obtener una cita médica por su ID
        CitaMedica getCitaMedicaById(int id);

        // Método para crear una nueva cita médica
        CitaMedica createCitaMedica(CitaMedica nuevaCita);

        // Método para actualizar una cita médica existente
        void updateCitaMedica(CitaMedica citaActualizada);

        // Método para eliminar una cita médica por su ID
        void deleteCitaMedica(int id);
    }
}
