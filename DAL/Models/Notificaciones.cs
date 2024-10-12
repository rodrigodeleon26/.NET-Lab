using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Notificaciones
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Mensaje { get; set; } = "-- Sin Mensaje --";

        [Required]
        public DateTime FechaEnvio { get; set; } = new DateTime();

        [Required]
        public bool Visto { get; set; } = false;
    }
}
