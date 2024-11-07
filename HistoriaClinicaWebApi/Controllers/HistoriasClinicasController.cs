using Shared;
using Microsoft.AspNetCore.Mvc;
using BL.IBLs;
using DAL.Models;
using Microsoft.Extensions.Logging;
using BL.BLs;
using Newtonsoft.Json;

namespace HistoriaClinicaWebApi.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriasClinicasController : ControllerBase
    {
        private readonly IBL_HistoriasClinicas _blHistoriasClinicas;
        private readonly S3Service _s3Service;

        public HistoriasClinicasController
            (
            IBL_HistoriasClinicas blHistoriasClinicas, 
            S3Service s3Service
            )
        {
            _blHistoriasClinicas = blHistoriasClinicas;
            _s3Service = s3Service;
        }

        // GET: api/<HistoriasClinicasController>
        [ProducesResponseType(typeof(List<ConsultaMedica>), 200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public IActionResult Get()
        {
            var consultasMedicas = _blHistoriasClinicas.getConsultasMedicas();

            // La capa de negocio ahora se encarga de devolver null si no hay consultas
            if (consultasMedicas == null)
            {
                return NotFound(new { Message = "No hay consultas médicas" });
            }
            return Ok(consultasMedicas);
        }

        // GET api/<HistoriasClinicasController>/5
        [ProducesResponseType(typeof(ConsultaMedicaCompletaDTO), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var consultaMedicaCompleta = _blHistoriasClinicas.getConsultaMedicaCompleta(id);

            if (consultaMedicaCompleta == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }

            return Ok(consultaMedicaCompleta);
        }


        //POST api/<HistoriasClinicasController>/
        [ProducesResponseType(typeof(ConsultaMedica), 201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult PostSimple(ConsultaMedicaDTO consultaMedica)
        {
            if (consultaMedica == null)
            {
                return BadRequest(new { Message = "La consulta médica no puede ser nula" });
            }

            var consultaMedicaCreada = _blHistoriasClinicas.createConsultaMedica(consultaMedica);

            return CreatedAtAction(nameof(Get), new { id = consultaMedicaCreada.Id }, consultaMedicaCreada);
        }

        //POST api/<HistoriasClinicasController>/5
        [ProducesResponseType(typeof(ConsultaMedica), 201)]
        [ProducesResponseType(400)]
        [HttpPost("{id}")]
        public IActionResult Post(long id)
        {
            var consultaMedicaCreada = _blHistoriasClinicas.createConsultaMedicaSD(id);
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

        //DELETE api/<HistoriasClinicasController>/5
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var consultaMedicaEliminada = _blHistoriasClinicas.deleteConsultaMedica(id);

            if (consultaMedicaEliminada == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }

            return Ok(consultaMedicaEliminada);
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
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}/Receta/{idReceta}")]
        public IActionResult DeleteReceta(int id, int idReceta)
        {
            var consultaMedicaSinReceta = _blHistoriasClinicas.deleteReceta(id, idReceta);
            if (consultaMedicaSinReceta == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID o no existe receta con ese ID" });
            }
            return Ok(consultaMedicaSinReceta); // O considera retornar NoContent()
        }

        // POST api/<HistoriasClinicasController>/5/Estudio
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPost("{id}/Estudio")]
        public async Task<IActionResult> Post(int id, [FromBody] Estudio estudio)
        {
            if (estudio == null)
            {
                return BadRequest(new { Message = "El estudio no puede ser nulo" });
            }

            var consultaMedicaConEstudio = await _blHistoriasClinicas.addEstudio(id, estudio);
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
        public async Task<IActionResult> Post(int id, int idEstudio, DateOnly fechaResultado, IFormFile imagenEstudio)
        {
            string imagenUrl = null;

            if (imagenEstudio != null && imagenEstudio.Length > 0)
            {
                using var stream = imagenEstudio.OpenReadStream();
                imagenUrl = await _s3Service.UploadFileAsync(stream, $"{Guid.NewGuid()}_{imagenEstudio.FileName}", imagenEstudio.ContentType);
            }

            var consultaMedicaConResultadoEstudio = _blHistoriasClinicas.addResultadoEstudio(id, idEstudio, fechaResultado, imagenUrl);

            if (consultaMedicaConResultadoEstudio == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID o no existe estudio con ese ID" });
            }

            return Ok(consultaMedicaConResultadoEstudio);
        }

        // GET api/<HistoriasClinicasController>/12345678/historiaClinica?pageNumber=1&pageSize=10
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{dni}/historiaClinica")]
        public IActionResult Get(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, string especialidades)
        {
            var especialidadesList = JsonConvert.DeserializeObject<List<EspecialidadDTO>>(especialidades);

            // Filtrar las especialidades con checked en true
            var especialidadesIds = especialidadesList.Where(e => e.IsChecked).Select(e => e.Id).ToList();

            var resultado = _blHistoriasClinicas.GetHistoriaClinica(dni, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);

            if (resultado == null)
            {
                return NotFound(new { Message = "No existe paciente con esa cédula" });
            }

            return Ok(resultado);
        }

        // PUT api/<HistoriasClinicasController>/5/guardarConsulta
        [ProducesResponseType(typeof(ConsultaMedica), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}/guardarConsulta")]
        public IActionResult Put(long id)
        {
            var consultaMedica = _blHistoriasClinicas.GuardarConsulta(id);
            if (consultaMedica == null)
            {
                return NotFound(new { Message = "No existe consulta médica con ese ID" });
            }

            return Ok(consultaMedica);
        }

        //GET api/<HistoriaClinicasController>/Medicamentos
        [ProducesResponseType(typeof(List<Medicamento>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("Medicamentos")]
        public IActionResult GetMedicamentos()
        {
            var medicamentos = _blHistoriasClinicas.getMedicamentos();

            if (medicamentos == null)
            {
                return NotFound(new { Message = "No hay medicamentos" });
            }

            return Ok(medicamentos);
        }
    }
}
