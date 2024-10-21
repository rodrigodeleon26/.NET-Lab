using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class AppUsers : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        // Relación con Paciente (opcional)
        [PersonalData]
        public long? PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Pacientes? Paciente { get; set; }

        //// Relación con Medico (opcional)
        //[PersonalData]
        //public int? MedicoId { get; set; }
        //[ForeignKey("MedicoId")]
        //public Medicos? Medico { get; set; }
    }
}
