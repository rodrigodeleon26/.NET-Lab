namespace Shared
{
    public class EspecialidadMedico
    {
        public long Id { get; set; }

        public Especialidad Especialidad { get; set; } = new Especialidad();

        public Medico Medico { get; set; } = new Medico();
    }
}
