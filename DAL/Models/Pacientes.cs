using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Pacientes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; } = "-- Sin Nombre --";

        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; } = "-- Sin Apellidos --";

        [Required]
        [MaxLength(20)]
        public string Documento { get; set; } = "-- Sin Documento --";

        [Required]
        public DateOnly FechaDeNacimiento { get; set; } = new DateOnly();

        [MaxLength(200)]
        public string Direccion { get; set; } = "-- Sin Dirección --";

        [MaxLength(20)]
        public string Telefono { get; set; } = "-- Sin Teléfono --";

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = "-- Sin Email --";

        public virtual ICollection<CitasMedicas> CitasMedicas { get; set; } = new List<CitasMedicas>(); // Usar virtual si necesitas Lazy Loading

        public virtual ICollection<Facturas> Facturas { get; set; } = new List<Facturas>(); // Usar virtual si necesitas Lazy Loading

        public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>(); // Usar virtual si necesitas Lazy Loading

        public virtual Contratos Contrato { get; set; } // Relación uno-a-uno
    }
}
