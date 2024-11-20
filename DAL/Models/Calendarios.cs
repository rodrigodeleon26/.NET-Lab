using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

public class Calendarios
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public long Id { get; set; }

    [Required]
    public TimeSpan HoraInicio { get; set; } = new TimeSpan();

    [Required]
    public TimeSpan HoraFin { get; set; } = new TimeSpan();

    [Required]
    public int TiempoCita { get; set; } = 0;

    [Required]
    public long ConsultorioId { get; set; }

    [ForeignKey(nameof(ConsultorioId))]
    public virtual Consultorios Consultorio { get; set; }

    [Required]
    public int CantidadCitas { get; set; } = 0;

    [NotMapped]
    public string[] DiasSemana { get; set; } = new string[] { "-- Sin Días de la Semana --" };

    [Required]
    public string DiasSemanaString
    {
        get => string.Join(",", DiasSemana);
        set => DiasSemana = value.Split(',');
    }

    [Required]
    public long EspecialidadId { get; set; }

    [ForeignKey(nameof(EspecialidadId))]
    public virtual Especialidades Especialidad { get; set; } // Usar virtual si necesitas Lazy Loading

    [Required]
    public long MedicoId { get; set; }

    [ForeignKey(nameof(MedicoId))]
    public virtual Medicos Medico { get; set; } // Usar virtual si necesitas Lazy Loading

    public virtual ICollection<CitasMedicas> CitasMedicas { get; set; } = new List<CitasMedicas>(); // Usar virtual si necesitas Lazy Loading

    [Required]
    public bool Activo { get; set; } = true;
}