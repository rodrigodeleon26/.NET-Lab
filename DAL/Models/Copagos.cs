using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Copagos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public long ArticuloId { get; set; }

        [ForeignKey(nameof(ArticuloId))]
        public virtual Articulos Articulo { get; set; }

        [Required]
        public long SeguroMedicoId { get; set; }

        [ForeignKey(nameof(SeguroMedicoId))]
        public virtual SegurosMedicos SeguroMedico { get; set; }

        [Required]
        public long EspecialidadId { get; set; }

        [ForeignKey(nameof(EspecialidadId))]
        public virtual Especialidades Especialidad { get; set; }

        public virtual ICollection<Precios> Precios { get; set; } = new List<Precios>();
    }
}
