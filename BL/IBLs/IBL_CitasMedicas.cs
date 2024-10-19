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

        // MEDICOS
        List<Medico> GetMedicos(); // Obtener todos los médicos
        Medico GetMedicoById(long id); // Obtener un médico por ID
        Medico CreateMedico(Medico nuevoMedico); // Crear un nuevo médico
        void UpdateMedico(Medico medicoActualizado); // Actualizar un médico existente
        void DeleteMedico(long id); // Eliminar un médico por ID

        // ESPECIALIDADES
        List<Especialidad> GetEspecialidades(); // Obtener todas las especialidades
        Especialidad GetEspecialidadById(long id); // Obtener una especialidad por ID
        Especialidad CreateEspecialidad(Especialidad nuevaEspecialidad); // Crear una nueva especialidad
        void UpdateEspecialidad(Especialidad especialidadActualizada); // Actualizar una especialidad existente
        void DeleteEspecialidad(long id); // Eliminar una especialidad por ID


        // CALENDARIOS
        List<Calendario> GetCalendarios(); // Obtener todos los calendarios
        Calendario GetCalendarioById(long medicoId, long especialidadId); // Obtener un calendario por MedicoId y EspecialidadId
        Calendario CreateCalendario(Calendario nuevoCalendario, long medicoId, long especialidadId); // Crear un nuevo calendario, asociándolo a un médico y una especialidad
        void UpdateCalendario(Calendario calendarioActualizado, long medicoId, long especialidadId); // Actualizar un calendario existente
        void DeleteCalendario(long medicoId, long especialidadId); // Eliminar un calendario por MedicoId y EspecialidadId
    }
}
