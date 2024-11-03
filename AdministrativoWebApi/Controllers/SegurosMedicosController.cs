using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurosMedicosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public SegurosMedicosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<SegurosMedicosController>
        [ProducesResponseType(typeof(List<SeguroMedico>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getSegurosMedicos());
        }

        // GET api/<SegurosMedicosController>/5
        [ProducesResponseType(typeof(SeguroMedico), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var seguroMedico = _blAdministrativo.getSeguroMedicoById(id);
            if (seguroMedico == null)
            {
                return NotFound();
            }
            return Ok(seguroMedico);
        }

        // POST api/<SegurosMedicosController>
        [ProducesResponseType(typeof(SeguroMedico), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] SeguroMedico seguroMedico)
        {
            if (seguroMedico == null)
            {
                return BadRequest();
            }
            seguroMedico.Contratos = null;
            seguroMedico.Copagos = null;
            seguroMedico.Precios = null;

            _blAdministrativo.addSeguroMedico(seguroMedico);
            return CreatedAtAction(nameof(Get), new { id = seguroMedico.Id }, seguroMedico);
        }

        // PUT api/<SegurosMedicosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] SeguroMedico seguroMedico)
        {
            if (seguroMedico == null || seguroMedico.Id != id)
            {
                return BadRequest();
            }

            var existingSeguro = _blAdministrativo.getSeguroMedicoById(id);
            if (existingSeguro == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateSeguroMedico(seguroMedico);
            return NoContent();
        }

        // DELETE api/<SegurosMedicosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var seguro = _blAdministrativo.getSeguroMedicoById(id);
            if (seguro == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteSeguroMedico(id);
            return NoContent();
        }
    }
}
