using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Precios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? CopagoId { get; set; }

        [ForeignKey(nameof(CopagoId))]
        public virtual Copagos? Copago { get; set; }

        public long? SeguroMedicoId { get; set; }

        [ForeignKey(nameof(SeguroMedicoId))]
        public virtual SegurosMedicos? SeguroMedico { get; set; }

        [Required]
        public float PrecioBase { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }
    }
}
