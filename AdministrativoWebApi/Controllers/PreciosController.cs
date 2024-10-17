using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreciosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public PreciosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<PreciosController>
        [ProducesResponseType(typeof(List<Precio>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getPrecios());
        }

        // GET api/<PreciosController>/5
        [ProducesResponseType(typeof(Precio), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var precio = _blAdministrativo.getPrecioById(id);
            if (precio == null)
            {
                return NotFound();
            }
            return Ok(precio);
        }

        // POST api/<PreciosController>
        [ProducesResponseType(typeof(Precio), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Precio precio)
        {
            if (precio == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addPrecio(precio);
            return CreatedAtAction(nameof(Get), new { id = precio.Id }, precio);
        }

        // PUT api/<PreciosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Precio precio)
        {
            if (precio == null || precio.Id != id)
            {
                return BadRequest();
            }

            var existingPrecio = _blAdministrativo.getPrecioById(id);
            if (existingPrecio == null)
            {
                return NotFound();
            }

            _blAdministrativo.updatePrecio(precio);
            return NoContent();
        }

        // DELETE api/<PreciosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var precio = _blAdministrativo.getPrecioById(id);
            if (precio == null)
            {
                return NotFound();
            }

            _blAdministrativo.deletePrecio(id);
            return NoContent();
        }
    }
}
