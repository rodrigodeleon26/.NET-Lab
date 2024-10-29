using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;

namespace PacienteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IBL_CitasMedicas _blCitasMedicas;

        public MedicosController(IBL_CitasMedicas blCitasMedicas)
        {
            _blCitasMedicas = blCitasMedicas;
        }

        // GET: Medico/api/<MedicosController>
        [ProducesResponseType(typeof(List<Medico>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blCitasMedicas.GetMedicos());
        }

        // GET Medico/api/<MedicosController>/5
        [ProducesResponseType(typeof(Medico), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var medico = _blCitasMedicas.GetMedicoById(id);
            if (medico == null)
            {
                return NotFound();
            }
            return Ok(medico);
        }

        // POST Medico/api/<MedicosController>
        [ProducesResponseType(typeof(Medico), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Post([FromBody] Medico nuevoMedico)
        {
            if (nuevoMedico == null)
            {
                return BadRequest();
            }
            var medicoCreado = _blCitasMedicas.CreateMedico(nuevoMedico);
            return CreatedAtAction(nameof(Get), new { id = medicoCreado.Id }, medicoCreado);
        }

        // PUT Medico/api/<MedicosController>/5
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Medico medicoActualizado)
        {
            if (medicoActualizado == null)
            {
                return BadRequest();
            }

            // Verificar si el médico existe por su ID
            var medicoExistente = _blCitasMedicas.GetMedicoById(id);
            if (medicoExistente == null)
            {
                return NotFound();
            }

            // Asignar el ID recibido en la URL al objeto medicoActualizado
            medicoActualizado.Id = id;

            // Actualizar el médico
            _blCitasMedicas.UpdateMedico(medicoActualizado);
            return NoContent();
        }

        // DELETE Medico/api/<MedicosController>/5
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var medicoExistente = _blCitasMedicas.GetMedicoById(id);
            if (medicoExistente == null)
            {
                return NotFound();
            }

            _blCitasMedicas.DeleteMedico(id);
            return NoContent();
        }
    }
}