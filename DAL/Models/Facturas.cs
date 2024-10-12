using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Facturas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = new DateTime();

        [Required]
        public float Monto { get; set; } = 0.0f;

        [Required]
        public bool Pago { get; set; } = false;

        [Required]
        public DateTime FechaPago { get; set; } = new DateTime();
    }
}
