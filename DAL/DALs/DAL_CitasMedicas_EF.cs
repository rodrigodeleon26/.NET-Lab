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
                    }).ToList();
            }
        }

        public List<CitaMedica> getCitasMedicasPorEspecialidad(string nombreEspecialidad, int numPagina, DateTime? fecha)
        {
            using (var _dbContext = new DBContext())
            {
                var query = _dbContext.CitasMedicas.AsQueryable();

                // Filtra por nombre de especialidad si se proporciona
                if (!string.IsNullOrEmpty(nombreEspecialidad))
                {
                    query = query.Where(p => p.Calendario.Especialidad.Nombre == nombreEspecialidad);
                }

                // Filtra por fecha específica si se proporciona
                if (fecha.HasValue)
                {
                    query = query.Where(p => p.Fecha.Date == fecha.Value.Date);
                }

                // Paginación: salta los registros de páginas anteriores y toma 10 registros
                return query
                    .OrderBy(p => p.Fecha) // Ordena por la hora de la cita
                    .Skip((numPagina - 1) * 10)
                    .Take(10)
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
                    }).ToList();
            }
        }

        public bool HayMasCitasMedicas(string nombreEspecialidad, int numPagina, DateTime fecha)
        {
            using (var _dbContext = new DBContext())
            {
                int skip = (numPagina - 1) * 10;
                return _dbContext.CitasMedicas
                    .Where(p => p.Calendario.Especialidad.Nombre == nombreEspecialidad && p.Fecha.Date == fecha.Date)
                    .Skip(skip)
                    .Any();
            }
        }

        // Obtener una cita médica por ID
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
                        },
                        CopagoId = p.CopagoId
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

                // Verificar si el mismo paciente ya tiene una cita en el mismo calendario y día con estado Completada, NoAsistida o Agendada
                // ((Luego tengo que cambiarlo para que solo revise la ESPECIALIDAD, ya que como está permite registrarse dos veces para por ejemplo
                // el odontologo si son medicos distintos y eso no está bien))
                var citaPacienteExistente = _dbContext.CitasMedicas
                    .Where(c => c.CalendarioId == calendarioId && c.Fecha.Date == nuevaCita.Fecha.Date)
                    .Where(c => c.Estado == "Completada" || c.Estado == "NoAsistida" || c.Estado == "Agendada")
                    .AsEnumerable() // Trae los datos a memoria
                    .FirstOrDefault(c => AES.Decrypt(c.PacienteId) == pacienteId.ToString());

                if (citaPacienteExistente != null)
                {
                    throw new Exception("El paciente ya tiene una cita en el mismo calendario y día.");
                }

                string pacienteIdEncriptado = AES.Encrypt(pacienteId.ToString());

                var citaEntity = new CitasMedicas
                {
                    Fecha = nuevaCita.Fecha,
                    Estado = nuevaCita.Estado ?? "Agendada",
                    PacienteId = pacienteIdEncriptado,
                    CalendarioId = calendarioId,
                    CopagoId = nuevaCita.CopagoId
                };

                _dbContext.CitasMedicas.Add(citaEntity);
                _dbContext.SaveChanges();

                nuevaCita.Id = citaEntity.Id; // Asignar el ID generado por la base de datos
                return nuevaCita;
            }
        }

        // Actualizar una cita médica existente
        public void updateCitaMedica(CitaMedicaDTO citaActualizada)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = _dbContext.CitasMedicas.FirstOrDefault(p => p.Id == citaActualizada.Id);
                if (citaEntity != null)
                {
                    citaEntity.Fecha = citaActualizada.Fecha;
                    citaEntity.Estado = citaActualizada.Estado;
                    citaEntity.PacienteId = citaActualizada.PacienteId;
                    if (citaActualizada.ConsultaMedicaId != null)
                    {
                        citaEntity.ConsultaMedicaId = citaActualizada.ConsultaMedicaId;
                    }

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

        //Obtener las citas medicas de un paciente
        public List<CitaMedica> GetCitasMedicasByPacienteId(long pacienteId, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            using (var _dbContext = new DBContext())
            {
                Console.WriteLine("====================================");
                Console.WriteLine("PacienteId: " + pacienteId);
                Console.WriteLine("====================================");
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
                query = orden.ToLower() == "asc" ? query.OrderBy(c => c.Fecha) : query.OrderByDescending(c => c.Fecha);

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
