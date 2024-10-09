using DAL.IDALs;
using Shared;

namespace DAL.DALs
{
    public class DAL_Pacientes_EF : IDAL_Pacientes
    {

        //private DBContext _dbContext;

        //public DAL_Personas_EF(DBContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public List <Paciente> getPacientes()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Pacientes
                    .Select(p => new Paciente
                    {
                        Id = p.Id,
                        Nombres = p.Nombres,
                        Apellidos = p.Apellidos,
                        Documento = p.Documento,
                        FechaDeNacimiento = p.FechaDeNacimiento,
                        Direccion = p.Direccion,
                        Telefono = p.Telefono,
                        Email = p.Email
                    }).ToList();
            }
        }
    }
}
