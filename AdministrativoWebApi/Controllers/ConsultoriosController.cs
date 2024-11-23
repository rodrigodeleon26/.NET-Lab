using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultoriosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public ConsultoriosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<ConsultoriosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Consultorio>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getConsultorios());
        }

        // GET api/<ConsultoriosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Consultorio), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var consultorio = _blAdministrativo.getConsultorioById(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return Ok(consultorio);
        }

        // POST api/<ConsultoriosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Consultorio), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Consultorio consultorio)
        {
            if (consultorio == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addConsultorio(consultorio);
            return CreatedAtAction(nameof(Get), new { id = consultorio.Id }, consultorio);
        }

        // PUT api/<ConsultoriosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Consultorio consultorio)
        {
            if (consultorio == null || consultorio.Id != id)
            {
                return BadRequest();
            }

            var existingC = _blAdministrativo.getConsultorioById(id);
            if (existingC == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateConsultorio(consultorio);
            return NoContent();
        }

        // DELETE api/<ConsultoriosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var consultorio = _blAdministrativo.getConsultorioById(id);
            if (consultorio == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteConsultorio(id);
            return NoContent();
        }
    }
}
