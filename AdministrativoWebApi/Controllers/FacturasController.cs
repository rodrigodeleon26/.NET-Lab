using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public FacturasController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<FacturasController>
        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getFacturas());
        }

        // GET: api/<FacturasController>/pagina/1
        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet("pagina/{pag}")]
        public IActionResult GetFacturasPaginadas(
        int pag,
        [FromQuery] string? pacienteString = null,
        [FromQuery] bool fechaAsc = false,
        [FromQuery] bool? estaPago = null)
        {
            var facturas = _blAdministrativo.getFacturasPaginadas(pag, pacienteString, fechaAsc, estaPago);
            return Ok(facturas);
        }

        // GET api/<FacturasController>/5
        [ProducesResponseType(typeof(Factura), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var factura = _blAdministrativo.getFacturaById(id);
            if (factura == null)
            {
                return NotFound();
            }
            return Ok(factura);
        }

        // POST api/<FacturasController>
        [ProducesResponseType(typeof(Factura), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Factura factura)
        {
            if (factura == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addFactura(factura);
            return CreatedAtAction(nameof(Get), new { id = factura.Id }, factura);
        }

        // PUT api/<FacturasController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Factura factura)
        {
            if (factura == null || factura.Id != id)
            {
                return BadRequest();
            }

            var _factura = _blAdministrativo.getFacturaById(id);
            if (_factura == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateFactura(factura);
            return NoContent();
        }

        // DELETE api/<FacturasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var factura = _blAdministrativo.getFacturaById(id);
            if (factura == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteFactura(id);
            return NoContent();
        }
    }
}
