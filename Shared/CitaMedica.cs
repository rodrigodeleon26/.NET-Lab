namespace Shared
{
    public class CitaMedica
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = new DateTime();

        public string Estado { get; set; } = "-- Sin Estado --";

        public Calendario Calendario { get; set; } = new Calendario();

        public Paciente? Paciente { get; set; } = new Paciente();

        public ConsultaMedica? ConsultaMedica { get; set; }
    }   
}
