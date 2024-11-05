namespace Shared
{
    public class Estudio
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = "-- Sin Nombre --";

        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public DateOnly? FechaRealizado { get; set; }

        public DateOnly? FechaResultado { get; set; }

        public string? ImagenUrl { get; set; }

        public long ConsultaMedicaId { get; set; }
    }

    public class EstudioDTO
    {
        public string Nombre { get; set; } = "-- Sin Nombre --";
        public string Descripcion { get; set; } = "-- Sin Descripción --";
        public DateOnly FechaRealizado { get; set; }
        public DateOnly FechaResultado { get; set; }
    }
}
