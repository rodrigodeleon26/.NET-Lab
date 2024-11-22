using BL.BLs;
using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Globalization;

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
		[ProducesResponseType(typeof(List<Calendario>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getCalendarios());
		}

		// GET api/<CalendariosController>/5
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
		[HttpGet("medico/{id}")]
		public IActionResult GetCalendariosMedico(long id)
		{
			return Ok(_blAdministrativo.getCalendarios().Where(c => c.Medico.Id == id));
		}

		// POST api/<CalendariosController>/checkOcupacionConsultorio
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
        [HttpPost("validarEspecialidadesParaBorrar/{medicoId}")]
		public IActionResult validarEspecialidadesParaBorrar(long medicoId, [FromBody] List<Especialidad> especialidades)
		{
			bool valido = _blAdministrativo.validarEspecialidadesParaBorrar(medicoId, especialidades);

			return Ok(valido);
		}

        // POST api/<CalendariosController>/borrarCalendariosIncompatibles/5
        [HttpPost("borrarCalendariosIncompatibles/{medicoId}")]
		public async Task<IActionResult> borrarCalendariosIncompatiblesAsync(long medicoId, [FromBody] List<Especialidad> especialidades)
		{
			await _blAdministrativo.borrarCalendariosIncompatiblesAsync(medicoId, especialidades);
            return NoContent();
        }

        //    return this.http.post<any>(`${this.apiUrl}/filtrarCalendarios/${medicoId}`, filtros);

        // POST api/<CalendariosController>/filtrarCalendarios/5
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
    }
}
