namespace Shared
{
    public class Receta
    {
        public long Id { get; set; }

        public DateOnly Vencimiento { get; set; } = new DateOnly();

        public string NombreMedicamento { get; set; } = "-- Sin Nombre --";

        public int Cantidad { get; set; } = 0;

        public string Frecuencia { get; set; } = "-- Sin Frecuencia --";

        public ConsultaMedica ConsultaMedica { get; set; } = new ConsultaMedica();
    }
}
