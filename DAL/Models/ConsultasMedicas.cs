using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class ConsultasMedicas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Descripcion { get; set; } = "-- Sin Descripción --";

        [Required]
        public string Diagnostico { get; set; } = "-- Sin Diagnóstico --";

        //[Required]
        //public long CitaMedicaId { get; set; } 

        //[ForeignKey(nameof(CitaMedicaId))]
        //public virtual CitasMedicas? CitaMedica { get; set; }

        public virtual ICollection<Estudios> Estudios { get; set; } = new List<Estudios>();

        public virtual ICollection<Recetas> Recetas { get; set; } = new List<Recetas>();
    }
}
