using DAL.IDALs;
using Shared;

namespace DAL.DALs
{
    public class DAL_CitasMedicas_EF : IDAL_CitasMedicas
    {

        //private DBContext _dbContext;

        //public DAL_Personas_EF(DBContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

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
    }
}
