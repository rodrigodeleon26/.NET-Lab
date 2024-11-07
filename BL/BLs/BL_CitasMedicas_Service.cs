using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLs
{
    public class BL_CitasMedicas_Service : IBL_CitasMedicas
    {
        private readonly IDAL_CitasMedicas dal;

        public BL_CitasMedicas_Service(IDAL_CitasMedicas dal)
        {
            this.dal = dal;
        }
        public Calendario CreateCalendario(Calendario nuevoCalendario, long medicoId, long especialidadId)
        {
            throw new NotImplementedException();
        }

        public CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId)
        {
            throw new NotImplementedException();
        }

        public Especialidad CreateEspecialidad(Especialidad nuevaEspecialidad)
        {
            throw new NotImplementedException();
        }

        public Medico CreateMedico(Medico nuevoMedico)
        {
            throw new NotImplementedException();
        }

        public void DeleteCalendario(long medicoId, long especialidadId)
        {
            throw new NotImplementedException();
        }

        public void deleteCitaMedica(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteEspecialidad(long id)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedico(long id)
        {
            throw new NotImplementedException();
        }

        public Calendario GetCalendarioById(long calendarioId)
        {
            throw new NotImplementedException();
        }

        public Calendario GetCalendarioByMedicoEspecialidad(long medicoId, long especialidadId)
        {
            throw new NotImplementedException();
        }

        public List<Calendario> GetCalendarios()
        {
            throw new NotImplementedException();
        }

        public CitaMedica getCitaMedicaById(long id)
        {
            return dal.getCitaMedicaById(id);
        }

        public List<CitaMedica> getCitasMedicas()
        {
            throw new NotImplementedException();
        }

        public List<CitaMedica> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha)
        {
            throw new NotImplementedException();
        }

        public bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public Especialidad GetEspecialidadById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Especialidad> GetEspecialidades()
        {
            throw new NotImplementedException();
        }

        public Medico GetMedicoById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Medico> GetMedicos()
        {
            throw new NotImplementedException();
        }

        public void UpdateCalendario(Calendario calendarioActualizado, long medicoId, long especialidadId)
        {
            throw new NotImplementedException();
        }

        public void updateCitaMedica(CitaMedica citaActualizada)
        {
            dal.updateCitaMedica(citaActualizada);
        }

        public void UpdateEspecialidad(Especialidad especialidadActualizada)
        {
            throw new NotImplementedException();
        }

        public void UpdateMedico(Medico medicoActualizado)
        {
            throw new NotImplementedException();
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
