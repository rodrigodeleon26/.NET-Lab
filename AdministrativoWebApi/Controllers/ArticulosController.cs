using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticulosController : ControllerBase
	{
		private readonly IBL_Administrativo _blAdministrativo;

		public ArticulosController(IBL_Administrativo blAdministrativo)
		{
			_blAdministrativo = blAdministrativo;
		}

        // GET: api/<ArticulosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Articulo>), 200)]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_blAdministrativo.getArticulos());
		}

        // GET api/<ArticulosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Articulo), 200)]
		[HttpGet("{id}")]
		public IActionResult Get(long id)
		{
			var articulo = _blAdministrativo.getArticuloById(id);
			if (articulo == null)
			{
				return NotFound();
			}
			return Ok(articulo);
		}

        // POST api/<ArticulosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Articulo), 201)]
		[HttpPost]
		public IActionResult Post([FromBody] Articulo articulo)
		{
			if (articulo == null)
			{
				return BadRequest();
			}

			_blAdministrativo.addArticulo(articulo);
			return CreatedAtAction(nameof(Get), new { id = articulo.Id }, articulo);
		}

        // PUT api/<ArticulosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
		public IActionResult Put(long id, [FromBody] Articulo articulo)
		{
			if (articulo == null || articulo.Id != id)
			{
				return BadRequest();
			}

			var existingA = _blAdministrativo.getArticuloById(id);
			if (existingA == null)
			{
				return NotFound();
			}

			_blAdministrativo.updateArticulo(articulo);
			return NoContent();
		}

        // DELETE api/<ArticulosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var articulo = _blAdministrativo.getArticuloById(id);
			if (articulo == null)
			{
				return NotFound();
			}

			_blAdministrativo.deleteArticulo(id);
			return NoContent();
		}

        // GET api/<ArticulosController>/filtro/{filtro}
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Articulo>), 200)]
		[HttpGet("filtro/{filtro}")]
		public IActionResult Get(string filtro)
        {
            return Ok(_blAdministrativo.getArticulosFiltrados(filtro));
        }

	}
}
