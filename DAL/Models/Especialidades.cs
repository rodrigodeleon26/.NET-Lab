using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Especialidades
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = "-- Sin Nombre --";

        [MaxLength(200)]
        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public virtual ICollection<Calendarios> Calendarios { get; set; } = new List<Calendarios>(); // Virtual para Lazy Loading

    }
}
