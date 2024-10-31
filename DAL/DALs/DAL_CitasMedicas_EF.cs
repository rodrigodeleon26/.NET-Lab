using DAL.IDALs;
using DAL.Models;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace DAL.DALs
{
    public class DAL_CitasMedicas_EF : IDAL_CitasMedicas
    {
        // private readonly DBContext _dbContext;

        // public DAL_CitasMedicas_EF(DBContext dbContext)
        // {
        //     _dbContext = dbContext;
        // }

        // Obtener todas las citas médicas
        public List<CitaMedica> getCitasMedicas()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.CitasMedicas
                    .Select(p => new CitaMedica
                    {
                        Id = p.Id,
                        Fecha = p.Fecha,
                        Estado = p.Estado,
                        PacienteId = p.PacienteId,
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
                            }
                        }
                    }).ToList();
            }
        }

        // Obtener una cita médica por ID
        public CitaMedica getCitaMedicaById(int id)
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
                        // Mapeo del calendario relacionado
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
                            }
                        }
                    })
                    .FirstOrDefault(); // Obtener la primera cita que coincida con el ID
            }
        }

        public CitaMedica createCitaMedica(CitaMedica nuevaCita, long calendarioId, long pacienteId)
        {
            using (var _dbContext = new DBContext())
            {
                // Obtener el calendario existente
                var calendarioExistente = _dbContext.Calendarios.FirstOrDefault(c => c.Id == calendarioId);

                if (calendarioExistente == null)
                {
                    throw new Exception("El calendario no existe.");
                }

                // Verificar si ya existe una cita con la misma combinación de CalendarioId y Fecha
                // ((Tambien tengo que cambiar para que verifique intervalos de hora para que
                // por ejemplo si el calendario usa intervalos de 15 minutos, y hay una agendada a las 10
                // no permita agendar entre las 10 y las 10:15))
                var citaExistente = _dbContext.CitasMedicas
                    .FirstOrDefault(c => c.CalendarioId == calendarioId && c.Fecha == nuevaCita.Fecha);

                if (citaExistente != null)
                {
                    throw new Exception("Ya existe una cita agendada en esa hora para ese dia.");
                }

                // Verificar si el mismo paciente ya tiene una cita en el mismo calendario y día
                // ((Luego tengo que cambiarlo para que solo revise la ESPECIALIDAD, ya que como está permite registrarse dos veces para por ejemplo
                // el odontologo si son medicos distintos y eso no está bien))
                var citaPacienteExistente = _dbContext.CitasMedicas
                    .FirstOrDefault(c => c.PacienteId == pacienteId && c.CalendarioId == calendarioId && c.Fecha.Date == nuevaCita.Fecha.Date);

                if (citaPacienteExistente != null)
                {
                    throw new Exception("El paciente ya tiene una cita en el mismo calendario y día.");
                }

                var citaEntity = new CitasMedicas
                {
                    Fecha = nuevaCita.Fecha,
                    Estado = nuevaCita.Estado ?? "Agendada",
                    PacienteId = pacienteId,
                    CalendarioId = calendarioId
                };

                _dbContext.CitasMedicas.Add(citaEntity);
                _dbContext.SaveChanges();

                nuevaCita.Id = citaEntity.Id; // Asignar el ID generado por la base de datos
                return nuevaCita;
            }
        }

        // Actualizar una cita médica existente
        public void updateCitaMedica(CitaMedica citaActualizada)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = _dbContext.CitasMedicas.FirstOrDefault(p => p.Id == citaActualizada.Id);
                if (citaEntity != null)
                {
                    citaEntity.Fecha = citaActualizada.Fecha;
                    citaEntity.Estado = citaActualizada.Estado;
                    citaEntity.PacienteId= citaActualizada.PacienteId;

                    _dbContext.CitasMedicas.Update(citaEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        // Eliminar una cita médica por ID
        public void deleteCitaMedica(int id)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = _dbContext.CitasMedicas.FirstOrDefault(p => p.Id == id);
                if (citaEntity != null)
                {
                    _dbContext.CitasMedicas.Remove(citaEntity);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
