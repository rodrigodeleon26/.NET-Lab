using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    public class Request_CambiarContrato
    {
        public long IdContratoActual { get; set; }
        public long IdNuevoSeguroMedico { get; set; }
    }

    public class ReactivarContratoRequest
    {
        public int Cuotas { get; set; }
        public int Interes { get; set; }
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

			if (contrato.Activo == false)
			{
				return BadRequest("El contrato ya está dado de baja");
			}

			_blAdministrativo.deleteContrato(id);
			return NoContent();
		}

		[ProducesResponseType(typeof(List<Contrato>), 200)]
		[ProducesResponseType(204)]
		[HttpGet("filtradosPaginados")]
		public IActionResult GetContratosFiltradosPaginados([FromQuery] int pag = 1, [FromQuery] string filtro = "")
		{
			var contratos = _blAdministrativo.GetContratosFiltradosPaginados(pag, filtro);
			return Ok(contratos);
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

			if (_blAdministrativo.puedeRenovarContrato(contratoActual.Id) == false)
			{
				return BadRequest("El contrato no puede ser cambiado, hay pagos pendientes");
			}

			_blAdministrativo.cambiarContrato(contratoActual, nuevoSeguroMedico);

            return Ok(new { message = "El contrato ha sido actualizado exitosamente" });
		}

        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet("{id}/getUltimasFacturas")]
        public IActionResult GetUltimasFacturas(long id)
        {
			var ultimasfacturas = _blAdministrativo.ObtenerUltimasFacturasDelContrato(id, 3);
            var deuda = _blAdministrativo.ObtenerDeudaDeContrato(id);
            return Ok(new { ultimasfacturas, deuda });
        }

        [HttpPost("{id}/reactivarContrato")]
        public IActionResult ReactivarContrato(long id, [FromBody] ReactivarContratoRequest request)
        {
            // Validar el contrato
            var contrato = _blAdministrativo.getContratoById(id);
            if (contrato == null)
            {
                return BadRequest("El contrato no existe");
            }

            if (contrato.Activo)
            {
                return BadRequest("El contrato ya está activo");
            }

            if (request.Cuotas != 6 && request.Cuotas != 12)
            {
                return BadRequest("La cantidad de cuotas debe ser 6 o 12");
            }

            if (request.Interes < 1 || request.Interes > 100)
            {
                return BadRequest("El interés debe estar entre 1 y 100");
            }

            if (_blAdministrativo.contratoEnRefinanciacion(id))
            {
                return BadRequest("El contrato ya está en refinanciación");
            }

            _blAdministrativo.reactivarContrato(id, request.Cuotas, request.Interes);
            return Ok(new { message = "El contrato ha sido reactivado exitosamente" });
        }

        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Contrato>), 200)]
        [ProducesResponseType(204)]
        [HttpGet("filtradosPaginados")]
        public IActionResult GetContratosFiltradosPaginados([FromQuery] int pag = 1, [FromQuery] string filtro = "")
        {
            var contratos = _blAdministrativo.GetContratosFiltradosPaginados(pag, filtro);
            return Ok(contratos);
        }
    }
}
