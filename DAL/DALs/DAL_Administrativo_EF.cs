using DAL.IDALs;
using DAL.Models;
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
		/**********************************************************/
		/**                  PACIENTES                           **/
		/**********************************************************/
		#region FUNCTIONES PACIENTES

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

		public void AddPaciente(Paciente paciente)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoPaciente = new Pacientes
				{
					Nombres = paciente.Nombres,
					Apellidos = paciente.Apellidos,
					Documento = paciente.Documento,
					FechaDeNacimiento = paciente.FechaDeNacimiento,
					Direccion = paciente.Direccion,
					Telefono = paciente.Telefono,
					Email = paciente.Email
				};
				_dbContext.Pacientes.Add(nuevoPaciente);
				_dbContext.SaveChanges();
			}
		}

		public void DeletePaciente(int id)
		{
			using (var _dbContext = new DBContext())
			{
				var paciente = _dbContext.Pacientes.Find(id);
				if (paciente != null)
				{
					_dbContext.Pacientes.Remove(paciente);
					_dbContext.SaveChanges();
				}
			}
		}

		public Paciente GetPacienteById(int id)
		{
			using (var _dbContext = new DBContext())
			{
				var paciente = _dbContext.Pacientes.Find(id);
				if (paciente != null)
				{
					return new Paciente
					{
						Id = paciente.Id,
						Nombres = paciente.Nombres,
						Apellidos = paciente.Apellidos,
						Documento = paciente.Documento,
						FechaDeNacimiento = paciente.FechaDeNacimiento,
						Direccion = paciente.Direccion,
						Telefono = paciente.Telefono,
						Email = paciente.Email
					};
				}
				return null;
			}
		}

		public void UpdatePaciente(Paciente paciente)
		{
			using (var _dbContext = new DBContext())
			{
				var existingPaciente = _dbContext.Pacientes.Find(paciente.Id);
				if (existingPaciente != null)
				{
					existingPaciente.Nombres = paciente.Nombres;
					existingPaciente.Apellidos = paciente.Apellidos;
					existingPaciente.Documento = paciente.Documento;
					existingPaciente.FechaDeNacimiento = paciente.FechaDeNacimiento;
					existingPaciente.Direccion = paciente.Direccion;
					existingPaciente.Telefono = paciente.Telefono;
					existingPaciente.Email = paciente.Email;

					_dbContext.SaveChanges();
				}
			}
		}

        #endregion


        /**********************************************************/
        /**                    Seguros                           **/
        /**********************************************************/

        #region FUNCTIONES SEGUROS

        public List<SeguroMedico> GetSegurosMedicos()
        {
			using (var _dbContext = new DBContext())
			{
				return _dbContext.SegurosMedicos
					.Select(s => new SeguroMedico
					{
						Id = s.Id,
						Nombre = s.Nombre,
						Descripcion = s.Descripcion,
						Contratos = s.Contratos.Select(c => new Contrato
						{
							Id = c.Id,
							FechaInicio = c.FechaInicio,
							Activo = c.Activo
						}).ToList(),
						Precios = s.Precios.Select(p => new Precio
						{
							Id = p.Id,
							PrecioBase = p.PrecioBase,
							FechaInicio = p.FechaInicio
						}).ToList(),
					}).ToList();
			}
        }

        public SeguroMedico GetSeguroMedicoById(int id)
        {
			using (var _dbContext = new DBContext())
			{
                var seguro = _dbContext.SegurosMedicos.Find(id);
                if (seguro != null)
                {
                    return new SeguroMedico
                    {
                        Id = seguro.Id,
                        Nombre = seguro.Nombre,
                        Descripcion = seguro.Descripcion,
                        Contratos = seguro.Contratos.Select(c => new Contrato
                        {
                            Id = c.Id,
                            FechaInicio = c.FechaInicio,
                            Activo = c.Activo
                        }).ToList(),
                        Precios = seguro.Precios.Select(p => new Precio
                        {
                            Id = p.Id,
                            PrecioBase = p.PrecioBase,
                            FechaInicio = p.FechaInicio
                        }).ToList(),
                    };
                }
                return null;
            }
        }

        public void AddSeguroMedico(SeguroMedico seguroMedico)
        {
			using (var _dbContext = new DBContext())
            {
                var nuevoSeguro = new SegurosMedicos
                {
                    Nombre = seguroMedico.Nombre,
                    Descripcion = seguroMedico.Descripcion
                };
                _dbContext.SegurosMedicos.Add(nuevoSeguro);
                _dbContext.SaveChanges();
            }
        }

        public void UpdateSeguroMedico(SeguroMedico seguroMedico)
        {
			using (var _dbContext = new DBContext())
			{
				var seguroExistente = _dbContext.SegurosMedicos.Find(seguroMedico.Id);
				if (seguroExistente != null)
				{
					seguroExistente.Nombre = seguroMedico.Nombre;
					seguroExistente.Descripcion = seguroMedico.Descripcion;
					_dbContext.SaveChanges();
				}
			}
        }

        public void DeleteSeguroMedico(int id)
        {
			using (var _dbContext = new DBContext())
			{
				var seguro = _dbContext.SegurosMedicos.Find(id);
				if (seguro != null)
                {
                    _dbContext.SegurosMedicos.Remove(seguro);
                    _dbContext.SaveChanges();
                }
			}
        }

        #endregion
    }
}
