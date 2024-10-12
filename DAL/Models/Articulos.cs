using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Articulos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nombre { get; set; } = "-- Sin Nombre --";

        public virtual ICollection<Copagos> Copagos { get; set; } = new List<Copagos>();
    }
}
