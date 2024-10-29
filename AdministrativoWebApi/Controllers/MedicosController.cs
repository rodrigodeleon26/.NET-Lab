using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public MedicosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<MedicosController>
        [ProducesResponseType(typeof(List<Medico>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getMedicos());
        }

        // GET api/<MedicosController>/5
        [ProducesResponseType(typeof(Medico), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var Medico = _blAdministrativo.getMedicoById(id);
            if (Medico == null)
            {
                return NotFound();
            }
            return Ok(Medico);
        }

        // POST api/<MedicosController>
        [ProducesResponseType(typeof(Medico), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Medico Medico)
        {
            if (Medico == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addMedico(Medico);
            return CreatedAtAction(nameof(Get), new { id = Medico.Id }, Medico);
        }

        // PUT api/<MedicosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Medico Medico)
        {
            if (Medico == null || Medico.Id != id)
            {
                return BadRequest();
            }

            var existingMedico = _blAdministrativo.getMedicoById(id);
            if (existingMedico == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateMedico(Medico);
            return NoContent();
        }

        // DELETE api/<MedicosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var Medico = _blAdministrativo.getMedicoById(id);
            if (Medico == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteMedico(id);
            return NoContent();
        }

        // POST api/<MedicosController>/asignarEspecialidad/5/3
        [HttpPost("asignarEspecialidad/{medId}/{espId}")]
        public IActionResult AsignarEspecialidad(long medId, long espId)
        {
            if(medId == 0 || espId == 0)
            {
                return BadRequest();
            }
            _blAdministrativo.asignarEspecialidad(medId, espId);
            return NoContent();
        }
    }
}
