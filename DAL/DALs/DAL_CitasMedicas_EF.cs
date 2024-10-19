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
                        PacienteId = p.PacienteId
                    }).ToList();
            }
        }

        // Obtener una cita médica por ID
        public CitaMedica getCitaMedicaById(int id)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = _dbContext.CitasMedicas.FirstOrDefault(p => p.Id == id);
                if (citaEntity == null) return null;

                return new CitaMedica
                {
                    Id = citaEntity.Id,
                    Fecha = citaEntity.Fecha,
                    Estado = citaEntity.Estado,
                    PacienteId = citaEntity.PacienteId
                };
            }
        }

        // Crear una nueva cita médica
        public CitaMedica createCitaMedica(CitaMedica nuevaCita)
        {
            using (var _dbContext = new DBContext())
            {
                var citaEntity = new CitasMedicas
                {
                    Fecha = nuevaCita.Fecha,
                    Estado = nuevaCita.Estado,
                    PacienteId = nuevaCita.PacienteId
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

        // MEDICOS
        public List<Medico> GetMedicos()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Medicos
                    .Select(m => new Medico
                    {
                        Id = m.Id,
                        Nombres = m.Nombres,
                        Apellidos = m.Apellidos,
                        Documento = m.Documento,
                        Email = m.Email,
                        Telefono = m.Telefono,
                        Calendarios = m.Calendarios.Select(c => new Calendario
                        {
                            HoraInicio = c.HoraInicio,
                            HoraFin = c.HoraFin,
                            TiempoCita = c.TiempoCita,
                            CantidadCitas = c.CantidadCitas,
                            DiasSemana = c.DiasSemana
                        }).ToList() // Convertir a lista de calendarios
                    }).ToList();
            }
        }

        // Obtener un médico por ID
        public Medico GetMedicoById(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var medicoEntity = _dbContext.Medicos.FirstOrDefault(m => m.Id == id);
                if (medicoEntity == null) return null;

                return new Medico
                {
                    Id = medicoEntity.Id,
                    Nombres = medicoEntity.Nombres,
                    Apellidos = medicoEntity.Apellidos,
                    Documento = medicoEntity.Documento,
                    Email = medicoEntity.Email,
                    Telefono = medicoEntity.Telefono,
                    Calendarios = medicoEntity.Calendarios.Select(c => new Calendario
                    {
                        HoraInicio = c.HoraInicio,
                        HoraFin = c.HoraFin,
                        TiempoCita = c.TiempoCita,
                        CantidadCitas = c.CantidadCitas,
                        DiasSemana = c.DiasSemana
                    }).ToList() // Convertir a lista de calendarios
                };
            }
        }

        // Crear un nuevo médico
        public Medico CreateMedico(Medico nuevoMedico)
        {
            using (var _dbContext = new DBContext())
            {
                var medicoEntity = new Medicos
                {
                    Nombres = nuevoMedico.Nombres,
                    Apellidos = nuevoMedico.Apellidos,
                    Documento = nuevoMedico.Documento,
                    Email = nuevoMedico.Email,
                    Telefono = nuevoMedico.Telefono
                };

                _dbContext.Medicos.Add(medicoEntity);
                _dbContext.SaveChanges();

                nuevoMedico.Id = medicoEntity.Id;
                return nuevoMedico;
            }
        }

        // Actualizar un médico existente
        public void UpdateMedico(Medico medicoActualizado)
        {
            using (var _dbContext = new DBContext())
            {
                var medicoEntity = _dbContext.Medicos.FirstOrDefault(m => m.Id == medicoActualizado.Id);
                if (medicoEntity != null)
                {
                    medicoEntity.Nombres = medicoActualizado.Nombres;
                    medicoEntity.Apellidos = medicoActualizado.Apellidos;
                    medicoEntity.Documento = medicoActualizado.Documento;
                    medicoEntity.Email = medicoActualizado.Email;
                    medicoEntity.Telefono = medicoActualizado.Telefono;

                    _dbContext.Medicos.Update(medicoEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        // Eliminar un médico por ID
        public void DeleteMedico(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var medicoEntity = _dbContext.Medicos.FirstOrDefault(m => m.Id == id);
                if (medicoEntity != null)
                {
                    _dbContext.Medicos.Remove(medicoEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        //ESPECIALIDADES
        // Obtener todas las especialidades
        public List<Especialidad> GetEspecialidades()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Especialidades
                    .Select(e => new Especialidad
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion
                    }).ToList();
            }
        }

        // Obtener una especialidad por ID
        public Especialidad GetEspecialidadById(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var especialidadEntity = _dbContext.Especialidades.FirstOrDefault(e => e.Id == id);
                if (especialidadEntity == null) return null;

                return new Especialidad
                {
                    Id = especialidadEntity.Id,
                    Nombre = especialidadEntity.Nombre,
                    Descripcion = especialidadEntity.Descripcion
                };
            }
        }

        // Crear una nueva especialidad
        public Especialidad CreateEspecialidad(Especialidad nuevaEspecialidad)
        {
            using (var _dbContext = new DBContext())
            {
                var especialidadEntity = new Especialidades
                {
                    Nombre = nuevaEspecialidad.Nombre,
                    Descripcion = nuevaEspecialidad.Descripcion
                    // Las listas de Calendarios y Copagos no suelen inicializarse al crear una entidad nueva
                };

                _dbContext.Especialidades.Add(especialidadEntity);
                _dbContext.SaveChanges();

                nuevaEspecialidad.Id = especialidadEntity.Id; // Asignar el ID generado por la base de datos
                return nuevaEspecialidad;
            }
        }

        // Actualizar una especialidad existente
        public void UpdateEspecialidad(Especialidad especialidadActualizada)
        {
            using (var _dbContext = new DBContext())
            {
                var especialidadEntity = _dbContext.Especialidades.FirstOrDefault(e => e.Id == especialidadActualizada.Id);
                if (especialidadEntity != null)
                {
                    especialidadEntity.Nombre = especialidadActualizada.Nombre;
                    especialidadEntity.Descripcion = especialidadActualizada.Descripcion;
                    // Si es necesario, también puedes actualizar las relaciones de Calendarios y Copagos

                    _dbContext.Especialidades.Update(especialidadEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        // Eliminar una especialidad por ID
        public void DeleteEspecialidad(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var especialidadEntity = _dbContext.Especialidades.FirstOrDefault(e => e.Id == id);
                if (especialidadEntity != null)
                {
                    _dbContext.Especialidades.Remove(especialidadEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        // CALENDARIOS
        // Obtener todos los calendarios
        public List<Calendario> GetCalendarios()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Calendarios
                    .Select(c => new Calendario
                    {
                        HoraInicio = c.HoraInicio,
                        HoraFin = c.HoraFin,
                        TiempoCita = c.TiempoCita,
                        CantidadCitas = c.CantidadCitas,
                        DiasSemana = c.DiasSemana,
                        Medico = new Medico
                        {
                            Id = c.Medico.Id,
                            Nombres = c.Medico.Nombres,
                            Apellidos = c.Medico.Apellidos,
                            Documento = c.Medico.Documento,
                            Email = c.Medico.Email,
                            Telefono = c.Medico.Telefono
                        }, // Mapeo del médico con las propiedades adicionales
                        Especialidad = new Especialidad
                        {
                            Id = c.Especialidad.Id,
                            Nombre = c.Especialidad.Nombre,
                            Descripcion = c.Especialidad.Descripcion
                        }, // Mapeo de la especialidad con Descripción
                        CitasMedicas = c.CitasMedicas.Select(cm => new CitaMedica
                        {
                            Id = cm.Id,
                            Fecha = cm.Fecha,
                            Estado = cm.Estado
                        }).ToList() // Mapeo de las citas médicas
                    }).ToList();
            }
        }

        // Obtener un calendario por ID (MedicoId y EspecialidadId)
        public Calendario GetCalendarioById(long medicoId, long especialidadId)
        {
            using (var _dbContext = new DBContext())
            {
                var calendarioEntity = _dbContext.Calendarios
                    .FirstOrDefault(c => c.MedicoId == medicoId && c.EspecialidadId == especialidadId);

                if (calendarioEntity == null) return null;

                return new Calendario
                {
                    HoraInicio = calendarioEntity.HoraInicio,
                    HoraFin = calendarioEntity.HoraFin,
                    TiempoCita = calendarioEntity.TiempoCita,
                    CantidadCitas = calendarioEntity.CantidadCitas,
                    DiasSemana = calendarioEntity.DiasSemana,
                    Medico = new Medico
                    {
                        Id = calendarioEntity.Medico.Id,
                        Nombres = calendarioEntity.Medico.Nombres,
                        Apellidos = calendarioEntity.Medico.Apellidos,
                        Documento = calendarioEntity.Medico.Documento,
                        Email = calendarioEntity.Medico.Email,
                        Telefono = calendarioEntity.Medico.Telefono
                    },
                    Especialidad = new Especialidad
                    {
                        Id = calendarioEntity.Especialidad.Id,
                        Nombre = calendarioEntity.Especialidad.Nombre,
                        Descripcion = calendarioEntity.Especialidad.Descripcion
                    },
                    CitasMedicas = calendarioEntity.CitasMedicas.Select(cm => new CitaMedica
                    {
                        Id = cm.Id,
                        Fecha = cm.Fecha,
                        Estado = cm.Estado
                    }).ToList()
                };
            }
        }

        // Crear un nuevo calendario (necesita ID de Medico y Especialidad)
        public Calendario CreateCalendario(Calendario nuevoCalendario, long medicoId, long especialidadId)
        {
            using (var _dbContext = new DBContext())
            {
                var medico = _dbContext.Medicos.FirstOrDefault(m => m.Id == medicoId);
                var especialidad = _dbContext.Especialidades.FirstOrDefault(e => e.Id == especialidadId);

                if (medico == null || especialidad == null)
                {
                    throw new Exception("Medico o Especialidad no encontrados.");
                }

                var calendarioEntity = new Calendarios
                {
                    HoraInicio = nuevoCalendario.HoraInicio,
                    HoraFin = nuevoCalendario.HoraFin,
                    TiempoCita = nuevoCalendario.TiempoCita,
                    CantidadCitas = nuevoCalendario.CantidadCitas,
                    DiasSemana = nuevoCalendario.DiasSemana,
                    MedicoId = medicoId,
                    EspecialidadId = especialidadId
                };

                _dbContext.Calendarios.Add(calendarioEntity);
                _dbContext.SaveChanges();

                return nuevoCalendario;
            }
        }

        // Actualizar un calendario existente
        public void UpdateCalendario(Calendario calendarioActualizado, long medicoId, long especialidadId)
        {
            using (var _dbContext = new DBContext())
            {
                var calendarioEntity = _dbContext.Calendarios
                    .FirstOrDefault(c => c.MedicoId == medicoId && c.EspecialidadId == especialidadId);

                if (calendarioEntity != null)
                {
                    calendarioEntity.HoraInicio = calendarioActualizado.HoraInicio;
                    calendarioEntity.HoraFin = calendarioActualizado.HoraFin;
                    calendarioEntity.TiempoCita = calendarioActualizado.TiempoCita;
                    calendarioEntity.CantidadCitas = calendarioActualizado.CantidadCitas;
                    calendarioEntity.DiasSemana = calendarioActualizado.DiasSemana;

                    _dbContext.Calendarios.Update(calendarioEntity);
                    _dbContext.SaveChanges();
                }
            }
        }

        // Eliminar un calendario por ID (MedicoId y EspecialidadId)
        public void DeleteCalendario(long medicoId, long especialidadId)
        {
            using (var _dbContext = new DBContext())
            {
                var calendarioEntity = _dbContext.Calendarios
                    .FirstOrDefault(c => c.MedicoId == medicoId && c.EspecialidadId == especialidadId);

                if (calendarioEntity != null)
                {
                    _dbContext.Calendarios.Remove(calendarioEntity);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
