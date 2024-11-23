using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    public class Request_CambiarContrato
    {
        public long IdContratoActual { get; set; }
        public long IdNuevoSeguroMedico { get; set; }
    }

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
		[ProducesResponseType(typeof(List<Contrato>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getContratos());
		}

		// GET api/<ContratosController>/5
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
		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var contrato = _blAdministrativo.getContratoById(id);
			if (contrato == null)
			{
				return NotFound();
			}

			if(contrato.Activo == false)
            {
                return BadRequest("El contrato ya está dado de baja");
            }

            contrato.Activo = false;
            _blAdministrativo.updateContrato(contrato);
            return Ok(new { message = "El contrato ha sido dado de baja" });
        }


        //      // POST api/<ContratosController>/contratar-seguro
        //      [HttpPost("/contratar-seguro")]
        //public IActionResult ContratarSeguro([FromBody] Request_ContratarSeguro request)
        //{
        //	_logger.LogInformation("Entro a la funcion");

        //	var paciente = _blAdministrativo.getPacienteById(request.IdPaciente);
        //	var seguro = _blAdministrativo.getSeguroMedicoById(request.IdSeguroMedico);

        //	if (paciente == null)
        //	{
        //		return BadRequest("El paciente no existe");
        //	}
        //	if (seguro == null)
        //	{
        //		return BadRequest("El seguro no existe");
        //	}

        //	_blAdministrativo.ContratarSeguroMedico(request.IdPaciente, request.IdSeguroMedico);
        //	return NoContent();
        //}

        // POST api/<ContratosController>/activar-contrato
        [HttpPost("activarContrato/{id}")]
        public IActionResult ActivarContrato(long id)
        {
            var contrato = _blAdministrativo.getContratoById(id);

            if (contrato == null)
            {
                return BadRequest("El contrato no existe");
            }

			if (contrato.Activo == true)
			{
                return BadRequest("El contrato ya está activo");
            }

			if (_blAdministrativo.puedeRenovarContrato(id) == false)
			{
				return BadRequest("El contrato no puede ser activado, hay pagos pendientes");
			}

            contrato.Activo = true;
            contrato.FechaInicio = DateTime.Now;
            _blAdministrativo.updateContrato(contrato);
            return Ok(new { message = "El contrato ha sido activado exitosamente" });
        }

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


		[HttpPost("cambiarContrato")]
		public IActionResult CambiarContrato([FromBody] Request_CambiarContrato request)
		{
			// Validar la existencia del contrato actual
			var contratoActual = _blAdministrativo.getContratoById(request.IdContratoActual);
			if (contratoActual == null)
			{
				return BadRequest("El contrato actual no existe");
			}

			// Validar la existencia del nuevo seguro médico
			var nuevoSeguroMedico = _blAdministrativo.getSeguroMedicoById(request.IdNuevoSeguroMedico);
			if (nuevoSeguroMedico == null)
			{
				return BadRequest("El nuevo seguro médico no existe");
			}

			if (nuevoSeguroMedico.Id == contratoActual.SeguroMedico.Id)
			{
				return BadRequest("El nuevo seguro médico es el mismo que el actual");
			}

			contratoActual.SeguroMedico = nuevoSeguroMedico;
			contratoActual.Activo = true;
			contratoActual.FechaInicio = DateTime.Now;
            _blAdministrativo.updateContrato(contratoActual);

            return Ok(new { message = "El contrato ha sido actualizado exitosamente" });
        }
    }
}
