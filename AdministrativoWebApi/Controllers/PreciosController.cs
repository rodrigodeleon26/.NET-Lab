using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreciosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;
        private readonly ILogger<PreciosController> _logger;

        public PreciosController(IBL_Administrativo blAdministrativo, ILogger<PreciosController> logger)
        {
            _blAdministrativo = blAdministrativo;
            _logger = logger;
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
        public IActionResult Get(long id)
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
            _logger.LogInformation("Se ha llegado a la función Post del controlador PreciosController.");

            if (precio == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addPrecio(precio);
            return CreatedAtAction(nameof(Get), new { id = precio.Id }, precio);
        }

        // PUT api/<PreciosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Precio precio)
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
        public IActionResult Delete(long id)
        {
            _logger.LogInformation("FUNCION");

            var precio = _blAdministrativo.getPrecioById(id);
            _logger.LogInformation("OBTUBO");

            if (precio == null)
            {
                return NotFound();
            }
            _logger.LogInformation("PASO EL IF.");

            _blAdministrativo.deletePrecio(id);
            return NoContent();
        }
    }
}
