using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public ContratosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
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

            _blAdministrativo.deleteContrato(id);
            return NoContent();
        }
    }
}
