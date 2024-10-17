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
                        Estado = p.Estado
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
                    Estado = citaEntity.Estado
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
                    Estado = nuevaCita.Estado
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
