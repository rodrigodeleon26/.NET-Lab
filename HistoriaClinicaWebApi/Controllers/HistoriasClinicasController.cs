using Shared;
using Microsoft.AspNetCore.Mvc;
using BL.IBLs;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace HistoriaClinicaWebApi.Controllers


{
    [Route("api/ConsultaMedica/[controller]")]
    [ApiController]
    public class HistoriasClinicasController : ControllerBase
    {
        private readonly IBL_HistoriasClinicas _blHistoriasClinicas;
        private readonly ILogger<HistoriasClinicasController> _logger;

        public HistoriasClinicasController(IBL_HistoriasClinicas blHistoriasClinicas, ILogger<HistoriasClinicasController> logger)
        {
            _blHistoriasClinicas = blHistoriasClinicas;
            _logger = logger;
        }

        // GET: api/<HistoriasClinicasController>
        [ProducesResponseType(typeof(List<ConsultaMedica>), 200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public IActionResult Get()
        {
            var consultasMedicas = _blHistoriasClinicas.getConsultasMedicas();
            if (consultasMedicas.Count == 0)
            {
                return NotFound(new { Message = "No hay consultas médicas" });
            }
            return Ok(consultasMedicas);
        }

        // GET api/<HistoriasClinicasController>/5
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var consultaMedica = _blHistoriasClinicas.getConsultaMedica(id);
            if (consultaMedica == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedica);
        }

        // POST api/<HistoriasClinicasController>
        [ProducesResponseType(typeof(ConsultaMedica), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Post(ConsultaMedica consultaMedica)
        {
            if (consultaMedica == null)
            {
                return BadRequest(new { Message = "La consulta médica no puede ser nula" });
            }
            var consultaMedicaCreada = _blHistoriasClinicas.createConsultaMedica(consultaMedica);
            return CreatedAtAction(nameof(Get), new { id = consultaMedicaCreada.Id }, consultaMedicaCreada);
        }

        //POST api/<HistoriasClinicasController>/Simple
        [ProducesResponseType(typeof(ConsultaMedica), 201)]
        [ProducesResponseType(400)]
        [HttpPost("Simple")]
        public IActionResult PostSimple(ConsultaMedica consultaMedica)
        {
            if (consultaMedica == null)
            {
                return BadRequest(new { Message = "La consulta médica no puede ser nula" });
            }
            var consultaMedicaCreada = _blHistoriasClinicas.createConsultaMedicaSimple(consultaMedica);
            return CreatedAtAction(nameof(Get), new { id = consultaMedicaCreada.Id }, consultaMedicaCreada);
        }


        // PUT api/<HistoriasClinicasController>/5
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ConsultaMedica consultaMedica)
        {
            if (consultaMedica == null)
            {
                return BadRequest(new { Message = "La consulta médica no puede ser nula" });
            }
            if (id != consultaMedica.Id)
            {
                return BadRequest(new { Message = "El ID de la consulta médica no coincide con el ID de la URL" });
            }
            var consultaMedicaActualizada = _blHistoriasClinicas.updateConsultaMedica(consultaMedica);
            if (consultaMedicaActualizada == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedicaActualizada);
        }

        // POST api/<HistoriasClinicasController>/5/Receta
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPost("{id}/Receta")]
        public IActionResult Post(int id, [FromBody] Receta receta)
        {
            if (receta == null)
            {
                return BadRequest(new { Message = "La receta no puede ser nula" });
            }
            var consultaMedicaConReceta = _blHistoriasClinicas.addReceta(id, receta);
            if (consultaMedicaConReceta == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedicaConReceta);
        }

        // PUT api/<HistoriasClinicasController>/5/Receta
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/Receta")]
        public IActionResult Put(int id, [FromBody] Receta receta)
        {
            if (receta == null)
            {
                return BadRequest(new { Message = "La receta no puede ser nula" });
            }
            var consultaMedicaConRecetaActualizada = _blHistoriasClinicas.updateReceta(id, receta);
            if (consultaMedicaConRecetaActualizada == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedicaConRecetaActualizada);
        }

        // DELETE api/<HistoriasClinicasController>/5/Receta/1
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}/Receta/{idReceta}")]
        public IActionResult DeleteReceta(int id, int idReceta)
        {
            var consultaMedicaSinReceta = _blHistoriasClinicas.deleteReceta(id, idReceta);
            if (consultaMedicaSinReceta == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID o no existe receta con ese ID" });
            }
            return Ok(consultaMedicaSinReceta);
        }

        // POST api/<HistoriasClinicasController>/5/Estudio
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPost("{id}/Estudio")]
        public IActionResult Post(int id, [FromBody] Estudio estudio)
        {
            if (estudio == null)
            {
                return BadRequest(new { Message = "El estudio no puede ser nulo" });
            }
            var consultaMedicaConEstudio = _blHistoriasClinicas.addEstudio(id, estudio);
            if (consultaMedicaConEstudio == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedicaConEstudio);
        }

        // PUT api/<HistoriasClinicasController>/5/Estudio
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/Estudio")]
        public IActionResult Put(int id, [FromBody] Estudio estudio)
        {
            if (estudio == null)
            {
                return BadRequest(new { Message = "El estudio no puede ser nulo" });
            }
            var consultaMedicaConEstudioActualizado = _blHistoriasClinicas.updateEstudio(id, estudio);
            if (consultaMedicaConEstudioActualizado == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }
            return Ok(consultaMedicaConEstudioActualizado);
        }

        // DELETE api/<HistoriasClinicasController>/5/Estudio/1
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}/Estudio/{idEstudio}")]
        public IActionResult DeleteEstudio(int id, int idEstudio) {
            var consultaMedicaSinEstudio = _blHistoriasClinicas.deleteEstudio(id, idEstudio);
            if (consultaMedicaSinEstudio == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID o no existe estudio con ese ID" });
            }
            return Ok(consultaMedicaSinEstudio);
        }

        // POST api/<HistoriasClinicasController>/5/Estudio/1/Resultado
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPost("{id}/Estudio/{idEstudio}/Resultado")]
        public IActionResult Post(int id, int idEstudio, string resultado, DateOnly fechaResultado)
        {
            if (resultado == null)
            {
                return BadRequest(new { Message = "El resultado no puede ser nulo" });
            }
            if (fechaResultado == null)
            {
                return BadRequest(new { Message = "La fecha del resultado no puede ser nula" });
            }
            var consultaMedicaConResultadoEstudio = _blHistoriasClinicas.addResultadoEstudio(id, idEstudio, resultado, fechaResultado);
            if (consultaMedicaConResultadoEstudio == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID o no existe estudio con ese ID" });
            }
            return Ok(consultaMedicaConResultadoEstudio);
        }
    }
}
