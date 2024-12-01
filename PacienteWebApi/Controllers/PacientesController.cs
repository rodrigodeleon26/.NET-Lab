using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacienteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IBL_Pacientes _blPacientes;

        public PacientesController(IBL_Pacientes blPacientes)
        {
            _blPacientes = blPacientes;
        }

        // GET: api/<PacientesController>
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<Paciente>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blPacientes.getPacientes());
        }

        // GET: api/<PacientesController>/53219872/misDatos
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(Paciente), 200)]
        [HttpGet("{dni}/misDatos")]
        public IActionResult Get(string dni)
        {
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes ver la informacion de otro usuario");
            }

            return Ok(_blPacientes.getMisDatos(dni));
        }

        // PUT: api/<PacientesController>/53219872/actualizarDatos
        [Authorize(Roles = "Paciente")]
        [HttpPut("{dni}/actulizarDatos")]
        public IActionResult Put(string dni, Paciente paciente)
        {
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes actualizar la informacion de otro paciente");
            }

            _blPacientes.actualizarDatos(paciente);

            return Ok();
        }

        //GET: api/<PacientesController>/54321987/miHistoriaClinica
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(ConsultaMedicaCompletaDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpGet("{dni}/miHistoriaClinica")]
        public IActionResult Get(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, string especialidades)
        {
            if (dni == null)
            {
                return BadRequest();
            }
            if (pageNumber < 1 || pageSize < 1 || pageNumber == null || pageSize == null)
            {
                return BadRequest();
            }
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes acceder a la historia clínica de otro paciente.");
            }

            // Procesar la lista de especialidades desde el parámetro
            var especialidadesList = JsonConvert.DeserializeObject<List<EspecialidadDTO>>(especialidades);

            // Filtrar las especialidades con checked en true
            var especialidadesIds = especialidadesList.Where(e => e.IsChecked).Select(e => e.Id).ToList();

            var resultado = _blPacientes.GetHistoriaClinica(dni, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);

            if (resultado == null)
            {
                return NotFound(new { Message = "No existe paciente con esa cédula" });
            }

            return Ok(resultado);
        }

        // GET api/<PacienteController>/12345678/notificaciones
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(List<Notificacion>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{dni}/notificaciones")]
        public IActionResult Get(string dni, int pageNumber, int pageSize)
        {
            if (dni == null)
            {
                return BadRequest();
            }
            if (pageNumber < 1 || pageSize < 1 || pageNumber == null || pageSize == null)
            {
                return BadRequest();
            }

            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes acceder a las notificaciones de otro paciente.");
            }

            return Ok(_blPacientes.getNotificaciones(dni, pageNumber, pageSize));
        }

        // PUT api/<PacienteController>/5/notificaciones
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/notificaciones")]
        public IActionResult Put(long id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            return Ok(_blPacientes.notificacionVista(id));
        }

        //GET: api/<PacientesController>/54321987/misCitas
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpGet("{dni}/misCitas")]
        public IActionResult GetCitas(string dni)
        {
            if (dni == null)
            {
                return BadRequest();
            }
            // Extraer el dni del token del usuario autenticado
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes acceder a las citas de otro paciente.");
            }

           return Ok(_blPacientes.getMisCitas(dni));
        }

        //DELETE: api/<PacientesController>/1234567/citas/5/cancelar
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpDelete("{dni}/citas/{id}/cancelarCita")]
        public IActionResult CancelarCita(string dni, long id)
        {
            if (dni == null || id == null)
            {
                return BadRequest();
            }
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes cancelar cita de otro paciente.");
            }

            return Ok(_blPacientes.CancelarCita(dni, id));
        }

        //GET: api/<PacientesController>/54321987/historialFacturacion
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpGet("{dni}/historialFacturacion")]
        public IActionResult GetFacturas(string dni, int pageNumber, int pageSize)
        {
            if (dni == null)
            {
                return BadRequest();
            }
            if (pageNumber < 1 || pageSize < 1 || pageNumber == null || pageSize == null)
            {
                return BadRequest();
            }
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != dni)
            {
                return Forbid("No puedes acceder al historial de facturación de otro paciente.");
            }

            return Ok(_blPacientes.getHistorialFacturacion(dni, pageNumber, pageSize));
        }

        [HttpGet("oauth2callback")]
        public async Task<IActionResult> OAuth2Callback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("El código de autorización es requerido.");
            }

            try
            {
                // `state` contiene el identificador del paciente enviado desde el frontend
                var patientId = state;
                var cedula = await _blPacientes.GetAccessToken(patientId, code);

                var frontendUrl = $"https://localhost:4200/cliente/mis-datos?cedula={cedula}";
                return Redirect(frontendUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el código de autorización: {ex.Message}");
            }
        }

        //PUT: api/<PacientesController>/5/desvincularGoogle
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpPut("{id}/desvincularGoogle")]
        public bool DesvincularGoogle(long id)
        {
            if (id == null)
            {
                return false;
            }
            _blPacientes.DesvincularGoogle(id);
            return true;
        }

    }
}