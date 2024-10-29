using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;

namespace CitaMedicaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {
        private readonly IBL_CitasMedicas _blCitasMedicas;

        public CalendarioController(IBL_CitasMedicas blCitasMedicas)
        {
            _blCitasMedicas = blCitasMedicas;
        }

        // GET: api/Calendario
        [HttpGet]
        [ProducesResponseType(typeof(List<Calendario>), 200)]
        public IActionResult Get()
        {
            var calendarios = _blCitasMedicas.GetCalendarios();
            return Ok(calendarios);
        }

        // GET api/Calendario/calendarioId
        [HttpGet("{calendarioId}")]
        [ProducesResponseType(typeof(Calendario), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(long calendarioId) {
            var calendario = _blCitasMedicas.GetCalendarioById(calendarioId);
            if (calendario == null)
            {
                return NotFound();
            }
            return Ok(calendario);
        }

        // GET api/Calendario/medicoId/especialidadId
        [HttpGet("{medicoId}/{especialidadId}")]
        [ProducesResponseType(typeof(Calendario), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(long medicoId, long especialidadId)
        {
            var calendario = _blCitasMedicas.GetCalendarioByMedicoEspecialidad(medicoId, especialidadId);
            if (calendario == null)
            {
                return NotFound();
            }
            return Ok(calendario);
        }

        // POST api/Calendario
        [HttpPost("{medicoId}/{especialidadId}")]
        [ProducesResponseType(typeof(Calendario), 201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] Calendario nuevoCalendario, long medicoId, long especialidadId)
        {
            if (nuevoCalendario == null)
            {
                return BadRequest();
            }

            var calendarioCreado = _blCitasMedicas.CreateCalendario(nuevoCalendario, medicoId, especialidadId);
            return CreatedAtAction(nameof(Get), new { medicoId = medicoId, especialidadId = especialidadId }, calendarioCreado);
        }

        // PUT api/Calendario/medicoId/especialidadId
        [HttpPut("{medicoId}/{especialidadId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put(long medicoId, long especialidadId, [FromBody] Calendario calendarioActualizado)
        {
            if (calendarioActualizado == null)
            {
                return BadRequest();
            }

            var calendarioExistente = _blCitasMedicas.GetCalendarioByMedicoEspecialidad(medicoId, especialidadId);
            if (calendarioExistente == null)
            {
                return NotFound();
            }

            _blCitasMedicas.UpdateCalendario(calendarioActualizado, medicoId, especialidadId);
            return NoContent();
        }

        // DELETE api/Calendario/medicoId/especialidadId
        [HttpDelete("{medicoId}/{especialidadId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long medicoId, long especialidadId)
        {
            var calendarioExistente = _blCitasMedicas.GetCalendarioByMedicoEspecialidad(medicoId, especialidadId);
            if (calendarioExistente == null)
            {
                return NotFound();
            }

            _blCitasMedicas.DeleteCalendario(medicoId, especialidadId);
            return NoContent();
        }
    }
}
