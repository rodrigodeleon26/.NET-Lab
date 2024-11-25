using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CopagosController : ControllerBase
	{
		private readonly IBL_Administrativo _blAdministrativo;

		public CopagosController(IBL_Administrativo blAdministrativo)
		{
			_blAdministrativo = blAdministrativo;
		}

        // GET: api/<CopagosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Copago>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getCopagos());
		}

        // GET api/<CopagosController>/5	
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Copago), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var copago = _blAdministrativo.getCopagoById(id);
			if (copago == null)
			{
				return NotFound();
			}
			return Ok(copago);
		}

        // POST api/<CopagosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Copago), 201)]
		[HttpPost]
		public IActionResult Post([FromBody] Copago copago)
		{
			if (copago == null)
			{
				return BadRequest();
			}

			_blAdministrativo.addCopago(copago);
			return CreatedAtAction(nameof(Get), new { id = copago.Id }, copago);
		}

        // PUT api/<CopagosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
		public IActionResult Put(long id, [FromBody] Copago copago)
        {
            if (copago == null || copago.Id != id)
            {
                return BadRequest();
            }

            var existingPrecio = _blAdministrativo.getCopagoById(id);
            if (existingPrecio == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateCopago(copago);
            return Ok(copago);
        }

        // DELETE api/<CopagosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
        {
            var precio = _blAdministrativo.getCopagoById(id);
            if (precio == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteCopago(id);
            return NoContent();
        }

        // GET api/<CopagosController>/getIdByFilds/[seguroMedicoId]/[especialidadId]/[articuloId]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(404)]
        [HttpGet("getIdByFilds/{seguroMedicoId}/{especialidadId}/{articuloId}")]
        public IActionResult GetIdByFilds(long seguroMedicoId, long especialidadId, long articuloId)
        {
			Console.WriteLine("en el controlador");
			Console.WriteLine("seguroMedicoId: " + seguroMedicoId);
            Console.WriteLine("especialidadId: " + especialidadId);
            Console.WriteLine("articuloId: " + articuloId);

            Copago copagoSearch = new Copago()
			{
				SeguroMedico = new SeguroMedico() { Id = seguroMedicoId },
				Especialidad = new Especialidad() { Id = especialidadId },
				Articulo = new Articulo() { Id = articuloId },
			};
            long copagoId = _blAdministrativo.getIdByFilds(copagoSearch);
            if (copagoId == 0)
            {
                return NotFound();
            }
            return Ok(copagoId);
        }
    }
}
