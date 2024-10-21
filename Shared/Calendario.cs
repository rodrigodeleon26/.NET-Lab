namespace Shared
{
    public class Calendario
    {
        public long Id { get; set; }

        public TimeSpan HoraInicio { get; set; } = new TimeSpan();

        public TimeSpan HoraFin { get; set; } = new TimeSpan();

        public int TiempoCita { get; set; } = 0;

        public int CantidadCitas { get; set; } = 0;

        public string DiasSemana { get; set; } = "-- Sin Días --";

        public Medico Medico { get; set; } = new Medico();

        public Especialidad Especialidad { get; set; } = new Especialidad();

        public List<CitaMedica> CitasMedicas { get; set; } = new List<CitaMedica>();
    }
}
