using BL.BLs;
using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Globalization;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class CalendariosController : ControllerBase
	{
		private readonly ILogger<CalendariosController> _logger;
		private readonly IBL_Administrativo _blAdministrativo;

		public CalendariosController(IBL_Administrativo blAdministrativo, ILogger<CalendariosController> logger)
		{
			_blAdministrativo = blAdministrativo;
			_logger = logger;
		}

        // GET: api/<CalendariosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Calendario>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getCalendarios());
		}

        // GET api/<CalendariosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Calendario), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var calendario = _blAdministrativo.getCalendarioById(id);
			if (calendario == null)
			{
				return NotFound();
			}
			return Ok(calendario);
		}

        // POST api/<CalendariosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Calendario), 201)]
		[HttpPost]
		public IActionResult Post([FromBody] Calendario calendario)
		{
			if (calendario == null)
			{
				return BadRequest();
			}
			calendario.CitasMedicas = [];

			_blAdministrativo.addCalendario(calendario);
			return CreatedAtAction(nameof(Get), new { id = calendario.Id }, calendario);
		}

        // PUT api/<CalendariosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
		public IActionResult Put(long id, [FromBody] Calendario calendario)
		{
			if (calendario == null || calendario.Id != id)
			{
				return BadRequest();
			}

			var existingC = _blAdministrativo.getCalendarioById(id);
			if (existingC == null)
			{
				return NotFound();
			}

			_blAdministrativo.updateCalendario(calendario);
			return NoContent();
		}

        // DELETE api/<CalendariosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var calendario = _blAdministrativo.getCalendarioById(id);
			if (calendario == null)
			{
				return NotFound();
			}

			_blAdministrativo.deleteCalendario(id);
			return NoContent();
		}

        // POST api/<CalendariosController>/crearCalendario
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("crearCalendario")]
		public IActionResult crearCalendario([FromBody] Request_CrearCalendario request)
		{
			long medId = request.MedicoId;
			long espId = request.EspecialidadId;
			long conId = request.ConsultorioId;
			TimeSpan horaInicio = request.HoraInicio;
			TimeSpan horaFin = request.HoraFin;
			int tiempo = request.Tiempo;
			int cant = request.Cantidad;
			string[]? dias = request.Dias;

			if (medId == 0 || espId == 0 || conId == 0 || tiempo <= 0 || cant <= 0 || dias == null || dias.Length == 0)
			{
				return BadRequest();
			}

			_blAdministrativo.crearCalendario(medId, espId, conId, horaInicio, horaFin, tiempo, cant, dias);

			return NoContent();
		}

        // GET api/<CalendariosController>/medico/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpGet("medico/{id}")]
		public IActionResult GetCalendariosMedico(long id)
		{
			return Ok(_blAdministrativo.getCalendarios().Where(c => c.Medico.Id == id));
		}

        // POST api/<CalendariosController>/checkOcupacionConsultorio
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("checkOcupacionConsultorio")]
		public IActionResult checkOcupacionConsultorio([FromBody] Calendario calendario)
		{
			if (calendario == null)
			{
				return BadRequest();
			}

			bool ocupado = _blAdministrativo.checkOcupacionConsultorio(calendario);

			return Ok(ocupado);
		}


		// POST api/<CalendariosController>/validarCalendariosPropios/3/5
		[HttpPost("validarCalendariosPropios/{medicoId}/{calendarioEditId}")]
        public IActionResult validarCalendariosPropios(long medicoId, long calendarioEditId, [FromBody] Calendario calendario)
		{
			if (calendario == null || medicoId == 0)
			{
				return BadRequest();
            }

			bool valido = _blAdministrativo.validarCalendariosPropios(medicoId, calendarioEditId, calendario);
			return Ok(valido);
        }

        // POST api/<CalendariosController>/validarEspecialidadesParaBorrar/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("validarEspecialidadesParaBorrar/{medicoId}")]
		public IActionResult validarEspecialidadesParaBorrar(long medicoId, [FromBody] List<Especialidad> especialidades)
		{
			bool valido = _blAdministrativo.validarEspecialidadesParaBorrar(medicoId, especialidades);

			return Ok(valido);
		}

        // POST api/<CalendariosController>/borrarCalendariosIncompatibles/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("borrarCalendariosIncompatibles/{medicoId}")]
		public async Task<IActionResult> borrarCalendariosIncompatiblesAsync(long medicoId, [FromBody] List<Especialidad> especialidades)
		{
			await _blAdministrativo.borrarCalendariosIncompatiblesAsync(medicoId, especialidades);
            return NoContent();
        }

        // POST api/<CalendariosController>/filtrarCalendarios/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("filtrarCalendarios/{medicoId}")]
        [ProducesResponseType(typeof(List<Calendario>), 200)]
        public IActionResult filtrarCalendarios(long medicoId, [FromBody] String[] filtros)
		{
			string filtroEspecialidad = filtros[0];
            string filtroDia = filtros[1];
            string filtroHoraInicio = filtros[2];

			if ( filtroEspecialidad == null || filtroDia == null || filtroHoraInicio == null)
			{
				return BadRequest();
            }

			return Ok(_blAdministrativo.getCalendariosFiltrados(medicoId, filtroEspecialidad, filtroDia, filtroHoraInicio));
        }

        //get calendarios por articulo para una fecha
        // GET api/<CalendariosController>/articulo/5/fecha/2024-11-24
        [HttpGet("{cedula}/articulo/{articuloId}/fecha/{fecha}")]
        public IActionResult getCalendariosByArticuloFecha(string cedula, long articuloId, string fecha)
        {
            if (fecha == null || articuloId == 0 || cedula == null)
            {
                return BadRequest();
            }

            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != cedula)
            {
                return Forbid("No puedes ver la informacion de otro usuario");
            }

            return Ok(_blAdministrativo.getCalendariosByArticuloFecha(cedula, articuloId, fecha));
        }

        //get calendarios por especialidad para una fecha
        // GET api/<CalendariosController>/especialidad/5/fecha/2024-11-24
        [HttpGet("{cedula}/especialidad/{especialidadId}/fecha/{fecha}")]
        public IActionResult GetCalendariosByEspecialidadYFecha(string cedula, long especialidadId, string fecha)
        {
            if (fecha == null || especialidadId == 0)
            {
                return BadRequest();
            }

            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != cedula)
            {
                return Forbid("No puedes ver la informacion de otro usuario");
            }

            return Ok(_blAdministrativo.GetCalendariosByEspecialidadYFecha(especialidadId, fecha));
        }
    }
}
