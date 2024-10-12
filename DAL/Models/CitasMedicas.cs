using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class CitasMedicas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = new DateTime();

        [Required]
        public string Estado { get; set; } = "-- Sin Estado --";

        [Required]
        public long CalendarioId { get; set; }

        [ForeignKey(nameof(CalendarioId))]
        public virtual Calendarios Calendario { get; set; }
    }
}
