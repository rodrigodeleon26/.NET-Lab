using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Contratos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; } = new DateTime();

        [Required]
        public bool Activo { get; set; } = false;

        [Required]
        public long PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public virtual Pacientes Paciente { get; set; } // Usar virtual si necesitas Lazy Loading

        [Required]
        public long SeguroMedicoId { get; set; }

        [ForeignKey(nameof(SeguroMedicoId))]
        public virtual SegurosMedicos SeguroMedico { get; set; } // Usar virtual si necesitas Lazy Loading
    }
}
