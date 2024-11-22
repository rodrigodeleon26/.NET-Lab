namespace Shared
{
    public class Factura
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; } = new DateTime();

        public float Monto { get; set; } = 0.0f;

        public bool Pago { get; set; } = false;

        public string Descripcion { get; set; }

        public DateTime? FechaPago { get; set; }

        public Paciente Paciente { get; set; } = new Paciente();

        public PagoPayPal PagoPayPal { get; set; } = new PagoPayPal();
    }
}
