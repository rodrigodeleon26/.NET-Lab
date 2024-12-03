using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdministrativoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IBL_Administrativo _blAdministrativo;

        public MedicosController(IBL_Administrativo blAdministrativo)
        {
            _blAdministrativo = blAdministrativo;
        }

        // GET: api/<MedicosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Medico>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blAdministrativo.getMedicos());
        }

        // GET api/<MedicosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Medico), 200)]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var Medico = _blAdministrativo.getMedicoById(id);
            if (Medico == null)
            {
                return NotFound();
            }
            return Ok(Medico);
        }

        // GET api/<MedicosController>/dni/5
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Medico), 200)]
        [HttpGet("dni/{dni}")]
        public IActionResult Get(string dni)
        {
            var Medico = _blAdministrativo.getMedicoByDocumento(dni);
            if (Medico == null)
            {
                return NotFound();
            }
            return Ok(Medico);
        }

        // POST api/<MedicosController>
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(Medico), 201)]
        [HttpPost]
        public IActionResult Post([FromBody] Medico Medico)
        {
            if (Medico == null)
            {
                return BadRequest();
            }

            _blAdministrativo.addMedico(Medico);
            return CreatedAtAction(nameof(Get), new { id = Medico.Id }, Medico);
        }

        // PUT api/<MedicosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Medico Medico)
        {
            if (Medico == null || Medico.Id != id)
            {
                return BadRequest();
            }

            var existingMedico = _blAdministrativo.getMedicoById(id);
            if (existingMedico == null)
            {
                return NotFound();
            }

            _blAdministrativo.updateMedico(Medico);
            return NoContent();
        }

        // DELETE api/<MedicosController>/5
        [Authorize(Roles = "Admin, Medico")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var Medico = _blAdministrativo.getMedicoById(id);
            if (Medico == null)
            {
                return NotFound();
            }

            _blAdministrativo.deleteMedico(id);
            return NoContent();
        }

        // POST api/<MedicosController>/asignarEspecialidad/5/3
        [Authorize(Roles = "Admin, Medico")]
        [HttpPost("asignarEspecialidad/{medId}/{espId}")]
        public IActionResult AsignarEspecialidad(long medId, long espId)
        {
            if(medId == 0 || espId == 0)
            {
                return BadRequest();
            }
            _blAdministrativo.asignarEspecialidad(medId, espId);
            return NoContent();
        }

        // GET api/<MedicosController>/3?hibber
        [Authorize(Roles = "Admin, Medico")]
        [ProducesResponseType(typeof(List<Medico>), 200)]
        [HttpGet("getMedicosPaginadosYFiltrados/{numPagina}")]
        public IActionResult GetMedicosPaginadosYFiltrados(int numPagina = 1, [FromQuery] string? filtro = null)
        {
            if (numPagina <= 0)
                return BadRequest();

            return Ok(_blAdministrativo.getMedicosPaginadosYFiltrados(numPagina, filtro));
        }
    }
}
