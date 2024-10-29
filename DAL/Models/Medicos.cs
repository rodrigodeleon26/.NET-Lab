using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Medicos
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

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = "-- Sin Email --";

        [MaxLength(20)]
        public string Telefono { get; set; } = "-- Sin Teléfono --";

        public virtual ICollection<Calendarios> Calendarios { get; set; } = new List<Calendarios>(); // Virtual para Lazy Loading

        public virtual ICollection<EspecialidadesMedicos> EspecialidadesMedicos { get; set; } = new List<EspecialidadesMedicos>(); // Virtual para Lazy Loading
    }
}
