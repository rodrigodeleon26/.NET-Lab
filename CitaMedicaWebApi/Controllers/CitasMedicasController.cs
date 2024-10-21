using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacienteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasMedicasController : ControllerBase
    {
        private readonly IBL_CitasMedicas _blCitasMedicas;

        public CitasMedicasController(IBL_CitasMedicas blCitasMedicas)
        {
            _blCitasMedicas = blCitasMedicas;
        }

        // GET: api/<CitasMedicasController>
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blCitasMedicas.getCitasMedicas());
        }

        // GET api/<CitasMedicasController>/5
        [ProducesResponseType(typeof(CitaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cita = _blCitasMedicas.getCitaMedicaById(id);
            if (cita == null)
            {
                return NotFound();
            }
            return Ok(cita);
        }

        // POST api/<CitasMedicasController>
        [HttpPost("{calendarioId}")]
        [ProducesResponseType(typeof(CitaMedica), 201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] CitaMedica nuevaCita, long calendarioId)
        {
            if (nuevaCita == null)
            {
                return BadRequest();
            }
            var citaCreada = _blCitasMedicas.createCitaMedica(nuevaCita, calendarioId);
            return CreatedAtAction(nameof(Get), new { id = citaCreada.Id }, citaCreada);
        }

        // PUT api/<CitasMedicasController>/5
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CitaMedica citaActualizada)
        {
            if (citaActualizada == null)
            {
                return BadRequest();
            }

            // Verificar si la cita médica existe por su ID
            var citaExistente = _blCitasMedicas.getCitaMedicaById(id);
            if (citaExistente == null)
            {
                return NotFound();
            }

            // Asignar el ID recibido en la URL al objeto citaActualizada
            citaActualizada.Id = id;

            // Actualizar la cita médica
            _blCitasMedicas.updateCitaMedica(citaActualizada);
            return NoContent();
        }

        // DELETE api/<CitasMedicasController>/5
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var citaExistente = _blCitasMedicas.getCitaMedicaById(id);
            if (citaExistente == null)
            {
                return NotFound();
            }

            _blCitasMedicas.deleteCitaMedica(id);
            return NoContent();
        }
    }
}