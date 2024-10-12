using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class SegurosMedicos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nombre { get; set; } = "-- Sin Nombre --";

        [Required]
        public string Descripcion { get; set; } = "-- Sin Descripción --";
    }
}
