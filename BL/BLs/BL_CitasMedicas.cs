using BL.IBLs;
using DAL.IDALs;
using Shared;
using System.Collections.Generic;

namespace BL.BLs
{
    public class BL_CitasMedicas : IBL_CitasMedicas
    {
        private readonly IDAL_CitasMedicas dal;

        public BL_CitasMedicas(IDAL_CitasMedicas dal)
        {
            this.dal = dal;
        }

        // Obtener todas las citas médicas
        public List<CitaMedica> getCitasMedicas()
        {
            return dal.getCitasMedicas();
        }

        // Obtener una cita médica por ID
        public CitaMedica getCitaMedicaById(int id)
        {
            return dal.getCitaMedicaById(id);
        }

        // Crear una nueva cita médica
        public CitaMedica createCitaMedica(CitaMedica nuevaCita)
        {
            return dal.createCitaMedica(nuevaCita);
        }

        // Actualizar una cita médica existente
        public void updateCitaMedica(CitaMedica citaActualizada)
        {
            dal.updateCitaMedica(citaActualizada);
        }

        // Eliminar una cita médica por ID
        public void deleteCitaMedica(int id)
        {
            dal.deleteCitaMedica(id);
        }

        // MEDICOS
        public List<Medico> GetMedicos()
        {
            return dal.GetMedicos();
        }

        public Medico GetMedicoById(long id)
        {
            return dal.GetMedicoById(id);
        }

        public Medico CreateMedico(Medico nuevoMedico)
        {
            return dal.CreateMedico(nuevoMedico);
        }

        public void UpdateMedico(Medico medicoActualizado)
        {
            dal.UpdateMedico(medicoActualizado);
        }

        public void DeleteMedico(long id)
        {
            dal.DeleteMedico(id);
        }

        // ESPECIALIDADES
        public List<Especialidad> GetEspecialidades()
        {
            return dal.GetEspecialidades();
        }

        public Especialidad GetEspecialidadById(long id)
        {
            return dal.GetEspecialidadById(id);
        }

        public Especialidad CreateEspecialidad(Especialidad nuevaEspecialidad)
        {
            return dal.CreateEspecialidad(nuevaEspecialidad);
        }

        public void UpdateEspecialidad(Especialidad especialidadActualizada)
        {
            dal.UpdateEspecialidad(especialidadActualizada);
        }

        public void DeleteEspecialidad(long id)
        {
            dal.DeleteEspecialidad(id);
        }

        // CALENDARIOS
        public List<Calendario> GetCalendarios()
        {
            return dal.GetCalendarios();
        }

        public Calendario GetCalendarioById(long medicoId, long especialidadId)
        {
            return dal.GetCalendarioById(medicoId, especialidadId);
        }

        public Calendario CreateCalendario(Calendario nuevoCalendario, long medicoId, long especialidadId)
        {
            return dal.CreateCalendario(nuevoCalendario, medicoId, especialidadId);
        }

        public void UpdateCalendario(Calendario calendarioActualizado, long medicoId, long especialidadId)
        {
            dal.UpdateCalendario(calendarioActualizado, medicoId, especialidadId);
        }

        public void DeleteCalendario(long medicoId, long especialidadId)
        {
            dal.DeleteCalendario(medicoId, especialidadId);
        }
    }
}