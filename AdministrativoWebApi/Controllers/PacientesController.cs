using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PacientesController : ControllerBase
	{
		private readonly IBL_Administrativo _blAdministrativo;

		public PacientesController(IBL_Administrativo blAdministrativo)
		{
			_blAdministrativo = blAdministrativo;
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

		// POST api/<PacienteController>
		[ProducesResponseType(typeof(Paciente), 201)]
		[HttpPost]
		public IActionResult Post([FromBody] Paciente paciente)
		{
			if (paciente == null)
			{
				return BadRequest();
			}
			paciente.CitasMedicas = [];
			paciente.Facturas = [];
			paciente.Notificaciones = [];
			paciente.Contrato = null;

			_blAdministrativo.addPaciente(paciente);
			return CreatedAtAction(nameof(Get), new { id = paciente.Id }, paciente);
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
	}
}
