using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Recetas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public DateOnly Vencimiento { get; set; } = new DateOnly();

        [Required]
        public string NombreMedicamento { get; set; } = "-- Sin Nombre --";

        [Required]
        public int Cantidad { get; set; } = 0;

        [Required]
        public string Frecuencia { get; set; } = "-- Sin Frecuencia --";
    }
}
