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
        public string Nombre { get; set; } 

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public DateOnly? FechaRealizado { get; set; }

        [Required]
        public DateOnly? FechaResultado { get; set; }

        [Required]
        public string? ImagenUrl { get; set; }

        [Required]
        public long ConsultaMedicaId { get; set; }

        [ForeignKey(nameof(ConsultaMedicaId))]
        public virtual ConsultasMedicas ConsultaMedica { get; set; } 

    }
}
