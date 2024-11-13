using BL.IBLs;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

public class AddPacienteRequest
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

        public PacientesController(IBL_Administrativo blAdministrativo, UserManager<AppUsers> userManager, DBContext dbContext)
        {
            _blAdministrativo = blAdministrativo;
            _userManager = userManager;
            _db = dbContext;
        }

        // GET: api/<PacienteController>
        [ProducesResponseType(typeof(List<Paciente>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getPacientes());
        }

        // GET api/<PacienteController>/5
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

        [ProducesResponseType(typeof(Paciente), 201)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddPacienteRequest pacienteRequest)
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

                // Formatear el documento
                pacienteRequest.Documento = FormatearDocumento(pacienteRequest.Documento);

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

                user.Paciente = _db.Pacientes.Find(paciente.Id);
                var password = GenerateRandomPassword(8);
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        code = "Error al crear usuario",
                        description = "No se pudo crear el usuario"
                    });
                }

                await _userManager.AddToRoleAsync(user, "PACIENTE");

                // Crear el contrato
                var contrato = new Contrato
                {
                    FechaInicio = DateTime.UtcNow,
                    Activo = false,
                    SeguroMedico = seguroMedico,
                    Paciente = nuevoPaciente
                };

                _blAdministrativo.addContrato(contrato);

                return CreatedAtAction(nameof(Post), new { id = paciente.Id }, paciente);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { code = ex.Code, description = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                return StatusCode(500, new { code = "Error Interno", description = "Ocurrió un error inesperado" });
            }
        }


        // PUT api/<PacienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Paciente paciente)
        {
            if (paciente == null || paciente.Id != id)
            {
                return BadRequest();
            }

            var existingPaciente = _blAdministrativo.getPacienteById(id);
            if (existingPaciente == null)
            {
                return NotFound();
            }

            _blAdministrativo.updatePaciente(paciente);
            return NoContent();
        }

        // DELETE api/<PacienteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var paciente = _blAdministrativo.getPacienteById(id);
            if (paciente == null)
            {
                return NotFound();
            }

            _blAdministrativo.deletePaciente(id);
            return NoContent();
        }

        [ProducesResponseType(typeof(List<Paciente>), 200)]
        [ProducesResponseType(204)]
        [HttpGet("filtradosPaginados")]
        public IActionResult GetFiltradosPaginados([FromQuery] int pag = 1, [FromQuery] string filtro = "")
        {
            var pacientes = _blAdministrativo.GetPacientesFiltradosPaginados(pag, filtro);
            if (pacientes == null || pacientes.Count == 0)
            {
                return NoContent();
            }
            return Ok(pacientes);
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string FormatearDocumento(string documento)
        {
            if (documento.Length == 8)
            {
                return $"{documento.Substring(0, 1)}.{documento.Substring(1, 3)}.{documento.Substring(4, 3)}-{documento.Substring(7, 1)}";
            }
            return documento;
        }
        private void ValidarPacienteRequest(AddPacienteRequest request)
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

    }
}
