namespace Shared
{
    public class Factura
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = new DateTime();

        public float Monto { get; set; } = 0.0f;

        public bool Pago { get; set; } = false;

        public DateTime FechaPago { get; set; } = new DateTime();

        public Paciente Paciente { get; set; } = new Paciente();
    }
}
