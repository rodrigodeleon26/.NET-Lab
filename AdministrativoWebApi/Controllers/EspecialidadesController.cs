using BL.IBLs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public EspecialidadesController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<EspecialidadesController>
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<Especialidad>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getEspecialidades());
        }

        // GET api/<EspecialidadesController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Especialidad), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var especialidad = _blAdministrativo.getEspecialidadById(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            return Ok(especialidad);
        }

        // POST api/<EspecialidadesController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Especialidad), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Especialidad especialidad)
        {
            if (especialidad == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addEspecialidad(especialidad);
            return CreatedAtAction(nameof(Get), new { id = especialidad.Id }, especialidad);
        }

        // PUT api/<EspecialidadesController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Especialidad especialidad)
        {
            if (especialidad == null || especialidad.Id != id)
            {
                return BadRequest();
            }

            var existingE = _blAdministrativo.getEspecialidadById(id);
            if (existingE == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateEspecialidad(especialidad);
            return NoContent();
        }

        // DELETE api/<EspecialidadesController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var especialidad = _blAdministrativo.getEspecialidadById(id);
            if (especialidad == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteEspecialidad(id);
            return NoContent();
        }

        // GET api/<EspecialidadesController>/EspecialidadesHabilitados/{cedula}
        [ProducesResponseType(typeof(List<Especialidad>), 200)]
        [HttpGet("EspecialidadesHabilitados/{cedula}")]
        public IActionResult GetEspecialidadesHabilitados(string cedula)
        {
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != cedula)
            {
                return Forbid("No puedes ver la informacion de otro usuario");
            }

            if (cedula == null)
            {
                return BadRequest();
            }
            return Ok(_blAdministrativo.getEspecialidadesHabilitadas(cedula));
        }
    }
}
