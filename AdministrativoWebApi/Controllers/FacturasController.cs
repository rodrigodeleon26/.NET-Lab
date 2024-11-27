using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Factura>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getFacturas());
        }

        [HttpGet("paypal/{paypalOrderId}")]
        public IActionResult GetFacturasByPaypal(string paypalOrderId)
        {
            var facturas = _blAdministrativo.GetFacturasByPaypal(paypalOrderId);

            if (facturas == null || !facturas.Any())
            {
                return NotFound("No se encontraron facturas para el ID de PayPal proporcionado.");
            }

            return Ok(facturas);
        }

        // GET: api/<FacturasController>/pagina/1
        [Authorize(Roles = "Admin, Medico, Paciente")]
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
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Factura), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Factura factura)
        {
            if (factura == null)
            {
                Console.WriteLine("Factura es null");
                return BadRequest();
            }
            Console.WriteLine("En el controlador");
            _blAdministrativo.addFactura(factura);
            return CreatedAtAction(nameof(Get), new { id = factura.Id }, factura);
        }

        // PUT api/<FacturasController>/5
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
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

        [Authorize(Roles = "Admin, Medico")]
        [HttpGet("pdf/{id}")]
        public IActionResult GetFacturaPdf(long id)
        {
            try
            {
                var pdfStream = _blAdministrativo.GenerarFactura(id);

                if (pdfStream == null)
                {
                    return NotFound();
                }

                // Devuelve el PDF como archivo descargable
                return File(pdfStream, "application/pdf", $"Factura_{DateTime.Now}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("pdf/lista")]
        public IActionResult GetMultipleFacturasPdf([FromBody] List<long> ids)
        {
            try
            {
                var pdfStream = _blAdministrativo.GenerarFacturaListada(ids);

                if (pdfStream == null)
                {
                    return NotFound("No se encontraron facturas para los IDs proporcionados.");
                }

                // Devuelve el PDF como archivo descargable
                return File(pdfStream, "application/pdf", $"Facturas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
