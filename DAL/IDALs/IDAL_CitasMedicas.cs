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

        // MEDICOS
        List<Medico> GetMedicos();
        Medico GetMedicoById(long id);
        Medico CreateMedico(Medico nuevoMedico);
        void UpdateMedico(Medico medicoActualizado);
        void DeleteMedico(long id);

        // ESPECIALIDADES
        List<Especialidad> GetEspecialidades(); // Obtener todas las especialidades
        Especialidad GetEspecialidadById(long id); // Obtener una especialidad por ID
        Especialidad CreateEspecialidad(Especialidad nuevaEspecialidad); // Crear una nueva especialidad
        void UpdateEspecialidad(Especialidad especialidadActualizada); // Actualizar una especialidad existente
        void DeleteEspecialidad(long id); // Eliminar una especialidad por ID

        // CALENDARIOS
        List<Calendario> GetCalendarios(); // Obtener todos los calendarios
        Calendario GetCalendarioById(long calendarioId); // Obtener un calendario por Id
        Calendario GetCalendarioByMedicoEspecialidad(long medicoId, long especialidadId); // Obtener un calendario por MedicoId y EspecialidadId
        Calendario CreateCalendario(Calendario nuevoCalendario, long medicoId, long especialidadId); // Crear un nuevo calendario, asociándolo a un médico y una especialidad
        void UpdateCalendario(Calendario calendarioActualizado, long medicoId, long especialidadId); // Actualizar un calendario existente
        void DeleteCalendario(long medicoId, long especialidadId); // Eliminar un calendario por MedicoId y EspecialidadId
    }
}