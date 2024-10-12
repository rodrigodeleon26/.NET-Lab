using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Estudios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nombre { get; set; } = "-- Sin Nombre --";    

        [Required]
        public string Descripcion { get; set; } = "-- Sin Descripción --";

        [Required]
        public DateOnly FechaRealizado { get; set; } = new DateOnly();

        [Required]
        public DateOnly FechaResultado { get; set; } = new DateOnly();

        [Required]
        public string Resultado { get; set; } = "-- Sin Resultado --";
    }
}
