using BL.IBLs;
using Microsoft.AspNetCore.Mvc;
using Shared;

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

        // GET: api/<PacientesController>
        [ProducesResponseType(typeof(List<CitaMedica>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_blCitasMedicas.getCitasMedicas());
        }
    }
}