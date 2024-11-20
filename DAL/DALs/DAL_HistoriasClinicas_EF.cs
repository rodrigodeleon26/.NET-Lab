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
                var consultas = context.ConsultasMedicas
                    .Select(c => new ConsultaMedica
                    {
                        Id = c.Id,
                        Descripcion = c.Descripcion,
                        Diagnostico = c.Diagnostico,
                        Estudios = c.Estudios.Select(e => new Estudio
                        {
                            Id = e.Id,
                            Nombre = e.Nombre,
                            Descripcion = e.Descripcion,
                            FechaRealizado = e.FechaRealizado,
                            FechaResultado = e.FechaResultado,
                            ImagenUrl = e.ImagenUrl,
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

                return consultas;
            }
        }


        //Consulta medica por id
        public ConsultaMedica getConsultaMedica(long id)
        {
            using (var context = new DBContext())
            {
                return context.ConsultasMedicas
                .Select(c => new ConsultaMedica
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    Diagnostico = c.Diagnostico,
                    Estudios = c.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = EncryptionHelper.TryDecrypt(e.ImagenUrl),
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

        //public static string GeneratePseudonym()
        //{
        //    return Guid.NewGuid().ToString();
        //}

        //Crear consulta medica
        public ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = new ConsultasMedicas
                {
                    Descripcion = consultaMedica.Descripcion,
                    Diagnostico = consultaMedica.Diagnostico,
                };
                context.ConsultasMedicas.Add(consultaMedicaEF);
                context.SaveChanges();

                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
                    }).ToList()
                };
            }
        }

        //Crear consulta medica sin datos solo CitamMedicaID
        public ConsultaMedica createConsultaMedicaSD(long citeMedicaId)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = new ConsultasMedicas
                {
                    Descripcion = "",
                    Diagnostico = "",
                };
                context.ConsultasMedicas.Add(consultaMedicaEF);
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
                    }).ToList()
                };
            }
        }

        //Actualizar consulta medica
        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica)
        {
            try
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
                    context.SaveChanges();
                    return new ConsultaMedica
                    {
                        Id = consultaMedicaEF.Id,
                        Descripcion = consultaMedicaEF.Descripcion,
                        Diagnostico = consultaMedicaEF.Diagnostico,
                        Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                        {
                            Id = r.Id,
                            NombreMedicamento = r.NombreMedicamento,
                            Cantidad = r.Cantidad,
                            Frecuencia = r.Frecuencia,
                            Vencimiento = r.Vencimiento
                        }).ToList(),
                        Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                        {
                            Id = e.Id,
                            Nombre = e.Nombre,
                            Descripcion = e.Descripcion,
                            FechaRealizado = e.FechaRealizado,
                            FechaResultado = e.FechaResultado,
                            ImagenUrl = e.ImagenUrl,
                        }).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes hacer logging del error o devolver una respuesta más detallada
                throw new Exception("Error al actualizar la consulta médica", ex);
            }
        }

        //Eliminar consulta medica
        public ConsultaMedica deleteConsultaMedica(int id)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Recetas)
                    .Include(c => c.Estudios)
                    .FirstOrDefault(c => c.Id == id);

                if (consultaMedicaEF == null)
                {
                    return null;
                }

                context.ConsultasMedicas.Remove(consultaMedicaEF);
                context.SaveChanges();

                // Mapeo de la entidad eliminada a modelo de negocio
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                .Include(c => c.Estudios)
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
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                    .Include(c => c.Estudios)
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
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                    .Include(c => c.Estudios)
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
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                .Include(c => c.Recetas)
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
                    ImagenUrl = estudio.ImagenUrl,
                    ConsultaMedicaId = idConsultaMedica
                };
                consultaMedicaEF.Estudios.Add(estudioEF);
                context.SaveChanges();

                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                    .Include(c => c.Recetas)
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
                estudioEF.ImagenUrl = estudio.ImagenUrl;
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
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
                    .Include(c => c.Recetas)
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
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = e.ImagenUrl,
                    }).ToList()
                };
            }
        }

        //Agregar resultado a estudio de consulta medica
        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl)
        {
            using (var context = new DBContext())
            {
                var consultaMedicaEF = context.ConsultasMedicas
                    .Include(c => c.Estudios)
                    .Include(c => c.Recetas)
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
                var encryptedImagenUrl = "";
                if (imagenUrl != null || imagenUrl != "")
                {
                    encryptedImagenUrl = EncryptionHelper.Encrypt(imagenUrl);
                }
                estudioEF.FechaResultado = fechaResultado;
                estudioEF.ImagenUrl = encryptedImagenUrl;
                context.SaveChanges();
                return new ConsultaMedica
                {
                    Id = consultaMedicaEF.Id,
                    Descripcion = consultaMedicaEF.Descripcion,
                    Diagnostico = consultaMedicaEF.Diagnostico,
                    Recetas = consultaMedicaEF.Recetas.Select(r => new Receta
                    {
                        Id = r.Id,
                        NombreMedicamento = r.NombreMedicamento,
                        Cantidad = r.Cantidad,
                        Frecuencia = r.Frecuencia,
                        Vencimiento = r.Vencimiento
                    }).ToList(),
                    Estudios = consultaMedicaEF.Estudios.Select(e => new Estudio
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        FechaRealizado = e.FechaRealizado,
                        FechaResultado = e.FechaResultado,
                        ImagenUrl = EncryptionHelper.TryDecrypt(e.ImagenUrl)
                    }).ToList()
                };
            }
        }

        public List<Medicamento> getMedicamentos()
        {
            using (var context = new DBContext())
            {
                return context.Medicamentos
                    .Select(m => new Medicamento
                    {
                        Id = m.Id,
                        Nombre = m.Nombre,
                        Descripcion = m.Descripcion,
                    }).ToList();
            }
        }


    }
}
