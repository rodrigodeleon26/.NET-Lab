using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;
using System.Net;

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
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet("especialidad/{espec}/{pag}/{fecha}")]
        public IActionResult Get(string espec, int pag, DateTime fecha)
        {
            return Ok(_blCitasMedicas.getCitasMedicasPorEspecialidad(espec, pag, fecha));
        }

        // TRAE TODAS LAS CITAS MEDICAS DE UNA ESPECIALIDAD. Formato:
        // GET: api/<CitasMedicasController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(bool), 200)]
        [HttpGet("conteo/{espec}/{pag}/{fecha}")]
        public IActionResult Get(string espec, DateTime fecha, int pag)
        {
            return Ok(_blCitasMedicas.HayMasCitasMedicas(espec, pag, fecha));
        }

        // TRAE TODAS LAS CITAS MEDICAS. Formato:
        // GET: api/<CitasMedicasController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blCitasMedicas.getCitasMedicas());
        }

        // TRAE UNA CITA MEDICA. Formato:
        // GET api/<CitasMedicasController>/[idCita]
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
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
        [Authorize(Roles = "Admin, Medico")]
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

        //OBTENER PAGINADO DE LAS CITAS MEDICAS DE UN PACIENTE
        //GET api/<CitasMedicasController>/paciente/[idPaciente]
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet("paciente/{idPaciente}")]
        public IActionResult GetCitasMedicasByPacienteId(long idPaciente, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, string especialidadesIds)
        {
            var especialidadesList = especialidadesIds.Split(',').Select(long.Parse).ToList();

            return Ok(_blCitasMedicas.GetCitasMedicasByPacienteId(idPaciente, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesList));
        }

        //OBTENER EL CONTEO DE LAS CITAS MEDICAS DE UN PACIENTE
        //GET api/<CitasMedicasController>/cant/[idPaciente]
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(int), 200)]
        [HttpGet("cant/{idPaciente}")]
        public IActionResult CountCitasMedicasByPacienteId(long idPaciente, DateTime? fechaInicio, DateTime? fechaFin, string orden, string especialidadesIds)
        {
            var especialidadesList = especialidadesIds.Split(',').Select(long.Parse).ToList();

            return Ok(_blCitasMedicas.CountCitasMedicasByPacienteId(idPaciente, fechaInicio, fechaFin, orden, especialidadesList));
        }

        //OBTENER CITAS AGENDADAS DEL PACIENTE 
        //GET api/<CitasMedicasController>/paciente/[id]
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet("paciente/{id}/misCitas")]
        public IActionResult GetCitasMedicasAgendadas(long id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            return Ok(_blCitasMedicas.GetCitasMedicasAgendadas(id));
        }

        //Cancelar cita
        //DELETE api/<CitasMedicasController>/[idCita]/paciente[documento]
        [Authorize(Roles = "Admin, Medico, Paciente")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}/paciente/{documento}")]
        public IActionResult CancelarCita(string documento, long id)
        {
            var citaExistente = _blCitasMedicas.getCitaMedicaById(id);
            if (citaExistente == null)
            {
                return NotFound();
            }

            return Ok(_blCitasMedicas.CancelarCita(documento, id));
        }

        //POST api/<CitasMedicasController>/agendar
        [HttpPost("agendar")]
        [ProducesResponseType(typeof(CitaMedica), 201)]
        [ProducesResponseType(400)]
        public IActionResult AgendarCita([FromBody] Request_DatosAgendarCita datosCita)
        {
            if (datosCita == null)
            {
                return BadRequest();
            }

            string cedula = datosCita.Cedula;
            long calendarioId = datosCita.CalendarioId;
            string fecha = datosCita.fecha;
            string hora = datosCita.hora;
            long articuloId = datosCita.ArticuloId;

            if (cedula == null || calendarioId == 0 || fecha == null || hora == null || articuloId == 0)
            {
                return BadRequest();
            }

            //chquear que sea el mismo usuario
            var dniUsuarioAutenticado = User.Claims.FirstOrDefault(c => c.Type == "cedula")?.Value;

            if (dniUsuarioAutenticado == null || dniUsuarioAutenticado != cedula)
            {
                return Forbid("No agendarte por otro usuario");
            }

            // dados los datos en formato fecha:"2024-11-26" hora:"08:00:00" darles formato para que quede DateTime
            var fechaHora = fecha + " " + hora;
            var fechaHoraDateTime = DateTime.Parse(fechaHora);

            Paciente paciente = _blCitasMedicas.getPacienteByCedula(cedula);
            if (paciente.Contrato == null || !paciente.Contrato.Activo)
            {
                return BadRequest("El paciente no tiene un contrato activo");
            }

            SeguroMedico seguro = paciente.Contrato.SeguroMedico;
            Calendario calendario = _blCitasMedicas.getCalendarioById(calendarioId);

            Copago copagoSearch = new Copago()
            {
                SeguroMedico = new SeguroMedico() { Id = seguro.Id },
                Especialidad = new Especialidad() { Id = calendario.Especialidad.Id },
                Articulo = new Articulo() { Id = articuloId },
            };

            long copagoId = _blCitasMedicas.getCopagoBySeguroEspecialidadArticulo(copagoSearch);

            CitaMedica nuevaCita = new CitaMedica
            {
                Fecha = fechaHoraDateTime,
                Estado = "Agendada",
                CalendarioId = calendarioId,
                CopagoId = copagoId,
            };

            var citaCreada = _blCitasMedicas.createCitaMedica(nuevaCita, calendarioId, paciente.Id);
            return CreatedAtAction(nameof(Get), new { id = citaCreada.Id }, citaCreada);
        }
    }
}