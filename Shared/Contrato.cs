namespace Shared
{
    public class Contrato
    {
        public long Id { get; set; }

        public DateTime FechaInicio { get; set; } = new DateTime();

        public bool Activo { get; set; } = false;

        public Paciente Paciente { get; set; } = new Paciente();

        public SeguroMedico SeguroMedico { get; set; } = new SeguroMedico();
    }
}
