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

        public virtual ICollection<Contratos> Contratos { get; set; } = new List<Contratos>();

        public virtual ICollection<Copagos> Copagos { get; set; } = new List<Copagos>(); 

        public virtual ICollection<Precios> Precios { get; set; } = new List<Precios>();
    }
}
