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

        public bool Activo { get; set; } = false;

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        // Nueva propiedad para almacenar la clave de autenticación de dos factores
        public string? TwoFactorAuthKey { get; set; }

        // Relación con Medico (opcional)
        [PersonalData]
        public long? MedicoId { get; set; }
        [ForeignKey("MedicoId")]
        public Medicos? Medico { get; set; }
    }
}
