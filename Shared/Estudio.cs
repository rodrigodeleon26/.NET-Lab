namespace Shared
{
    public class Estudio
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = "-- Sin Nombre --";

        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public DateOnly FechaRealizado { get; set; } = new DateOnly();

        public DateOnly FechaResultado { get; set; } = new DateOnly();

        public string Resultado { get; set; } = "-- Sin Resultado --";
    }
}
