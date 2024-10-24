namespace Shared
{
    public class Calendario
    {
        public long Id { get; set; }

        public TimeSpan HoraInicio { get; set; } = new TimeSpan();

        public TimeSpan HoraFin { get; set; } = new TimeSpan();

        public int TiempoCita { get; set; } = 0;

        public Consultorio Consultorio { get; set; } = new Consultorio();

        public int CantidadCitas { get; set; } = 0;

        public string[] DiasSemana { get; set; } = new string[] { "-- Sin Días de la Semana --" };

        public Medico Medico { get; set; } = new Medico();

        public Especialidad Especialidad { get; set; } = new Especialidad();

        public List<CitaMedica> CitasMedicas { get; set; } = new List<CitaMedica>();
    }
}
