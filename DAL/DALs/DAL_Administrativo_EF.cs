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

		public void DeletePaciente(long id)
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

		public Paciente GetPacienteById(long id)
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

		public SeguroMedico GetSeguroMedicoById(long id)
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

		public void DeleteSeguroMedico(long id)
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

		/**********************************************************/
		/**                    Contratos                         **/
		/**********************************************************/
		#region FUNCTIONES CONTRATOS

		public List<Contrato> GetContratos()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Contratos
					.Select(c => new Contrato
					{
						Id = c.Id,
						FechaInicio = c.FechaInicio,
						Activo = c.Activo,
						Paciente = new Paciente
						{
							Id = c.Paciente.Id,
							Nombres = c.Paciente.Nombres,
							Apellidos = c.Paciente.Apellidos,
							Documento = c.Paciente.Documento,
							FechaDeNacimiento = c.Paciente.FechaDeNacimiento,
							Direccion = c.Paciente.Direccion,
							Telefono = c.Paciente.Telefono,
							Email = c.Paciente.Email
						},
						SeguroMedico = new SeguroMedico
						{
							Id = c.SeguroMedico.Id,
							Nombre = c.SeguroMedico.Nombre,
							Descripcion = c.SeguroMedico.Descripcion
						}
					}).ToList();
			}
		}

		public Contrato GetContratoById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var contrato = _dbContext.Contratos.Find(id);
				if (contrato != null)
				{
					return new Contrato
					{
						Id = contrato.Id,
						FechaInicio = contrato.FechaInicio,
						Activo = contrato.Activo,
						Paciente = new Paciente
						{
							Id = contrato.Paciente.Id,
							Nombres = contrato.Paciente.Nombres,
							Apellidos = contrato.Paciente.Apellidos,
							Documento = contrato.Paciente.Documento,
							FechaDeNacimiento = contrato.Paciente.FechaDeNacimiento,
							Direccion = contrato.Paciente.Direccion,
							Telefono = contrato.Paciente.Telefono,
							Email = contrato.Paciente.Email
						},
						SeguroMedico = new SeguroMedico
						{
							Id = contrato.SeguroMedico.Id,
							Nombre = contrato.SeguroMedico.Nombre,
							Descripcion = contrato.SeguroMedico.Descripcion
						}
					};
				}
				return null;
			}
		}

		public void AddContrato(Contrato contrato)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoContrato = new Contratos
				{
					Id = contrato.Id,
					FechaInicio = contrato.FechaInicio,
					Activo = contrato.Activo,
					PacienteId = contrato.Paciente.Id,
					SeguroMedicoId = contrato.SeguroMedico.Id
				};
				_dbContext.Contratos.Add(nuevoContrato);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateContrato(Contrato contrato)
		{
			using (var _dbContext = new DBContext())
			{
				var contratoExistente = _dbContext.Contratos.Find(contrato.Id);
				if (contratoExistente != null)
				{
					contratoExistente.FechaInicio = contrato.FechaInicio;
					contratoExistente.Activo = contrato.Activo;
					contratoExistente.PacienteId = contrato.Paciente.Id;
					contratoExistente.SeguroMedicoId = contrato.SeguroMedico.Id;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteContrato(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var contrato = _dbContext.Contratos.Find(id);
				if (contrato != null)
				{
					_dbContext.Contratos.Remove(contrato);
					_dbContext.SaveChanges();
				}
			}
		}

		#endregion

		/**********************************************************/
		/**                    Precios                           **/
		/**********************************************************/
		#region FUNCTIONES PRECIOS

		public List<Precio> GetPrecios()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Precios
					.Select(p => new Precio
					{
						Id = p.Id,
						PrecioBase = p.PrecioBase,
						FechaInicio = p.FechaInicio,
						Copago = new Copago
						{
							Id = p.Copago.Id,
							Articulo = new Articulo
							{
								Id = p.Copago.Articulo.Id,
								Nombre = p.Copago.Articulo.Nombre
							},
							SeguroMedico = new SeguroMedico
							{
								Id = p.Copago.SeguroMedico.Id,
								Nombre = p.Copago.SeguroMedico.Nombre,
								Descripcion = p.Copago.SeguroMedico.Descripcion
							},
							Especialidad = new Especialidad
							{
								Id = p.Copago.Especialidad.Id,
								Nombre = p.Copago.Especialidad.Nombre,
								Descripcion = p.Copago.Especialidad.Descripcion
							}
						},
						SeguroMedico = new SeguroMedico
						{
							Id = p.SeguroMedico.Id,
							Nombre = p.SeguroMedico.Nombre,
							Descripcion = p.SeguroMedico.Descripcion
						}
					}).ToList();
			}
		}

		public Precio GetPrecioById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var precio = _dbContext.Precios.Find(id);
				if (precio != null)
				{
					return new Precio
					{
						Id = precio.Id,
						PrecioBase = precio.PrecioBase,
						FechaInicio = precio.FechaInicio,
						Copago = new Copago
						{
							Id = precio.Copago.Id,
							Articulo = new Articulo
							{
								Id = precio.Copago.Articulo.Id,
								Nombre = precio.Copago.Articulo.Nombre
							},
							SeguroMedico = new SeguroMedico
							{
								Id = precio.Copago.SeguroMedico.Id,
								Nombre = precio.Copago.SeguroMedico.Nombre,
								Descripcion = precio.Copago.SeguroMedico.Descripcion
							},
							Especialidad = new Especialidad
							{
								Id = precio.Copago.Especialidad.Id,
								Nombre = precio.Copago.Especialidad.Nombre,
								Descripcion = precio.Copago.Especialidad.Descripcion
							}
						},
						SeguroMedico = new SeguroMedico
						{
							Id = precio.SeguroMedico.Id,
							Nombre = precio.SeguroMedico.Nombre,
							Descripcion = precio.SeguroMedico.Descripcion
						}
					};
				}
				return null;
			}
		}

		public void AddPrecio(Precio precio)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoPrecio = new Precios
				{
					PrecioBase = precio.PrecioBase,
					FechaInicio = precio.FechaInicio,
					CopagoId = precio.Copago.Id,
					SeguroMedicoId = precio.SeguroMedico.Id
				};
				_dbContext.Precios.Add(nuevoPrecio);
				_dbContext.SaveChanges();
			}
		}

		public void UpdatePrecio(Precio precio)
		{
			using (var _dbContext = new DBContext())
			{
				var precioExistente = _dbContext.Precios.Find(precio.Id);
				if (precioExistente != null)
				{
					precioExistente.PrecioBase = precio.PrecioBase;
					precioExistente.FechaInicio = precio.FechaInicio;
					precioExistente.CopagoId = precio.Copago.Id;
					precioExistente.SeguroMedicoId = precio.SeguroMedico.Id;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeletePrecio(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var precio = _dbContext.Precios.Find(id);
				if (precio != null)
				{
					_dbContext.Precios.Remove(precio);
					_dbContext.SaveChanges();
				}
			}
		}

		#endregion

		/**********************************************************/
		/**                    Copagos                           **/
		/**********************************************************/
		#region FUNCTIONES COPAGOS


		public List<Copago> GetCopagos()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Copagos
					.Select(c => new Copago
					{
						Id = c.Id,
						Articulo = new Articulo
						{
							Id = c.Articulo.Id,
							Nombre = c.Articulo.Nombre
						},
						SeguroMedico = new SeguroMedico
						{
							Id = c.SeguroMedico.Id,
							Nombre = c.SeguroMedico.Nombre,
							Descripcion = c.SeguroMedico.Descripcion
						},
						Especialidad = new Especialidad
						{
							Id = c.Especialidad.Id,
							Nombre = c.Especialidad.Nombre,
							Descripcion = c.Especialidad.Descripcion
						},
						Precios = c.Precios.Select(p => new Precio
						{
							Id = p.Id,
							PrecioBase = p.PrecioBase,
							FechaInicio = p.FechaInicio
						}).ToList()
					}).ToList();
			}
		}

		public Copago GetCopagoById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var copago = _dbContext.Copagos.Find(id);
				if (copago != null)
				{
					return new Copago
					{
						Id = copago.Id,
						Articulo = new Articulo
						{
							Id = copago.Articulo.Id,
							Nombre = copago.Articulo.Nombre
						},
						SeguroMedico = new SeguroMedico
						{
							Id = copago.SeguroMedico.Id,
							Nombre = copago.SeguroMedico.Nombre,
							Descripcion = copago.SeguroMedico.Descripcion
						},
						Especialidad = new Especialidad
						{
							Id = copago.Especialidad.Id,
							Nombre = copago.Especialidad.Nombre,
							Descripcion = copago.Especialidad.Descripcion
						},
						Precios = copago.Precios.Select(p => new Precio
						{
							Id = p.Id,
							PrecioBase = p.PrecioBase,
							FechaInicio = p.FechaInicio
						}).ToList()
					};
				}
				return null;
			}
		}

		public void AddCopago(Copago copago)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoCopago = new Copagos
				{
					ArticuloId = copago.Articulo.Id,
					SeguroMedicoId = copago.SeguroMedico.Id,
					EspecialidadId = copago.Especialidad.Id
				};
				_dbContext.Copagos.Add(nuevoCopago);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateCopago(Copago copago)
		{
			using (var _dbContext = new DBContext())
			{
				var copagoExistente = _dbContext.Copagos.Find(copago.Id);
				if (copagoExistente != null)
				{
					copagoExistente.ArticuloId = copago.Articulo.Id;
					copagoExistente.SeguroMedicoId = copago.SeguroMedico.Id;
					copagoExistente.EspecialidadId = copago.Especialidad.Id;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteCopago(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var copago = _dbContext.Copagos.Find(id);
				if (copago != null)
				{
					_dbContext.Copagos.Remove(copago);
					_dbContext.SaveChanges();
				}
			}
		}
        #endregion

        /**********************************************************/
        /**                    Facturas                          **/
        /**********************************************************/
        #region FUNCTIONES FACTURAS

        public List<Factura> GetFacturas()
        {
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Facturas
					.Select(f => new Factura
					{
						Id = f.Id,
						Fecha = f.Fecha,
						Monto = f.Monto,
						Pago = f.Pago,
						FechaPago = f.FechaPago,
						Paciente = new Paciente
						{
							Id = f.Paciente.Id,
							Nombres = f.Paciente.Nombres,
							Apellidos = f.Paciente.Apellidos,
							Documento = f.Paciente.Documento,
							FechaDeNacimiento = f.Paciente.FechaDeNacimiento,
							Direccion = f.Paciente.Direccion,
							Telefono = f.Paciente.Telefono,
							Email = f.Paciente.Email
						}
					}).ToList();
			}
        }

        public Factura GetFacturaById(long id)
        {
			using (var _dbContext = new DBContext())
			{
				var factura = _dbContext.Facturas.Find(id);
				if (factura != null)
				{
					return new Factura
					{
						Id = factura.Id,
						Fecha = factura.Fecha,
						Monto = factura.Monto,
						Pago = factura.Pago,
						FechaPago = factura.FechaPago,
						Paciente = new Paciente
						{
							Id = factura.Paciente.Id,
							Nombres = factura.Paciente.Nombres,
							Apellidos = factura.Paciente.Apellidos,
							Documento = factura.Paciente.Documento,
							FechaDeNacimiento = factura.Paciente.FechaDeNacimiento,
							Direccion = factura.Paciente.Direccion,
							Telefono = factura.Paciente.Telefono,
							Email = factura.Paciente.Email
						}
					};
				}
				return null;
			}
        }

        public void AddFactura(Factura factura)
        {
			using (var _dbContext = new DBContext())
			{
				var nuevaFactura = new Facturas
				{
					Fecha = factura.Fecha,
					Monto = factura.Monto,
					Pago = factura.Pago,
					FechaPago = factura.FechaPago,
					PacienteId = factura.Paciente.Id
				};
				_dbContext.Facturas.Add(nuevaFactura);
				_dbContext.SaveChanges();
			}
        }

        public void UpdateFactura(Factura factura)
        {
			using (var _dbContext = new DBContext())
			{
				var facturaExistente = _dbContext.Facturas.Find(factura.Id);
				if (facturaExistente != null)
				{
					facturaExistente.Fecha = factura.Fecha;
					facturaExistente.Monto = factura.Monto;
					facturaExistente.Pago = factura.Pago;
					facturaExistente.FechaPago = factura.FechaPago;
					facturaExistente.PacienteId = factura.Paciente.Id;
					_dbContext.SaveChanges();
				}
			}
        }

        public void DeleteFactura(long id)
        {
			using (var _dbContext = new DBContext())
			{
				var factura = _dbContext.Facturas.Find(id);
				if (factura != null)
				{
					_dbContext.Facturas.Remove(factura);
					_dbContext.SaveChanges();
				}
			}
        }

        #endregion
    }
}
