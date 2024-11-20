using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class CitasMedicas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = new DateTime();

        [Required]
        public string Estado { get; set; } = "-- Sin Estado --";

        [Required]
        public long CalendarioId { get; set; }

        [ForeignKey(nameof(CalendarioId))]
        public virtual Calendarios Calendario { get; set; }

        public long? PacienteId { get; set; } // Hacer nullable

        [ForeignKey(nameof(PacienteId))]
        public virtual Pacientes? Paciente { get; set; } // Hacer nullable

        public long? ConsultaMedicaId { get; set; } // Hacer nullable

        [ForeignKey(nameof(ConsultaMedicaId))]
        public virtual ConsultasMedicas? ConsultaMedica { get; set; } 
    }
}
