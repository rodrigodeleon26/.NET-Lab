using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Paciente
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
    }
}
