namespace Shared
{
    public class Copago
    {
        public long Id { get; set; }

        public Articulo Articulo { get; set; } = new Articulo();

        public SeguroMedico SeguroMedico { get; set; } = new SeguroMedico();

        public Especialidad Especialidad { get; set; } = new Especialidad();

        public List<Precio> Precios { get; set; } = new List<Precio>();
    }
}
