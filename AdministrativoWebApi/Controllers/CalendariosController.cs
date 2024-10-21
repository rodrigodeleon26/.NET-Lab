using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendariosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public CalendariosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<CalendariosController>
        [ProducesResponseType(typeof(List<Calendario>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getCalendarios());
        }

        // GET api/<CalendariosController>/5
        [ProducesResponseType(typeof(Calendario), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var calendario = _blAdministrativo.getCalendarioById(id);
            if (calendario == null)
            {
                return NotFound();
            }
            return Ok(calendario);
        }

        // POST api/<CalendariosController>
        [ProducesResponseType(typeof(Calendario), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Calendario calendario)
        {
            if (calendario == null)
            {
                return BadRequest();
            }
            calendario.CitasMedicas = [];

            _blAdministrativo.addCalendario(calendario);
            return CreatedAtAction(nameof(Get), new { id = calendario.Id }, calendario);
        }

        // PUT api/<CalendariosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Calendario calendario)
        {
            if (calendario == null || calendario.Id != id)
            {
                return BadRequest();
            }

            var existingC = _blAdministrativo.getCalendarioById(id);
            if (existingC == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateCalendario(calendario);
            return NoContent();
        }

        // DELETE api/<CalendariosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var calendario = _blAdministrativo.getCalendarioById(id);
            if (calendario == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteCalendario(id);
            return NoContent();
        }
    }
}
