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

        // TRAE TODAS LAS CITAS MEDICAS DE UNA ESPECIALIDAD. Formato:
        // GET: api/<CitasMedicasController>
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet("especialidad/{espec}/{pag}/{fecha}")]
        public IActionResult Get(string espec, int pag, DateTime fecha)
        {
            return Ok(_blCitasMedicas.getCitasMedicasPorEspecialidad(espec, pag, fecha));
        }

        // TRAE TODAS LAS CITAS MEDICAS DE UNA ESPECIALIDAD. Formato:
        // GET: api/<CitasMedicasController>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpGet("conteo/{espec}/{pag}/{fecha}")]
        public IActionResult Get(string espec, DateTime fecha, int pag)
        {
            return Ok(_blCitasMedicas.HayMasCitasMedicas(espec, pag, fecha));
        }

        // TRAE TODAS LAS CITAS MEDICAS. Formato:
        // GET: api/<CitasMedicasController>
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blCitasMedicas.getCitasMedicas());
        }

        // TRAE UNA CITA MEDICA. Formato:
        // GET api/<CitasMedicasController>/[idCita]
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

        // CREA UNA CITA MEDICA. Formato:
        // POST api/<CitasMedicasController>/[idCalendario]/[idPaciente] (con el objeto en el body, sobre todo para la fecha)
        [HttpPost("{calendarioId}/{pacienteId}")]
        [ProducesResponseType(typeof(CitaMedica), 201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] CitaMedica nuevaCita, long calendarioId, long pacienteId)
        {
            if (nuevaCita == null)
            {
                return BadRequest();
            }
            var citaCreada = _blCitasMedicas.createCitaMedica(nuevaCita, calendarioId, pacienteId);
            return CreatedAtAction(nameof(Get), new { id = citaCreada.Id }, citaCreada);
        }

        // CAMBIA EL ESTADO. Formato:
        // PUT api/<CitasMedicasController>/[idCita]/[estado]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/{estado}")]
        public IActionResult EditarEstado(int id, string estado)
        {

            var citaExistente = _blCitasMedicas.getCitaMedicaById(id);
            if (citaExistente == null)
            {
                return NotFound();
            }
            citaExistente.Estado = estado;
            _blCitasMedicas.updateCitaMedica(citaExistente);

            return NoContent();
        }

        // ACTUALIZA UNA CITA MEDICA. Formato:
        // PUT api/<CitasMedicasController>/[idCita] (con el objeto en el body)
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CitaMedicaDTO citaActualizada)
        {
            if (citaActualizada == null)
            {
                return BadRequest();
            }

            var citaExistente = _blCitasMedicas.getCitaMedicaById(id);
            if (citaExistente == null)
            {
                return NotFound();
            }

            citaActualizada.Id = id;

            _blCitasMedicas.updateCitaMedica(citaActualizada);
            return NoContent();
        }

        // ELIMINA UNA CITA MEDICA DE LA BASE DE DATOS. Formato:
        // DELETE api/<CitasMedicasController>/[idCita]
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