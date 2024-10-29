using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;

namespace CitaMedicaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadController : ControllerBase
    {
        private readonly IBL_CitasMedicas _blCitasMedicas;

        public EspecialidadController(IBL_CitasMedicas blCitasMedicas)
        {
            _blCitasMedicas = blCitasMedicas;
        }

        // GET: api/Especialidad
        [HttpGet]
        [ProducesResponseType(typeof(List<Especialidad>), 200)]
        public IActionResult Get()
        {
            var especialidades = _blCitasMedicas.GetEspecialidades();
            return Ok(especialidades);
        }

        // GET api/Especialidad/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Especialidad), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(long id)
        {
            var especialidad = _blCitasMedicas.GetEspecialidadById(id);
            if (especialidad == null)
            {
                return NotFound();
            }
            return Ok(especialidad);
        }

        // POST api/Especialidad
        [HttpPost]
        [ProducesResponseType(typeof(Especialidad), 201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] Especialidad nuevaEspecialidad)
        {
            if (nuevaEspecialidad == null)
            {
                return BadRequest();
            }

            var especialidadCreada = _blCitasMedicas.CreateEspecialidad(nuevaEspecialidad);
            return CreatedAtAction(nameof(Get), new { id = especialidadCreada.Id }, especialidadCreada);
        }

        // PUT api/Especialidad/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Put(long id, [FromBody] Especialidad especialidadActualizada)
        {
            if (especialidadActualizada == null)
            {
                return BadRequest();
            }

            var especialidadExistente = _blCitasMedicas.GetEspecialidadById(id);
            if (especialidadExistente == null)
            {
                return NotFound();
            }

            especialidadActualizada.Id = id; // Asignar el ID de la URL al objeto actualizado
            _blCitasMedicas.UpdateEspecialidad(especialidadActualizada);
            return NoContent();
        }

        // DELETE api/Especialidad/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(long id)
        {
            var especialidadExistente = _blCitasMedicas.GetEspecialidadById(id);
            if (especialidadExistente == null)
            {
                return NotFound();
            }

            _blCitasMedicas.DeleteEspecialidad(id);
            return NoContent();
        }
    }
}