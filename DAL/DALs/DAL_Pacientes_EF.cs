using DAL.IDALs;
using DAL.Models;
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

        public List<Paciente> getPacientes()
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

        public void addPaciente(Paciente paciente)
        {
            using (var _dbContext = new DBContext())
            {
                _dbContext.Pacientes.Add(new Pacientes
                {
                    Nombres = paciente.Nombres,
                    Apellidos = paciente.Apellidos,
                    Documento = paciente.Documento,
                    FechaDeNacimiento = paciente.FechaDeNacimiento,
                    Direccion = paciente.Direccion,
                    Telefono = paciente.Telefono,
                    Email = paciente.Email
                });
                _dbContext.SaveChanges();
            }
        }

        public Paciente getXDocumento(string documento)
        {
            using (var _dbContext = new DBContext())
            {
                var paciente = _dbContext.Pacientes
                    .Where(p => p.Documento == documento)
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
                    }).FirstOrDefault();
                return paciente;
            }
        }

        public Paciente GetPaciente(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var paciente = _dbContext.Pacientes
                    .Where(p => p.Id == id)
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
                    }).FirstOrDefault();
                return paciente;
            }
        }

        public bool notificacionVista(long id)
        {
            using (var _dbContext = new DBContext())
            {
                Notificaciones notificacion = _dbContext.Notificaciones.Find(id);

                if (notificacion == null)
                {
                    return false;
                }
                else
                {
                    notificacion.Visto = true;
                    _dbContext.SaveChanges();
                    return true;
                }
            }
        }
    }
}
