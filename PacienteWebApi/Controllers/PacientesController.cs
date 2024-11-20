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

        //GET: api/<PacientesController>/54321987/miHistoriaClinica
        [Authorize(Roles = "Paciente")]
        [ProducesResponseType(typeof(ConsultaMedicaCompletaDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [HttpGet("{dni}/miHistoriaClinica")]
        public IActionResult Get(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, string especialidades)
        {
            // Extraer el dni del token del usuario autenticado
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

    }
}