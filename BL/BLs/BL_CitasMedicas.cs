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
        public CitaMedica getCitaMedicaById(long id)
        {
            return dal.getCitaMedicaById(id);
        }

        // Crear una nueva cita médica
        public CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId)
        {
            return dal.createCitaMedica(nuevaCita, calendarioId, pacienteId);
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

        public List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            return dal.GetCitasMedicasByPacienteId(pacienteId, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);
        }

        public int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            return dal.CountCitasMedicasByPacienteId(pacienteId, fechaInicio, fechaFin, orden, especialidadesIds);
        }
    }
}