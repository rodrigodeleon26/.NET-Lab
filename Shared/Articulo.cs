namespace Shared
{
    public class Articulo
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = "-- Sin Nombre --";

        public List<Copago> Copagos{ get; set; } = new List<Copago>();
    }
}
