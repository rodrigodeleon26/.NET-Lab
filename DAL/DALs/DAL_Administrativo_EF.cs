using DAL.IDALs;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

		private readonly ILogger<DAL_Administrativo_EF> _logger;

		public DAL_Administrativo_EF(ILogger<DAL_Administrativo_EF> logger)
		{
			_logger = logger;
		}
		/**********************************************************/
		/**                  PACIENTES                           **/
		/**********************************************************/
		#region FUNCTIONES PACIENTES

        public List<Notificacion> getNotificaciones(long id, int pageNumber, int pageSize)
        {
            using (var _dbContext = new DBContext())
            {
                var paciente = _dbContext.Pacientes.Find(id);
                return _dbContext.Notificaciones
                    .Where(n => n.PacienteId == id)
                    .OrderByDescending(n => n.FechaEnvio)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(n => new Notificacion
                    {
                        Id = n.Id,
                        Mensaje = n.Mensaje,
                        FechaEnvio = n.FechaEnvio,
                        Visto = n.Visto,
                        Paciente = new Paciente
                        {
                            Id = paciente.Id,
                            Nombres = paciente.Nombres,
                            Apellidos = paciente.Apellidos,
                            Documento = paciente.Documento,
                            FechaDeNacimiento = paciente.FechaDeNacimiento,
                            Direccion = paciente.Direccion,
                            Telefono = paciente.Telefono,
                            Email = paciente.Email
                        }
                    })
                    .ToList();
            }
        }

        public int CountNotificaciones(long id)
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Notificaciones.Count(n => n.PacienteId == id);
            }
        }

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
                    paciente.Activo = false; // Cambiar el estado a inactivo
                    _dbContext.Pacientes.Update(paciente);
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
					var contrato = _dbContext.Contratos
						.Include(c => c.SeguroMedico)
						.FirstOrDefault(c => c.PacienteId == paciente.Id);

					return new Paciente
					{
						Id = paciente.Id,
						Nombres = paciente.Nombres,
						Apellidos = paciente.Apellidos,
						Documento = paciente.Documento,
						FechaDeNacimiento = paciente.FechaDeNacimiento,
						Direccion = paciente.Direccion,
						Telefono = paciente.Telefono,
						Email = paciente.Email,
						Contrato = contrato != null ? new Contrato
						{
							Id = contrato.Id,
							FechaInicio = contrato.FechaInicio,
							Activo = contrato.Activo,
							SeguroMedico = new SeguroMedico
							{
								Id = contrato.SeguroMedico.Id,
								Nombre = contrato.SeguroMedico.Nombre,
								Descripcion = contrato.SeguroMedico.Descripcion
							}
						} : null
					};
				}
				return null;
			}
		}

		public Paciente GetPacienteByDNI(string dni)
		{
			using (var _dbContext = new DBContext())
			{
				var paciente = _dbContext.Pacientes
					.Include(p => p.Contrato)
					.ThenInclude(c => c.SeguroMedico)
					.FirstOrDefault(p => p.Documento == dni);

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
						Email = paciente.Email,
						Contrato = paciente.Contrato != null ? new Contrato
						{
							Id = paciente.Contrato.Id,
							FechaInicio = paciente.Contrato.FechaInicio,
							Activo = paciente.Contrato.Activo,
							SeguroMedico = new SeguroMedico
							{
								Id = paciente.Contrato.SeguroMedico.Id,
								Nombre = paciente.Contrato.SeguroMedico.Nombre,
								Descripcion = paciente.Contrato.SeguroMedico.Descripcion
							}
						} : null
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
                    if (nuevaCedulaOcupada(paciente.Documento, paciente.Id))
                    {
                        throw new Exception("Ya existe un paciente con la cedula ingresada");
                    }

                    existingPaciente.Nombres = paciente.Nombres;
                    existingPaciente.Apellidos = paciente.Apellidos;
                    existingPaciente.Documento = paciente.Documento;
                    existingPaciente.FechaDeNacimiento = paciente.FechaDeNacimiento;
                    existingPaciente.Direccion = paciente.Direccion;
                    existingPaciente.Telefono = paciente.Telefono;
                    existingPaciente.Email = paciente.Email;
                    // Update Contrato
                    if (paciente.Contrato != null)
                    {
                        var existingContrato = _dbContext.Contratos
                        .FirstOrDefault(c => c.PacienteId == paciente.Id);

                        if (existingContrato != null)
                        {
                            existingContrato.FechaInicio = paciente.Contrato.FechaInicio;
                            existingContrato.Activo = paciente.Contrato.Activo;
                            existingContrato.SeguroMedicoId = paciente.Contrato.SeguroMedico.Id;
                        }
                        else
                        {
                            existingPaciente.Contrato = new Contratos
                            {
                                Id = paciente.Contrato.Id,
                                FechaInicio = paciente.Contrato.FechaInicio,
                                Activo = paciente.Contrato.Activo,
                                PacienteId = paciente.Id,
                                SeguroMedicoId = paciente.Contrato.SeguroMedico.Id
                            };
                        }
                    }
                    else
                    {
                        existingPaciente.Contrato = null;
                    }

                    _dbContext.SaveChanges();
                }
            }
        }

        public bool nuevaCedulaOcupada(string nuevaCi, long pacienteId)
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Pacientes.Any(p => p.Documento == nuevaCi && p.Id != pacienteId);
			}
		}

		public List<Paciente> GetPacientesFiltradosPaginados(int numPagina, string filtro)
		{
			using (var _dbContext = new DBContext())
			{
				var query = _dbContext.Pacientes.AsQueryable();

				if (!string.IsNullOrEmpty(filtro))
				{
					query = query.Where(p => p.Nombres.Contains(filtro) || p.Apellidos.Contains(filtro) || p.Documento.Contains(filtro));
				}

				return query
					.Skip((numPagina - 1) * 5)
					.Take(5)
					.Select(p => new Paciente
					{
						Id = p.Id,
						Nombres = p.Nombres,
						Apellidos = p.Apellidos,
						Documento = p.Documento,
						Direccion = p.Direccion,
						Telefono = p.Telefono,
						Email = p.Email,
						Activo = p.Activo
					}).ToList();
			}
		}

		public bool emailDuplicado(string email)
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Pacientes.Any(p => p.Email == email);
			}
		}

		public bool cedulaDuplicada(string cedula)
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Pacientes.Any(p => p.Documento == cedula);
			}
		}

		public List<Factura> getHistorialFacturacion(long id, int pageNumber, int pageSize)
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Facturas
					.Where(f => f.PacienteId == id && f.Pago == true)
					.OrderByDescending(f => f.Fecha)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.Select(f => new Factura
					{
						Id = f.Id,
						Fecha = f.Fecha,
						FechaPago = f.FechaPago,
						Monto = f.Monto,
						Pago = f.Pago,
						Descripcion = f.Descripcion,
					})
					.ToList();
			}
		}

		public int countFacturas(long id)
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Facturas.Count(f => f.PacienteId == id);
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
					}).ToList()
				}).ToList();
		}
	}

	public SeguroMedico GetSeguroMedicoById(long id)
	{
		using (var _dbContext = new DBContext())
		{
			var seguro = _dbContext.SegurosMedicos
				.Include(s => s.Contratos)
				.Include(s => s.Precios)
				.Include(s => s.Copagos)
					.ThenInclude(c => c.Articulo)
				.Include(s => s.Copagos)
					.ThenInclude(c => c.Especialidad)
				.Include(s => s.Copagos)
					.ThenInclude(c => c.Precios)
				.FirstOrDefault(s => s.Id == id);
				
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
					Copagos = seguro.Copagos.Select(c => new Copago
					{
						Id = c.Id,
						Articulo = new Articulo
						{
							Id = c.Articulo.Id,
							Nombre = c.Articulo.Nombre
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
					}).ToList()
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
					var paciente = _dbContext.Pacientes.Find(contrato.PacienteId);
					var seguroMedico = _dbContext.SegurosMedicos.Find(contrato.SeguroMedicoId);
					if (seguroMedico != null && paciente != null)
					{
						return new Contrato
						{
							Id = contrato.Id,
							FechaInicio = contrato.FechaInicio,
							Activo = contrato.Activo,
							Paciente = new Paciente
							{
								Id = paciente.Id,
								Nombres = paciente.Nombres,
								Apellidos = paciente.Apellidos,
								Documento = paciente.Documento,
								FechaDeNacimiento = paciente.FechaDeNacimiento,
								Direccion = paciente.Direccion,
								Telefono = paciente.Telefono,
								Email = paciente.Email
							},
							SeguroMedico = new SeguroMedico
							{
								Id = seguroMedico.Id,
								Nombre = seguroMedico.Nombre,
								Descripcion = seguroMedico.Descripcion
							}
						};
					}
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

        public List<Contrato> GetContratosFiltradosPaginados(int numPagina, string filtro)
        {
            using (var _dbContext = new DBContext())
            {
                var query = _dbContext.Contratos
                    .Include(c => c.Paciente)
                    .Include(c => c.SeguroMedico)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(c => c.Paciente.Documento.Contains(filtro));
                }

                return query
                    .Skip((numPagina - 1) * 5)
                    .Take(5)
                    .Select(c => new Contrato
                    {
                        Id = c.Id,
                        FechaInicio = c.FechaInicio,
                        Activo = c.Activo,
                        Paciente = new Paciente
                        {
                            Documento = c.Paciente.Documento
                        },
                        SeguroMedico = new SeguroMedico
                        {
                            Nombre = c.SeguroMedico.Nombre
                        }
                    }).ToList();
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
						Copago = p.Copago != null ? new Copago
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
						}: null,
						SeguroMedico = p.SeguroMedico != null ? new SeguroMedico
						{
							Id = p.SeguroMedico.Id,
							Nombre = p.SeguroMedico.Nombre,
							Descripcion = p.SeguroMedico.Descripcion
						}: null
					}).ToList();
			}
		}

		public Precio GetPrecioById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var precio = _dbContext.Precios
					.Include(Precios => Precios.Copago)
						.ThenInclude(c => c.Articulo)
					.Include(Precios => Precios.Copago)
						.ThenInclude(c => c.Especialidad)
					.Include(Precios => Precios.Copago)
						.ThenInclude(c => c.SeguroMedico)
					.Include(Precios => Precios.SeguroMedico)
					.FirstOrDefault(p => p.Id == id);

				if (precio != null)
				{
					_logger.LogInformation($"Precio encontrado: {precio.Id}");
					return new Precio
					{
						Id = precio.Id,
						PrecioBase = precio.PrecioBase,
						FechaInicio = precio.FechaInicio,
						Copago = precio.Copago != null ? new Copago()
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
						} : null,
						SeguroMedico = precio.SeguroMedico != null ? new SeguroMedico()
						{
							Id = precio.SeguroMedico.Id,
							Nombre = precio.SeguroMedico.Nombre,
							Descripcion = precio.SeguroMedico.Descripcion
						} : null

					};
				}
				_logger.LogInformation("Precio encontrado: es null");
				return null;
			}
		}

        public Precio GetPrecioBySeguro(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var precio = _dbContext.Precios
					.Include(precio => precio.Copago)
						.ThenInclude(c => c.Articulo)
					.Include(precio => precio.Copago)
						.ThenInclude(c => c.Especialidad)
					.Include(precio => precio.Copago)
						.ThenInclude(c => c.SeguroMedico)
					.Include(precio => precio.SeguroMedico)
					.Where(p => p.SeguroMedicoId == id)
					.OrderByDescending(p => p.FechaInicio)
					.FirstOrDefault();

                if (precio != null)
                {
                    _logger.LogInformation($"Precio encontrado: {precio.Id}");
                    return new Precio
                    {
                        Id = precio.Id,
                        PrecioBase = precio.PrecioBase,
                        FechaInicio = precio.FechaInicio,
                        Copago = precio.Copago != null ? new Copago()
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
                        } : null,
                        SeguroMedico = precio.SeguroMedico != null ? new SeguroMedico()
                        {
                            Id = precio.SeguroMedico.Id,
                            Nombre = precio.SeguroMedico.Nombre,
                            Descripcion = precio.SeguroMedico.Descripcion
                        } : null

                    };
                }
                _logger.LogInformation("Precio encontrado: es null");
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
					CopagoId = null,
					SeguroMedicoId = null
				};
				if(precio.Copago != null)
				{
					nuevoPrecio.CopagoId = precio.Copago.Id;
				}
				if (precio.SeguroMedico != null)
				{
					nuevoPrecio.SeguroMedicoId = precio.SeguroMedico.Id;
				}
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
				var copago = _dbContext.Copagos
					.Include(c => c.Articulo)
					.Include(c => c.SeguroMedico)
					.Include(c => c.Especialidad)
					.Include(c => c.Precios)
					.FirstOrDefault(c => c.Id == id);

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

		public long getIdByFilds(Copago copago)
		{
			using (var _dbContext = new DBContext())
			{
				var copagoExistente = _dbContext.Copagos.FirstOrDefault(c => c.ArticuloId == copago.Articulo.Id && c.SeguroMedicoId == copago.SeguroMedico.Id && c.EspecialidadId == copago.Especialidad.Id);
				if (copagoExistente != null)
				{
					return copagoExistente.Id;
				}
				return 0;
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
                        Descripcion = f.Descripcion,
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

        public List<Factura> ObtenerUltimasFacturasDelContrato(long contratoId, int cantidad)
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Facturas
                    .Where(f => f.Paciente.Contrato.Id == contratoId)
                    .OrderByDescending(f => f.Fecha)
                    .Take(cantidad)
                    .Select(f => new Factura
                    {
                        Id = f.Id,
                        Fecha = f.Fecha,
                        Monto = f.Monto,
                        Pago = f.Pago,
                        FechaPago = f.FechaPago,
                        Descripcion = f.Descripcion,
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
                    })
                    .ToList();
            }
        }

        public bool ExisteFacturaParaPacienteEnMes(long pacienteId, int mes, int año)
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Facturas
                    .Any(f => f.Paciente.Id == pacienteId && f.Fecha.Month == mes && f.Fecha.Year == año);
            }
        }

        public List<Factura> GetFacturasPaginadas(int numPagina, string? pacienteString, bool fechaAsc, bool? estaPago)
        {
            using (var _dbContext = new DBContext())
            {
                var query = _dbContext.Facturas.AsQueryable();

                if (!string.IsNullOrEmpty(pacienteString))
                {
                    query = query.Where(f => f.Paciente.Nombres.Contains(pacienteString) || f.Paciente.Apellidos.Contains(pacienteString) || f.Paciente.Documento.Contains(pacienteString));
                }

                if (estaPago.HasValue)
                {
                    query = query.Where(f => f.Pago == estaPago.Value);
                }

                query = fechaAsc ? query.OrderBy(f => f.Fecha) : query.OrderByDescending(f => f.Fecha);

                return query
                    .Skip((numPagina - 1) * 20)
                    .Take(20)
                    .Select(f => new Factura
                    {
                        Id = f.Id,
                        Fecha = f.Fecha,
                        Monto = f.Monto,
                        Pago = f.Pago,
                        FechaPago = f.FechaPago,
                        Descripcion = f.Descripcion,
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
                // Carga la factura con su relación a Paciente
                var factura = _dbContext.Facturas
                                        .Include(f => f.Paciente)
                                        .FirstOrDefault(f => f.Id == id);

                if (factura != null)
                {
                    return new Factura
                    {
                        Id = factura.Id,
                        Fecha = factura.Fecha,
                        Monto = factura.Monto,
                        Pago = factura.Pago,
                        FechaPago = factura.FechaPago,
						Descripcion = factura.Descripcion,
                        Paciente = new Paciente
                        {
                            Id = factura.Paciente.Id,
                            Nombres = factura.Paciente.Nombres,
                            Apellidos = factura.Paciente.Apellidos,
                            Documento = factura.Paciente.Documento
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
                    Descripcion = factura.Descripcion,
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
					facturaExistente.Descripcion = factura.Descripcion;
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

        public IEnumerable<Contrato> GetContratosActivos()
        {
            using (var _dbContext = new DBContext())
            {
                return _dbContext.Contratos
                    .Where(c => c.Activo)
                    .Include(c => c.Paciente)
                    .Include(c => c.SeguroMedico)
                    .ToList()
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
                    });
            }
        }

        public async Task SaveChangesAsync()
        {
            using (var _dbContext = new DBContext())
            {
				await _dbContext.SaveChangesAsync();
            }
        }

        #endregion


        /**********************************************************/
        /**                     Medicos                          **/
        /**********************************************************/
        #region FUNCTIONES MEDICOS

        public List<Medico> GetMedicos()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Medicos
                    .Where(m => m.Activo)
                    .Select(m => new Medico
					{
						Id = m.Id,
						Nombres = m.Nombres,
						Apellidos = m.Apellidos,
						Documento = m.Documento,
						Email = m.Email,
						Telefono = m.Telefono,
						Especialidades = m.EspecialidadesMedicos.Select(em => new Especialidad
						{
							Id = em.Especialidad.Id,
							Nombre = em.Especialidad.Nombre,
							Descripcion = em.Especialidad.Descripcion
						}).ToList()
					}).ToList();
			}
		}

		public Medico GetMedicoById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var medico = _dbContext.Medicos.Where(m => m.Activo)
                               .Include(m => m.EspecialidadesMedicos)
							   .ThenInclude(me => me.Especialidad)
							   .FirstOrDefault(m => m.Id == id);

				if (medico != null)
				{
					return new Medico
					{
						Id = medico.Id,
						Nombres = medico.Nombres,
						Apellidos = medico.Apellidos,
						Documento = medico.Documento,
						Email = medico.Email,
						Telefono = medico.Telefono,
						Especialidades = medico.EspecialidadesMedicos.Select(em => new Especialidad
						{
							Id = em.Especialidad.Id,
							Nombre = em.Especialidad.Nombre,
							Descripcion = em.Especialidad.Descripcion
						}).ToList()
					};
				}
				return null;
			}
		}

        public Medico GetMedicoByDocumento(string ci)
        {
            using (var _dbContext = new DBContext())
            {
                var medico = _dbContext.Medicos
                               .Include(m => m.EspecialidadesMedicos)
                               .ThenInclude(me => me.Especialidad)
                               .FirstOrDefault(m => m.Documento == ci);

                if (medico != null)
                {
                    return new Medico
                    {
                        Id = medico.Id,
                        Nombres = medico.Nombres,
                        Apellidos = medico.Apellidos,
                        Documento = medico.Documento,
                        Email = medico.Email,
                        Telefono = medico.Telefono,
                        Especialidades = medico.EspecialidadesMedicos.Select(em => new Especialidad
                        {
                            Id = em.Especialidad.Id,
                            Nombre = em.Especialidad.Nombre,
                            Descripcion = em.Especialidad.Descripcion
                        }).ToList()
                    };
                }
                return null;
            }
        }

        public void AddMedico(Medico medico)
		{
			using (var _dbContext = new DBContext())
			{
				//chequear que no existan con la misma ci
				if (_dbContext.Medicos.Any(m => m.Documento == medico.Documento))
				{
					throw new Exception("Ya existe un medico con la cedula ingresada");
				}

				var nuevoMedico = new Medicos
				{
					Nombres = medico.Nombres,
					Apellidos = medico.Apellidos,
					Documento = medico.Documento,
					Email = medico.Email,
					Telefono = medico.Telefono
				};
				if (!medico.Especialidades.IsNullOrEmpty())
				{
					nuevoMedico.EspecialidadesMedicos = medico.Especialidades.Select(e => new EspecialidadesMedicos
					{
						MedicoId = medico.Id,
						EspecialidadId = e.Id
					}).ToList();
				}
				_dbContext.Medicos.Add(nuevoMedico);
				_dbContext.SaveChanges();
			}
		}

        public void UpdateMedico(Medico medico)
        {
            using (var _dbContext = new DBContext())
            {
                var medicoExistente = _dbContext.Medicos.Find(medico.Id);
                if (medicoExistente != null)
                {
                    medicoExistente.Nombres = medico.Nombres;
                    medicoExistente.Apellidos = medico.Apellidos;
                    medicoExistente.Documento = medico.Documento;
                    medicoExistente.Email = medico.Email;
                    medicoExistente.Telefono = medico.Telefono;
                    // Obtener todos los Ids de Especialidades del nuevo medico
                    var especialidadesIds = medico.Especialidades.Select(e => e.Id).ToList();

                    // Especialidades que ya existen y se mantienen, estan en el arreglo especialidadesIds
                    var especialidadesExistentes = _dbContext.EspecialidadesMedicos
                    .Where(em => em.MedicoId == medico.Id && especialidadesIds.Contains(em.EspecialidadId))
                    .Select(em => em.EspecialidadId)
                    .ToList();

                    //especialidades que ya existen pero no estan en especialidadesIds por lo que hay que borrarlas
                    var especialidadesABorrar = _dbContext.EspecialidadesMedicos
                    .Where(em => em.MedicoId == medico.Id && !especialidadesIds.Contains(em.EspecialidadId))
                    .ToList();

                    // Filtrar para agregar solo las especialidades que aún no existen
                    medicoExistente.EspecialidadesMedicos = medico.Especialidades
                    .Where(e => !especialidadesExistentes.Contains(e.Id))
                    .Select(e => new EspecialidadesMedicos
                    {
                        MedicoId = medico.Id,
                        EspecialidadId = e.Id
                    })
                    .ToList();

                    // Borrar aquellas especialidades que ya no existen
                    foreach (var especialidad in especialidadesABorrar)
                    {
                        medicoExistente.EspecialidadesMedicos.Remove(especialidad);
                    }

                    _dbContext.SaveChanges();
                }
            }
        }

		public void DeleteMedico(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var medico = _dbContext.Medicos.Find(id);
				if (medico != null)
				{
                    //_dbContext.Medicos.Remove(medico);
                    medico.Activo = false;
                    _dbContext.SaveChanges();
				}
			}
		}

		public List<Medico> GetMedicosPaginadosYFiltrados(int numPagina, string filtro)
		{
			using (var _dbContext = new DBContext())
			{
				var query = _dbContext.Medicos.AsQueryable();

				if (!string.IsNullOrEmpty(filtro))
				{
					query = query.Where(m => m.Nombres.Contains(filtro) || m.Apellidos.Contains(filtro) || m.Documento.Contains(filtro));
				}

				return query
					.Skip((numPagina - 1) * 10)
					.Take(10)
                    .Where(m => m.Activo)
                    .Select(m => new Medico
					{
						Id = m.Id,
						Nombres = m.Nombres,
						Apellidos = m.Apellidos,
						Documento = m.Documento,
						Email = m.Email,
						Telefono = m.Telefono,
						Especialidades = m.EspecialidadesMedicos.Select(em => new Especialidad
						{
							Id = em.Especialidad.Id,
							Nombre = em.Especialidad.Nombre,
							Descripcion = em.Especialidad.Descripcion
						}).ToList()
					}).ToList();
			}
		}

		#endregion


		/**********************************************************/
		/**                 Citas Medicas                        **/
		/**********************************************************/
		#region FUNCTIONES CITAS MEDICAS

		public List<CitaMedica> GetCitasMedicas()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.CitasMedicas
					.Select(c => new CitaMedica
					{
						Id = c.Id,
						Fecha = c.Fecha,
						Estado = c.Estado,
						Calendario = new Calendario
						{
							Id = c.Calendario.Id,
							HoraInicio = c.Calendario.HoraInicio,
							HoraFin = c.Calendario.HoraFin,
							TiempoCita = c.Calendario.TiempoCita,
							CantidadCitas = c.Calendario.CantidadCitas,
							DiasSemana = c.Calendario.DiasSemana,
							Medico = new Medico
							{
								Id = c.Calendario.Medico.Id,
								Nombres = c.Calendario.Medico.Nombres,
								Apellidos = c.Calendario.Medico.Apellidos,
								Documento = c.Calendario.Medico.Documento,
								Email = c.Calendario.Medico.Email,
								Telefono = c.Calendario.Medico.Telefono
							},
							Especialidad = new Especialidad
							{
								Id = c.Calendario.Especialidad.Id,
								Nombre = c.Calendario.Especialidad.Nombre,
								Descripcion = c.Calendario.Especialidad.Descripcion
							},
						}
					}).ToList();
			}
		}

		public CitaMedica GetCitasMedicasById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var cita = _dbContext.CitasMedicas.Find(id);
				if ( cita != null)
				{

					return new CitaMedica
					{
						Id = cita.Id,
						Fecha = cita.Fecha,
						Estado = cita.Estado,
						Calendario = new Calendario
						{
							Id = cita.Calendario.Id,
							HoraInicio = cita.Calendario.HoraInicio,
							HoraFin = cita.Calendario.HoraFin,
							TiempoCita = cita.Calendario.TiempoCita,
							CantidadCitas = cita.Calendario.CantidadCitas,
							DiasSemana = cita.Calendario.DiasSemana,
							Medico = new Medico
							{
								Id = cita.Calendario.Medico.Id,
								Nombres = cita.Calendario.Medico.Nombres,
								Apellidos = cita.Calendario.Medico.Apellidos,
								Documento = cita.Calendario.Medico.Documento,
								Email = cita.Calendario.Medico.Email,
								Telefono = cita.Calendario.Medico.Telefono
							},
							Especialidad = new Especialidad
							{
								Id = cita.Calendario.Especialidad.Id,
								Nombre = cita.Calendario.Especialidad.Nombre,
								Descripcion = cita.Calendario.Especialidad.Descripcion
							},
						}
					};
				}
				return null;
			}
		}

		public void AddCitasMedicas(CitaMedica citasMedicas)
		{
			using (var _dbContext = new DBContext())
			{
				_logger.LogInformation("pacienteID: " + citasMedicas.PacienteId);
				var nuevaCita = new CitasMedicas
				{
					Fecha = citasMedicas.Fecha,
					Estado = citasMedicas.Estado,
					CalendarioId = citasMedicas.CalendarioId,
                    PacienteId = citasMedicas.PacienteId
				};
				_dbContext.CitasMedicas.Add(nuevaCita);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateCitasMedicas(CitaMedica citasMedicas)
		{
			using (var _dbContext = new DBContext())
			{
				var citaExistente = _dbContext.CitasMedicas.Find(citasMedicas.Id);
				if (citaExistente != null)
				{
					citaExistente.Fecha = citasMedicas.Fecha;
					citaExistente.Estado = citasMedicas.Estado;
					citaExistente.CalendarioId = citasMedicas.Calendario.Id;
					citaExistente.PacienteId = citasMedicas.PacienteId;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteCitasMedicas(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var cita = _dbContext.CitasMedicas.Find(id);
				if (cita != null)
				{
					_dbContext.CitasMedicas.Remove(cita);
					_dbContext.SaveChanges();
				}
			}
		}

		#endregion


		/**********************************************************/
		/**                 Calendarios                          **/
		/**********************************************************/
		#region FUNCTIONES CALENDARIOS

		public List<Calendario> GetCalendarios()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Calendarios
					.Where(c => c.Activo) // filtrar calendarios activos
					.Select(c => new Calendario
					{
						Id = c.Id,
						HoraInicio = c.HoraInicio,
						HoraFin = c.HoraFin,
						TiempoCita = c.TiempoCita,
						CantidadCitas = c.CantidadCitas,
						DiasSemana = c.DiasSemana,
						Consultorio = new Consultorio
						{
							Id = c.Consultorio.Id,
							Numero = c.Consultorio.Numero,
							Piso = c.Consultorio.Piso
						},
						Medico = new Medico
						{
							Id = c.Medico.Id,
							Nombres = c.Medico.Nombres,
							Apellidos = c.Medico.Apellidos,
							Documento = c.Medico.Documento,
							Email = c.Medico.Email,
							Telefono = c.Medico.Telefono
						},
						Especialidad = new Especialidad
						{
							Id = c.Especialidad.Id,
							Nombre = c.Especialidad.Nombre,
							Descripcion = c.Especialidad.Descripcion
						},
						CitasMedicas = c.CitasMedicas.Select(c => new CitaMedica
						{
							Id = c.Id,
							Fecha = c.Fecha,
							Estado = c.Estado

						}).ToList(),
					}).ToList();
			}
		}

		public Calendario GetCalendarioById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var c = _dbContext.Calendarios.Where(c => c.Activo)
					.Include(c => c.Medico)
					.Include(c => c.Especialidad)
					.Include(c => c.Consultorio)
					.FirstOrDefault(c => c.Id == id);

				if (c != null)
				{
					var citas = _dbContext.CitasMedicas
						.Where(cm => cm.CalendarioId == c.Id)
						.ToList();

					return new Calendario
					{
						Id = c.Id,
						HoraInicio = c.HoraInicio,
						HoraFin = c.HoraFin,
						TiempoCita = c.TiempoCita,
						CantidadCitas = c.CantidadCitas,
						DiasSemana = c.DiasSemana,
						Consultorio = new Consultorio
						{
							Id = c.Consultorio.Id,
							Numero = c.Consultorio.Numero,
							Piso = c.Consultorio.Piso
						},
						Medico = new Medico
						{
							Id = c.Medico.Id,
							Nombres = c.Medico.Nombres,
							Apellidos = c.Medico.Apellidos,
							Documento = c.Medico.Documento,
							Email = c.Medico.Email,
							Telefono = c.Medico.Telefono
						},
						Especialidad = new Especialidad
						{
							Id = c.Especialidad.Id,
							Nombre = c.Especialidad.Nombre,
							Descripcion = c.Especialidad.Descripcion
						},
						CitasMedicas = !citas.IsNullOrEmpty() ? citas.Select(cm => new CitaMedica
						{
							Id = cm.Id,
							Fecha = cm.Fecha,
							Estado = cm.Estado
						}).ToList() : new List<CitaMedica>()
					};
				}
				return null;
			}
		}

		public void AddCalendario(Calendario calendario)
		{
			using (var _dbContext = new DBContext())
			{
				//chequeo que no exista un calendario con el medico y la especialidad
				//if(_dbContext.Calendarios.Any(c => c.MedicoId == calendario.Medico.Id && c.EspecialidadId == calendario.Especialidad.Id)){
				//	throw new Exception("Ya existe un calendario para el medico y especialidad seleccionados");
				//}

				var nuevoCalendario = new Calendarios
				{
					HoraInicio = calendario.HoraInicio,
					HoraFin = calendario.HoraFin,
					TiempoCita = calendario.TiempoCita,
					CantidadCitas = calendario.CantidadCitas,
					DiasSemana = calendario.DiasSemana,
					MedicoId = calendario.Medico.Id,
					EspecialidadId = calendario.Especialidad.Id,
					ConsultorioId = calendario.Consultorio.Id
				};
				_dbContext.Calendarios.Add(nuevoCalendario);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateCalendario(Calendario calendario)
		{
			using (var _dbContext = new DBContext())
			{
				var ExistCalendario = _dbContext.Calendarios.Find(calendario.Id);
				
				if (ExistCalendario != null)
				{
					ExistCalendario.HoraInicio = calendario.HoraInicio;
					ExistCalendario.HoraFin = calendario.HoraFin;
					ExistCalendario.TiempoCita = calendario.TiempoCita;
					ExistCalendario.CantidadCitas = calendario.CantidadCitas;
					ExistCalendario.DiasSemana = calendario.DiasSemana;
					ExistCalendario.ConsultorioId = calendario.Consultorio.Id;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteCalendario(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var calendario = _dbContext.Calendarios.Find(id);
				if (calendario != null)
				{
					//_dbContext.Calendarios.Remove(calendario);

					calendario.Activo = false;
					_dbContext.SaveChanges();
				}
			}
		}

        public List<Calendario> GetCalendariosFiltrados(long medicoId, string filtroEspecialidad, string filtroDia, string filtroHoraInicio)
        {
            using (var _dbContext = new DBContext())
            {
                // Obtener la consulta base para los calendarios del médico
                var query = _dbContext.Calendarios
                    .Where(c => c.MedicoId == medicoId && c.Activo)
                    .AsQueryable();

                // Filtrar por especialidad
                if (!string.IsNullOrEmpty(filtroEspecialidad) && filtroEspecialidad != "PorDefecto")
                {
                    if (filtroEspecialidad == "Agrupar")
                    {
                        // Agrupar por especialidad
                        query = query.OrderBy(c => c.Especialidad.Nombre);
                    }
                    else
                    {
                        // Filtrar por nombre de especialidad
                        query = query.Where(c => c.Especialidad.Nombre == filtroEspecialidad);
                    }
                }

                // Filtrar por día
                if (!string.IsNullOrEmpty(filtroDia) && filtroDia != "PorDefecto")
                {
                    query = query.Where(c => c.DiasSemana.Contains(filtroDia));
                }

                // Ordenar por hora de inicio
                if (!string.IsNullOrEmpty(filtroHoraInicio) && filtroHoraInicio != "PorDefecto")
                {
                    query = filtroHoraInicio.Equals("Ascendente", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(c => c.HoraInicio)
                        : query.OrderByDescending(c => c.HoraInicio);
                }

                // Seleccionar los calendarios con las relaciones necesarias
                return query.Select(c => new Calendario
                {
                    Id = c.Id,
                    HoraInicio = c.HoraInicio,
                    HoraFin = c.HoraFin,
                    TiempoCita = c.TiempoCita,
                    CantidadCitas = c.CantidadCitas,
                    DiasSemana = c.DiasSemana,
                    Consultorio = new Consultorio
                    {
                        Id = c.Consultorio.Id,
                        Numero = c.Consultorio.Numero,
                        Piso = c.Consultorio.Piso
                    },
                    Medico = new Medico
                    {
                        Id = c.Medico.Id,
                        Nombres = c.Medico.Nombres,
                        Apellidos = c.Medico.Apellidos,
                        Documento = c.Medico.Documento,
                        Email = c.Medico.Email,
                        Telefono = c.Medico.Telefono
                    },
                    Especialidad = new Especialidad
                    {
                        Id = c.Especialidad.Id,
                        Nombre = c.Especialidad.Nombre,
                        Descripcion = c.Especialidad.Descripcion
                    },
                    CitasMedicas = c.CitasMedicas.Select(cm => new CitaMedica
                    {
                        Id = cm.Id,
                        Fecha = cm.Fecha,
                        Estado = cm.Estado
                    }).ToList()
                }).ToList();
            }
        }


        #endregion


        /**********************************************************/
        /**                 Consultorios                         **/
        /**********************************************************/
        #region FUNCTIONES CONSULTORIOS

        public List<Consultorio> GetConsultorios()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Consultorios
					.Select(c => new Consultorio
					{
						Id = c.Id,
						Numero = c.Numero,
						Piso = c.Piso
					}).ToList();
			}
		}

		public Consultorio GetConsultorioById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var consultorio = _dbContext.Consultorios.Find(id);
				if (consultorio != null)
				{
					return new Consultorio
					{
						Id = consultorio.Id,
						Numero = consultorio.Numero,
						Piso = consultorio.Piso
					};
				}
				return null;
			}
		}

		public void AddConsultorio(Consultorio consultorio)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoConsultorio = new Consultorios
				{
					Numero = consultorio.Numero,
					Piso = consultorio.Piso
				};
				_dbContext.Consultorios.Add(nuevoConsultorio);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateConsultorio(Consultorio consultorio)
		{
			using (var _dbContext = new DBContext())
			{
				var consultorioExistente = _dbContext.Consultorios.Find(consultorio.Id);
				if (consultorioExistente != null)
				{
					consultorioExistente.Numero = consultorio.Numero;
					consultorioExistente.Piso = consultorio.Piso;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteConsultorio(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var consultorio = _dbContext.Consultorios.Find(id);
				if (consultorio != null)
				{
					_dbContext.Consultorios.Remove(consultorio);
					_dbContext.SaveChanges();
				}
			}
		}

		#endregion


		/**********************************************************/
		/**                 Especialidades                       **/
		/**********************************************************/
		#region FUNCTIONES ESPECIALIDADES

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

		public Especialidad GetEspecialidadById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var especialidad = _dbContext.Especialidades.Find(id);
				if (especialidad != null)
				{
					return new Especialidad
					{
						Id = especialidad.Id,
						Nombre = especialidad.Nombre,
						Descripcion = especialidad.Descripcion
					};
				}
				return null;
			}
		}

		public void AddEspecialidad(Especialidad especialidad)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevaEspecialidad = new Especialidades
				{
					Nombre = especialidad.Nombre,
					Descripcion = especialidad.Descripcion
				};
				_dbContext.Especialidades.Add(nuevaEspecialidad);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateEspecialidad(Especialidad especialidad)
		{
			using (var _dbContext = new DBContext())
			{
				var especialidadExistente = _dbContext.Especialidades.Find(especialidad.Id);
				if (especialidadExistente != null)
				{
					especialidadExistente.Nombre = especialidad.Nombre;
					especialidadExistente.Descripcion = especialidad.Descripcion;
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteEspecialidad(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var especialidad = _dbContext.Especialidades.Find(id);
				if (especialidad != null)
				{
					_dbContext.Especialidades.Remove(especialidad);
					_dbContext.SaveChanges();
				}
			}
		}

		#endregion


		/**********************************************************/
		/**                 Articulos                            **/
		/**********************************************************/
		#region FUNCTIONES ARTICULOS

		public List<Articulo> GetArticulos()
		{
			using (var _dbContext = new DBContext())
			{
				return _dbContext.Articulos.Include(a => a.Copagos)
					.Select(a => new Articulo
					{
						Id = a.Id,
						Nombre = a.Nombre,
						Copagos = a.Copagos.Select(c => new Copago
						{
							Id = c.Id,
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
						}).ToList()
					}).ToList();
			}
		}

		public Articulo GetArticuloById(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var articulo = _dbContext.Articulos.Include(a => a.Copagos).FirstOrDefault(a => a.Id == id);

				if (articulo != null)
				{
					return new Articulo
					{
						Id = articulo.Id,
						Nombre = articulo.Nombre,
						Copagos = articulo.Copagos.Select(c => new Copago
						{
							Id = c.Id,
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
						}).ToList()
					};
				}
				return null;
			}
		}

		public void AddArticulo(Articulo articulo)
		{
			using (var _dbContext = new DBContext())
			{
				var nuevoArticulo = new Articulos
				{
					Nombre = articulo.Nombre,
					Copagos = nameof(articulo.Copagos) != null ? articulo.Copagos.Select(c => new Copagos
					{
						SeguroMedicoId = c.SeguroMedico.Id,
						EspecialidadId = c.Especialidad.Id,
						Precios = c.Precios.Select(p => new Precios
						{
							PrecioBase = p.PrecioBase,
							FechaInicio = p.FechaInicio
						}).ToList()
					}).ToList() : new List<Copagos>()
				};
				_dbContext.Articulos.Add(nuevoArticulo);
				_dbContext.SaveChanges();
			}
		}

		public void UpdateArticulo(Articulo articulo)
		{
			using (var _dbContext = new DBContext())
			{
				var articuloExistente = _dbContext.Articulos.Find(articulo.Id);
				if (articuloExistente != null)
				{
					articuloExistente.Nombre = articulo.Nombre;
					articuloExistente.Copagos = articulo.Copagos.Select(c => new Copagos
					{
						SeguroMedicoId = c.SeguroMedico.Id,
						EspecialidadId = c.Especialidad.Id,
						ArticuloId = c.Id,
						Precios = c.Precios.Select(p => new Precios
						{
							PrecioBase = p.PrecioBase,
							FechaInicio = p.FechaInicio
						}).ToList()
					}).ToList();
					_dbContext.SaveChanges();
				}
			}
		}

		public void DeleteArticulo(long id)
		{
			using (var _dbContext = new DBContext())
			{
				var articulo = _dbContext.Articulos.Find(id);
				if (articulo != null)
				{
					_dbContext.Articulos.Remove(articulo);
					_dbContext.SaveChanges();
				}
			}
		}

		public List<Articulo> GetArticulosFiltrados(string filtro)
		{
			using (var _dbContext = new DBContext())
			{
				var query = _dbContext.Articulos.AsQueryable();

				if (!string.IsNullOrEmpty(filtro))
				{
					query = query.Where(m => m.Nombre.Contains(filtro));
				}

				return query
					.Select(a => new Articulo
					{
						Id = a.Id,
						Nombre = a.Nombre,
						Copagos = a.Copagos.Select(c => new Copago
						{
							Id = c.Id,
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
						}).ToList()
					}).ToList();
			}
		}

        #endregion

        /**********************************************************/
        /**                 PayPalPago                           **/
        /**********************************************************/
        #region FUNCTIONES PAYPALPAGO

		public List<PagoPayPal> GetPaypalPagos()
		{
            using (var _dbContext = new DBContext())
			{
                return _dbContext.PagosPayPal
                    .Select(f => new PagoPayPal
                    {
                        Id = f.Id,
                        linkPago = f.linkPago,
                        pagoId = f.pagoId
                    }).ToList();
            }
        }

        public PagoPayPal GetPaypalPagoById(long id)
        {
            using (var _dbContext = new DBContext())
            {
                var pago = _dbContext.PagosPayPal
                    .Where(p => p.Id == id)
                    .Select(f => new PagoPayPal
                    {
                        Id = f.Id,
                        linkPago = f.linkPago,
                        pagoId = f.pagoId
                    })
                    .FirstOrDefault();

                return pago;
            }
        }

        public void AddPaypalPago(PagoPayPal nuevoPago)
        {
            using (var _dbContext = new DBContext())
            {
                var pago = new PagosPayPal // Asegúrate de usar la entidad de tu modelo de base de datos
                {
                    linkPago = nuevoPago.linkPago,
                    pagoId = nuevoPago.pagoId
                };

                _dbContext.PagosPayPal.Add(pago); // Agrega el registro al contexto
                _dbContext.SaveChanges();         // Guarda los cambios en la base de datos
            }
        }

        #endregion
    }
}
