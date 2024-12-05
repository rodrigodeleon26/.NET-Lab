using BL.IBLs;
using BL.BLs;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Services;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

public class PacienteRequest
{
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Documento { get; set; }
    public DateOnly FechaDeNacimiento { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public long SeguroMedicoId { get; set; }
}

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;
        private readonly UserManager<AppUsers> _userManager;
        private readonly DBContext _db;
        private readonly IEmailService emailService;
        private readonly PayPalService _payPalService;

        public PacientesController(IBL_Administrativo blAdministrativo, UserManager<AppUsers> userManager, DBContext dbContext, PayPalService payPalService)
        {
            _blAdministrativo = blAdministrativo;
            _userManager = userManager;
            _payPalService = payPalService;
            _db = dbContext;
            emailService = new EmailService();
        }

        // GET: api/<PacienteController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Paciente>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getPacientes());
        }

        // GET api/<PacienteController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Paciente), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var paciente = _blAdministrativo.getPacienteById(id);
			if (paciente == null)
			{
				return NotFound();
			}
			return Ok(paciente);
		}

        // GET api/<PacienteController>/12345678
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(Paciente), 200)]
        [HttpGet("dni/{dni}")]
        public IActionResult Get(string dni)
        {
            var paciente = _blAdministrativo.getPacienteByDNI(dni);
            if (paciente == null)
            {
                return NotFound();
            }
            return Ok(paciente);
        }

        // POST api/<PacienteController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Paciente), 201)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PacienteRequest pacienteRequest)
        {
            try
            {
                ValidarPacienteRequest(pacienteRequest);

                if (_blAdministrativo.cedulaDuplicada(pacienteRequest.Documento))
                {
                    return BadRequest(new
                    {
                        code = "Documento Duplicado",
                        description = $"El paciente con documento {pacienteRequest.Documento} ya tiene un usuario asociado"
                    });
                }

                if (_blAdministrativo.emailDuplicado(pacienteRequest.Email))
                {
                    return BadRequest(new
                    {
                        code = "Email duplicado",
                        description = $"El paciente con email {pacienteRequest.Email} ya tiene un usuario asociado"
                    });
                }
                
                var seguroMedico = _blAdministrativo.getSeguroMedicoById(pacienteRequest.SeguroMedicoId);
                if (seguroMedico == null)
                {
                    return BadRequest(new
                    {
                        code = "Seguro médico no encontrado",
                        description = "No se encontró el seguro médico"
                    });
                }

                var paciente = new Paciente
                {
                    Nombres = pacienteRequest.Nombres,
                    Apellidos = pacienteRequest.Apellidos,
                    Documento = pacienteRequest.Documento,
                    Direccion = pacienteRequest.Direccion,
                    Telefono = pacienteRequest.Telefono,
                    Email = pacienteRequest.Email,
                    FechaDeNacimiento = pacienteRequest.FechaDeNacimiento
                };

                _blAdministrativo.addPaciente(paciente);

                var nuevoPaciente = _blAdministrativo.getPacienteByDNI(pacienteRequest.Documento);
                if (nuevoPaciente == null)
                {
                    return BadRequest(new
                    {
                        code = "Paciente no encontrado",
                        description = "No se pudo encontrar el paciente recién creado"
                    });
                }

                // Crear el usuario de aplicación
                AppUsers user = new AppUsers
                {
                    Email = pacienteRequest.Email,
                    UserName = pacienteRequest.Email,
                    FullName = $"{pacienteRequest.Nombres.ToUpper()} {pacienteRequest.Apellidos.ToUpper()}"
                };

                user.Paciente = _db.Pacientes.Find(nuevoPaciente.Id);
                Console.WriteLine("antes hash");
                var password = GenerateRandomPassword(8);
                Console.WriteLine("hash" + password);
                var result = await _userManager.CreateAsync(user, password);
                Console.WriteLine("resultl" + result.ToString);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        code = "Error al crear usuario",
                        description = "No se pudo crear el usuario"
                    });
                }

                await _userManager.AddToRoleAsync(user, "PACIENTE");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedEmail = WebUtility.UrlEncode(user.Email);
                var encodedToken = WebUtility.UrlEncode(token);
                var resetLink = $"https://localhost:4200/resetPassword?email={encodedEmail}&token={encodedToken}";

                // Construir el mensaje de bienvenida
                var htmlMessage = $@"
                <h1>Bienvenido a SistemaHCE</h1>
                <p>Hola {user.FullName},</p>
                <p>Tu cuenta ha sido creada exitosamente. Por favor, haz clic en el siguiente enlace para establecer tu contraseña:</p>
                <a href='{resetLink}'>Establecer Contraseña</a>
                <p>Si no puedes hacer clic en el enlace, copia y pega la siguiente URL en tu navegador:</p>
                <p>{resetLink}</p>
                <p>Recuerda que debes pagar la primer cuota de tu seguro médico para activar el mismo.</p>
                <p>Saludos,</p>
                <p>El equipo de SistemaHCE</p>";

                // Enviar el correos
                await emailService.SendEmailAsync(user.Email, "Bienvenido a SistemaHCE - Establece tu Contraseña", htmlMessage);

                // Crear el contrato
                var contrato = new Contrato
                {
                    FechaInicio = DateTime.UtcNow,
                    Activo = false,
                    SeguroMedico = seguroMedico,
                    Paciente = nuevoPaciente
                };
                _blAdministrativo.addContrato(contrato);

                Precio precio = _blAdministrativo.GetPrecioBySeguro(pacienteRequest.SeguroMedicoId);
                

                // Llamar al PayPalService para crear el pago
                PayPalOrderResponse order = await _payPalService.CreateOrderAsync(
                    new List<PayPalPurchaseUnit>
                    {
                        new PayPalPurchaseUnit
                        {
                            reference_id = paciente.Id.ToString(),
                            description = "Cuota inicial: Pago de seguro médico",
                            amount = new PayPalAmount
                            {
                                currency_code = "USD",
                                value = precio.PrecioBase.ToString()
                            }
                        }
                    },
                    "USD",
                    "https://localhost:4200/cliente/payment/success",
                    "https://localhost:4200/cliente/payment/cancel"
                );

                var approvalUrl = order.links.FirstOrDefault(link => link.rel == "approve")?.href;
                if (approvalUrl == null)
                {
                    return BadRequest("No se pudo obtener la URL de aprobación.");
                }

                var nuevoPago = new PagoPayPal
                {
                    linkPago = approvalUrl,
                    pagoId = order.id
                };

                _blAdministrativo.AddPaypalPago(nuevoPago);

                var pagoCreado = _blAdministrativo.GetPaypalPagoByOrdenId(order.id);

                var factura = new Factura
                {
                    Fecha = DateTime.Now,
                    Monto = precio.PrecioBase,
                    Descripcion = "Cuota inicial: Pago de seguro médico",
                    FechaPago = null,
                    Pago = false,
                    Paciente = nuevoPaciente,
                    PagoPayPal = pagoCreado
                };


                _blAdministrativo.addFactura(factura);
                Console.WriteLine("Sale del add factura");

                return CreatedAtAction(nameof(Post), new { id = paciente.Id }, paciente);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { code = ex.Code, description = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "No hay InnerException";
                return StatusCode(500, new
                {
                    code = "Error Interno",
                    description = ex.Message,
                    innerException = innerExceptionMessage
                });
            }
        }

        // PUT api/<PacienteController>/5
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] PacienteRequest pacienteRequest)
        {
            try
            {
                ValidarPacienteRequest(pacienteRequest);

                var pacienteExistente = _blAdministrativo.getPacienteById(id);
                if (pacienteExistente == null)
                {
                    return NotFound(new
                    {
                        code = "Paciente no encontrado",
                        description = "No se encontró el paciente con el ID proporcionado"
                    });
                }

                if (pacienteExistente.Documento != pacienteRequest.Documento && _blAdministrativo.cedulaDuplicada(pacienteRequest.Documento))
                {
                    return BadRequest(new
                    {
                        code = "Documento Duplicado",
                        description = $"El paciente con documento {pacienteRequest.Documento} ya tiene un usuario asociado"
                    });
                }

                if (pacienteExistente.Email != pacienteRequest.Email && _blAdministrativo.emailDuplicado(pacienteRequest.Email))
                {
                    return BadRequest(new
                    {
                        code = "Email duplicado",
                        description = $"El paciente con email {pacienteRequest.Email} ya tiene un usuario asociado"
                    });
                }

                pacienteExistente.Nombres = pacienteRequest.Nombres;
                pacienteExistente.Apellidos = pacienteRequest.Apellidos;
                pacienteExistente.Documento = pacienteRequest.Documento;
                pacienteExistente.Direccion = pacienteRequest.Direccion;
                pacienteExistente.Telefono = pacienteRequest.Telefono;
                pacienteExistente.Email = pacienteRequest.Email;
                pacienteExistente.FechaDeNacimiento = pacienteRequest.FechaDeNacimiento;

                _blAdministrativo.updatePaciente(pacienteExistente);

                return Ok(pacienteExistente);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { code = ex.Code, description = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                return StatusCode(500, new { code = "Error Interno", description = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var paciente = _blAdministrativo.getPacienteById(id);
                if (paciente == null)
                {
                    return NotFound(new
                    {
                        code = "Paciente no encontrado",
                        description = "No se encontró el paciente con el ID proporcionado"
                    });
                }

                _blAdministrativo.deletePaciente(id);

                return Ok(new
                {
                    code = "Paciente Eliminado",
                    description = "El paciente fue eliminado exitosamente"
                });
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                return StatusCode(500, new { code = "Error Interno", description = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Paciente>), 200)]
        [ProducesResponseType(204)]
        [HttpGet("filtradosPaginados")]
        public IActionResult GetFiltradosPaginados([FromQuery] int pag = 1, [FromQuery] string filtro = "")
        {
            var pacientes = _blAdministrativo.GetPacientesFiltradosPaginados(pag, filtro);
            return Ok(pacientes);
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void ValidarPacienteRequest(PacienteRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Nombres) ||
                string.IsNullOrEmpty(request.Apellidos) ||
                string.IsNullOrEmpty(request.Documento) ||
                string.IsNullOrEmpty(request.Direccion) ||
                string.IsNullOrEmpty(request.Telefono) ||
                string.IsNullOrEmpty(request.Email))
            {
                throw new ValidationException("Datos incorrectos", "Todos los campos son obligatorios");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(request.Documento, @"^\d{8}$"))
            {
                throw new ValidationException("Documento inválido", "El documento debe tener 8 dígitos");
            }

            DateOnly fechaMinima = new DateOnly(1920, 1, 1);
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);
            if (request.FechaDeNacimiento < fechaMinima || request.FechaDeNacimiento >= fechaActual)
            {
                throw new ValidationException("Fecha de nacimiento inválida", "La fecha de nacimiento debe ser mayor a 01/01/1920 y menor a la fecha actual");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(request.Telefono, @"^\d{8,9}$"))
            {
                throw new ValidationException("Teléfono inválido", "El teléfono debe tener 8 o 9 dígitos");
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(request.Email);
                if (addr.Address != request.Email)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new ValidationException("Email inválido", "El email no tiene un formato válido");
            }
        }

        public class ValidationException : Exception
        {
            public string Code { get; }

            public ValidationException(string code, string message) : base(message)
            {
                Code = code;
            }
        }

        // GET api/<PacienteController>/12345678/notificaciones
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<Notificacion>), 200)]
        [HttpGet("{id}/notificaciones")]
        public IActionResult Get(long id, int pageNumber, int pageSize)
        {
			if (pageNumber < 1 || pageSize < 1 || id == null)
			{
				return BadRequest();
			}
			return Ok(_blAdministrativo.getNotificaciones(id, pageNumber, pageSize));	
        }

        // GET api/<PacienteController>/5/notificaciones/count
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(int), 200)]
		[HttpGet("{id}/notificaciones/count")]
		public IActionResult GetCount(long id)
		{
			if (id == null)
			{
				return BadRequest();
			}
			return Ok(_blAdministrativo.CountNotificaciones(id));
		}

        //GET api/<PacienteController>/5/historialFacturacion
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet("{id}/historialFacturacion")]
        public IActionResult GetHistorialFacturacion(long id, int pageNumber, int pageSize)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (pageNumber < 1 || pageSize < 1 || id == null)
            {
                return BadRequest();
            }
            return Ok(_blAdministrativo.getHistorialFacturacion(id, pageNumber, pageSize));
        }

        //GET api/<PacienteController>/5/historialFacturacion/count
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet("{id}/historialFacturacion/count")]
        public IActionResult GetCantFacturas(long id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            return Ok(_blAdministrativo.countFacturas(id));
        }
    }
}