using DAL.IDALs;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_CitasMedicas_Service : IDAL_CitasMedicas_Service
    {
        public CitaMedica getCitaMedicaById(long id)
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.CitasMedicas
                    .Where(p => p.Id == id)
                    .Select(p => new CitaMedica
                    {
                        Id = p.Id,
                        Fecha = p.Fecha,
                        Estado = p.Estado,
                        PacienteId = p.PacienteId,
                        ConsultaMedicaId = p.ConsultaMedicaId,
                        Calendario = new Calendario
                        {
                            HoraInicio = p.Calendario.HoraInicio,
                            HoraFin = p.Calendario.HoraFin,
                            TiempoCita = p.Calendario.TiempoCita,
                            CantidadCitas = p.Calendario.CantidadCitas,
                            DiasSemana = p.Calendario.DiasSemana,
                            Medico = new Medico
                            {
                                Id = p.Calendario.Medico.Id,
                                Nombres = p.Calendario.Medico.Nombres,
                                Apellidos = p.Calendario.Medico.Apellidos,
                                Documento = p.Calendario.Medico.Documento,
                                Email = p.Calendario.Medico.Email,
                                Telefono = p.Calendario.Medico.Telefono
                            },
                            Especialidad = new Especialidad
                            {
                                Id = p.Calendario.Especialidad.Id,
                                Nombre = p.Calendario.Especialidad.Nombre,
                                Descripcion = p.Calendario.Especialidad.Descripcion
                            },
                            Consultorio = new Consultorio
                            {
                                Id = p.Calendario.Consultorio.Id,
                                Numero = p.Calendario.Consultorio.Numero,
                                Piso = p.Calendario.Consultorio.Piso
                            }
                        }
                    })
                    .FirstOrDefault(); // Obtener la primera cita que coincida con el ID
            }
        }

        public void updateCitaMedica(CitaMedica citaActualizada)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = _dbContext.CitasMedicas.FirstOrDefault(p => p.Id == citaActualizada.Id);
                if (citaEntity != null)
                {
                    citaEntity.Fecha = citaActualizada.Fecha;
                    citaEntity.Estado = citaActualizada.Estado;
                    citaEntity.PacienteId = citaActualizada.PacienteId;
                    citaEntity.ConsultaMedicaId = citaActualizada.ConsultaMedicaId;

                    _dbContext.CitasMedicas.Update(citaEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            using (var _dbContext = new DBContext())
            {
                string IdEncriptada = AES.Encrypt(pacienteId.ToString());

                var query = _dbContext.CitasMedicas
                    .Where(c => c.Estado == "Completada" && c.PacienteId == IdEncriptada);
 

                // Aplicar filtro de fechas si ambas fechas están presentes
                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    query = query.Where(c => c.Fecha >= fechaInicio.Value && c.Fecha <= fechaFin.Value);
                }

                if (especialidadesIds.Any())
                {
                    query = query.Where(c => especialidadesIds.Contains(c.Calendario.Especialidad.Id));
                }

                // Aplicar orden
                query = orden.ToLower() == "asc" ? query.OrderBy(c => c.Fecha) : query.OrderByDescending(c => c.Fecha); ;

                return query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CitaMedica
                    {
                        Id = c.Id,
                        Fecha = c.Fecha,
                        Estado = c.Estado,
                        PacienteId = c.PacienteId,
                        Calendario = new Calendario
                        {
                            Medico = new Medico
                            {
                                Id = c.Calendario.Medico.Id,
                                Nombres = c.Calendario.Medico.Nombres,
                                Apellidos = c.Calendario.Medico.Apellidos,
                                Documento = c.Calendario.Medico.Documento,
                                Email = c.Calendario.Medico.Email,
                                Telefono = c.Calendario.Medico.Telefono
                            },
                            Especialidad = new Especialidad
                            {
                                Id = c.Calendario.Especialidad.Id,
                                Nombre = c.Calendario.Especialidad.Nombre,
                                Descripcion = c.Calendario.Especialidad.Descripcion
                            }
                        },
                        ConsultaMedicaId = c.ConsultaMedicaId
                    })
                    .ToList();
            }
        }

        public int CountCitasMedicasByPacienteId(long pacienteId, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            using (var _dbContext = new DBContext())
            {
                string IdEncriptada = AES.Encrypt(pacienteId.ToString());

                var query = _dbContext.CitasMedicas
                    .Where(c => c.Estado == "Completada" && c.PacienteId == IdEncriptada);

                // Aplicar filtro de rango de fechas solo si ambos valores están presentes
                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    query = query.Where(c => c.Fecha >= fechaInicio.Value && c.Fecha <= fechaFin.Value);
                }

                if (especialidadesIds.Any())
                {
                    query = query.Where(c => especialidadesIds.Contains(c.Calendario.Especialidad.Id));
                }

                // Contar los resultados después de aplicar los filtros
                return query.Count();
            }
        }
    }
}
