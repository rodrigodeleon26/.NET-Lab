namespace Shared
{
    public class Paciente
    {
        public long Id { get; set; }

        public string Nombres { get; set; } = "-- Sin Nombre --";

        public string Apellidos { get; set; } = "-- Sin Apellidos --";

        public string Documento { get; set; } = "-- Sin Documento --";

        public DateOnly? FechaDeNacimiento { get; set; } = null;

        public string? Direccion { get; set; } = null;

        public string? Telefono { get; set; } = null;

        public string? Email { get; set; } = null;

        public List<CitaMedica>? CitasMedicas { get; set; } = new List<CitaMedica>();

        public List<Factura>? Facturas { get; set; } = new List<Factura>();

        public List<Notificacion>? Notificaciones { get; set; } = new List<Notificacion>();

        public Contrato? Contrato { get; set; }
    }
}
