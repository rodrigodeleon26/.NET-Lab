namespace Shared
{
    public class CitaMedica
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = new DateTime();

        public string Estado { get; set; } = "-- Sin Estado --";

        public Calendario Calendario { get; set; } = new Calendario();

        public long CalendarioId { get; set; }


        public Paciente? Paciente { get; set; } = new Paciente();

        //public long? ConsultorioId { get; set; }

        public long? ConsultaMedicaId { get; set; }
        //public ConsultaMedica? ConsultaMedica { get; set; }

        public long? PacienteId { get; set; }
    }
}
