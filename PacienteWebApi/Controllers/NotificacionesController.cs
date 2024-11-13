using BL.BLs;
using BL.IBLs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PacienteWebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly IBL_Pacientes _blPacientes;

        public NotificacionesController(IBL_Pacientes blPacientes)
        {
            _blPacientes = blPacientes;
        }

        // POST api/<FacturasController>
        [ProducesResponseType(typeof(Factura), 201)]
        [HttpPost("{idPaciente}")]
        public IActionResult Post(long idPaciente, [FromBody] Notificacion notificacion)
        {
            if (notificacion == null)
            {
                return BadRequest();
            }

            _blPacientes.AddNotificacion(notificacion, idPaciente);
            //return CreatedAtAction(nameof(Get), new { id = notificacion.Id }, notificacion);
            return Ok();
        }
    }
}