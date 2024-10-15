using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALs
{
    public class DAL_Administrativo_EF : IDAL_Administrativo
    {
        public List<Paciente> GetPacientes()
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
