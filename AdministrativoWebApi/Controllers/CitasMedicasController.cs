using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasMedicasController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public CitasMedicasController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<CitasMedicasController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getCitasMedicas());
        }

        // GET api/<CitasMedicasController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(CitaMedica), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var Cita = _blAdministrativo.getCitaMedicaById(id);
            if (Cita == null)
            {
                return NotFound();
            }
            return Ok(Cita);
        }

        // POST api/<CitasMedicasController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(CitaMedica), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] CitaMedica CitaMedica)
        {
            if (CitaMedica == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addCitaMedica(CitaMedica);
            return CreatedAtAction(nameof(Get), new { id = CitaMedica.Id }, CitaMedica);
        }

        // PUT api/<CitasMedicasController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] CitaMedica citaMedica)
        {
            if (citaMedica == null || citaMedica.Id != id)
            {
                return BadRequest();
            }

            var existingCita = _blAdministrativo.getCitaMedicaById(id);
            if (existingCita == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateCitaMedica(citaMedica);
            return NoContent();
        }

        // DELETE api/<CitasMedicasController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var Cita = _blAdministrativo.getCitaMedicaById(id);
            if (Cita == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteCitaMedica(id);
            return NoContent();
        }
    }
}
