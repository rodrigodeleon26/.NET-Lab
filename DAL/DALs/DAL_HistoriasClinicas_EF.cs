using DAL.IDALs;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DAL.DALs
{
    public class DAL_HistoriasClinicas_EF : IDAL_HistoriasClinicas
    {

        //private DBContext _dbContext;

        //public DAL_HistoriasClinicas_EF(DBContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //Todas las consultas médicas
        public List<ConsultaMedica> getConsultasMedicas()
        {
            using (var context = new DBContext())
            {
                return context.ConsultasMedicas
                .Select(c => new ConsultaMedica
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    Diagnostico = c.Diagnostico,
                    CitaMedicaId = c.CitaMedicaId,
                    Estudios = c.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList(),
                    Recetas = c.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList()
                }).ToList();
            }
        }

        //Consulta medica por id
        public ConsultaMedica getConsultaMedica(int id)
        {
            using (var context = new DBContext())
            {
                return context.ConsultasMedicas
                .Select(c => new ConsultaMedica
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    Diagnostico = c.Diagnostico,
                    CitaMedicaId = c.CitaMedicaId,
                    Estudios = c.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList(),
                    Recetas = c.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList()
                }).FirstOrDefault(c => c.Id == id);
            }
        }

        //Crear consulta medica
        public ConsultaMedica createConsultaMedica(ConsultaMedica consultaMedica)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = new ConsultasMedicas
                {
                    Descripcion = consultaMedica.Descripcion,
                    Diagnostico = consultaMedica.Diagnostico,
                    CitaMedicaId = consultaMedica.CitaMedicaId,
                    Recetas = consultaMedica.Recetas.Select(r => new Recetas
                    {
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia
                        // No es necesario asignar ConsultaMedicaId aquí, ya que se maneja automáticamente por la relación
                    }).ToList(),
                    Estudios = consultaMedica.Estudios.Select(e => new Estudios
                    {
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado
                        // No es necesario asignar ConsultaMedicaId aquí, ya que se maneja automáticamente por la relación
                    }).ToList()
                };
                context.ConsultasMedicas.Add(consultaMedicaEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Crear consulta medica sin recetas ni estudios
        public ConsultaMedica createConsultaMedicaSimple(ConsultaMedica consultaMedica)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = new ConsultasMedicas
                {
                    Descripcion = consultaMedica.Descripcion,
                    Diagnostico = consultaMedica.Diagnostico,
                    CitaMedicaId = consultaMedica.CitaMedicaId
                };
                context.ConsultasMedicas.Add(consultaMedicaEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId
                };
            }
        }

        //Actualizar consulta medica
        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                .Include(c => c.Recetas)
                .Include(c => c.Estudios)
                .FirstOrDefault(c => c.Id == consultaMedica.Id);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                consultaMedicaEF.Descripcion = consultaMedica.Descripcion;
                consultaMedicaEF.Diagnostico = consultaMedica.Diagnostico;
                consultaMedicaEF.CitaMedicaId = consultaMedica.CitaMedicaId;
                consultaMedicaEF.Recetas = consultaMedica.Recetas.Select(r => new Recetas
                {
                    Id = r.Id,
                    Vencimiento = r.Vencimiento,
                    NombreMedicamento = r.NombreMedicamento,
                    Cantidad = r.Cantidad,
                    Frecuencia = r.Frecuencia,
                    ConsultaMedicaId = r.ConsultaMedicaId
                }).ToList();
                consultaMedicaEF.Estudios = consultaMedica.Estudios.Select(e => new Estudios
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    FechaRealizado = e.FechaRealizado,
                    FechaResultado = e.FechaResultado,
                    Resultado = e.Resultado,
                    ConsultaMedicaId = e.ConsultaMedicaId
                }).ToList();
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                    }).ToList()
                };
            }
        }

        //Agregar receta a consulta medica
        public ConsultaMedica addReceta(int idConsultaMedica, Receta receta)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                .Include(c => c.Recetas)
                .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var recetaEF = new Recetas
                {
                    Vencimiento = receta.Vencimiento,
                    NombreMedicamento = receta.NombreMedicamento,
                    Cantidad = receta.Cantidad,
                    Frecuencia = receta.Frecuencia,
                    ConsultaMedicaId = idConsultaMedica
                };
                consultaMedicaEF.Recetas.Add(recetaEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Actualizar receta de consulta medica
        public ConsultaMedica updateReceta(int idConsultaMedica, Receta receta)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Recetas)
                    .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var recetaEF = consultaMedicaEF.Recetas.FirstOrDefault(r => r.Id == receta.Id);
                if (recetaEF == null)
                {
                    return null;
                }
                recetaEF.Vencimiento = receta.Vencimiento;
                recetaEF.NombreMedicamento = receta.NombreMedicamento;
                recetaEF.Cantidad = receta.Cantidad;
                recetaEF.Frecuencia = receta.Frecuencia;
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Eliminar receta de consulta medica
        public ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Recetas)
                    .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var recetaEF = consultaMedicaEF.Recetas.FirstOrDefault(r => r.Id == idReceta);
                if (recetaEF == null)
                {
                    return null;
                }
                consultaMedicaEF.Recetas.Remove(recetaEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Agregar estudio a consulta medica
        public ConsultaMedica addEstudio(int idConsultaMedica, Estudio estudio)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                .Include(c => c.Estudios)
                .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var estudioEF = new Estudios
                {
                    Nombre = estudio.Nombre,
                    Descripcion = estudio.Descripcion,
                    FechaRealizado = estudio.FechaRealizado,
                    FechaResultado = estudio.FechaResultado,
                    Resultado = estudio.Resultado,
                    ConsultaMedicaId = idConsultaMedica
                };
                consultaMedicaEF.Estudios.Add(estudioEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Actualizar estudio de consulta medica
        public ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Estudios)
                    .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var estudioEF = consultaMedicaEF.Estudios.FirstOrDefault(e => e.Id == estudio.Id);
                if (estudioEF == null)
                {
                    return null;
                }
                estudioEF.Nombre = estudio.Nombre;
                estudioEF.Descripcion = estudio.Descripcion;
                estudioEF.FechaRealizado = estudio.FechaRealizado;
                estudioEF.FechaResultado = estudio.FechaResultado;
                estudioEF.Resultado = estudio.Resultado;
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Eliminar estudio de consulta medica
        public ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Estudios)
                    .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var estudioEF = consultaMedicaEF.Estudios.FirstOrDefault(e => e.Id == idEstudio);
                if (estudioEF == null)
                {
                    return null;
                }
                consultaMedicaEF.Estudios.Remove(estudioEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }

        //Agregar resultado a estudio de consulta medica
        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, string resultado, DateOnly fechaResultado)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Estudios)
                    .FirstOrDefault(c => c.Id == idConsultaMedica);
                if (consultaMedicaEF == null)
                {
                    return null;
                }
                var estudioEF = consultaMedicaEF.Estudios.FirstOrDefault(e => e.Id == idEstudio);
                if (estudioEF == null)
                {
                    return null;
                }
                estudioEF.FechaResultado = fechaResultado;
                estudioEF.Resultado = resultado;
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    CitaMedicaId = consultaMedicaEF.CitaMedicaId,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        Vencimiento = r.Vencimiento,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        ConsultaMedicaId = r.ConsultaMedicaId
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        Resultado = e.Resultado,
                        ConsultaMedicaId = e.ConsultaMedicaId
                    }).ToList()
                };
            }
        }


    }
}
