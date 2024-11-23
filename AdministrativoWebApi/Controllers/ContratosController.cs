using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContratosController : ControllerBase
	{
		private readonly IBL_Administrativo _blAdministrativo;
		private readonly ILogger<ContratosController> _logger;

		public ContratosController(IBL_Administrativo blAdministrativo, ILogger<ContratosController> logger)
		{
			_blAdministrativo = blAdministrativo;
			_logger = logger;
		}

        // GET: api/<ContratosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Contrato>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getContratos());
		}

        // GET api/<ContratosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Contrato), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var contrato = _blAdministrativo.getContratoById(id);
			if (contrato == null)
			{
				return NotFound();
			}
			return Ok(contrato);
		}

        // POST api/<ContratosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Contrato), 201)]
		[HttpPost]
		public IActionResult Post([FromBody] Contrato contrato)
		{
			if (contrato == null)
			{
				return BadRequest();
			}

			_blAdministrativo.addContrato(contrato);
			return CreatedAtAction(nameof(Get), new { id = contrato.Id }, contrato);
		}

        // PUT api/<ContratosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
		public IActionResult Put(long id, [FromBody] Contrato contrato)
		{
			if (contrato == null || contrato.Id != id)
			{
				return BadRequest();
			}

			var existingContrato = _blAdministrativo.getContratoById(id);
			if (existingContrato == null)
			{
				return NotFound();
			}

			_blAdministrativo.updateContrato(contrato);
			return NoContent();
		}

        // DELETE api/<ContratosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var contrato = _blAdministrativo.getContratoById(id);
			if (contrato == null)
			{
				return NotFound();
			}

			_blAdministrativo.deleteContrato(id);
			return NoContent();
		}


        // POST api/<ContratosController>/contratar-seguro
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("/contratar-seguro")]
		public IActionResult ContratarSeguro([FromBody] Request_ContratarSeguro request)
		{
			_logger.LogInformation("Entro a la funcion");

			var paciente = _blAdministrativo.getPacienteById(request.IdPaciente);
			var seguro = _blAdministrativo.getSeguroMedicoById(request.IdSeguroMedico);

			if (paciente == null)
			{
				return BadRequest("El paciente no existe");
			}
			if (seguro == null)
			{
				return BadRequest("El seguro no existe");
			}

			_blAdministrativo.ContratarSeguroMedico(request.IdPaciente, request.IdSeguroMedico);
			return NoContent();
		}

        // POST api/<ContratosController>/activar-contrato
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("/activar-contrato")]
		public IActionResult ActivarContrato([FromBody] long idContrato)
        {
            var contrato = _blAdministrativo.getContratoById(idContrato);

            if (contrato == null)
            {
                return BadRequest("El contrato no existe");
            }

            _blAdministrativo.activarContrato(idContrato);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Contrato>), 200)]
        [ProducesResponseType(204)]
        [HttpGet("filtradosPaginados")]
        public IActionResult GetContratosFiltradosPaginados([FromQuery] int pag = 1, [FromQuery] string filtro = "")
        {
            var contratos = _blAdministrativo.GetContratosFiltradosPaginados(pag, filtro);
            if (contratos == null || contratos.Count == 0)
            {
                return NoContent();
            }
            return Ok(contratos);
        }
    }
}
