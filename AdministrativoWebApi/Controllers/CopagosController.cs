using BL.IBLs;
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
		[ProducesResponseType(typeof(List<Copago>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getCopagos());
		}

		// GET api/<CopagosController>/5
		[ProducesResponseType(typeof(Copago), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var copago = _blAdministrativo.getCopagoById(id);
			if (copago == null)
			{
				return NotFound();
			}
			return Ok(copago);
		}

		// POST api/<CopagosController>
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
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] Copago copago)
        {
            if (copago == null || copago.Id != id)
            {
                return BadRequest();
            }

            var existingPrecio = _blAdministrativo.getPrecioById(id);
            if (existingPrecio == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateCopago(copago);
            return Ok(copago);
        }

		// DELETE api/<CopagosController>/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
        {
            var precio = _blAdministrativo.getPrecioById(id);
            if (precio == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteCopago(id);
            return NoContent();
        }
	}
}
