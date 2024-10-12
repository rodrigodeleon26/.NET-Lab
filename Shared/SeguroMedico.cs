namespace Shared
{
    public class SeguroMedico
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = "-- Sin Nombre --";

        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public List<Contrato> Contratos { get; set; } = new List<Contrato>();

        public List<Copago> Copagos { get; set; } = new List<Copago>();

        public List<Precio> Precios { get; set; } = new List<Precio>();
    }
}
