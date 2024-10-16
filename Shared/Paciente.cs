namespace Shared
{
    public class Paciente
    {
        public long Id { get; set; }

        public string Nombres { get; set; } = "-- Sin Nombre --";

        public string Apellidos { get; set; } = "-- Sin Apellidos --";

        public string Documento { get; set; } = "-- Sin Documento --";

        public DateOnly FechaDeNacimiento { get; set; } = new DateOnly();

        public string Direccion { get; set; } = "-- Sin Dirección --";

        public string Telefono { get; set; } = "-- Sin Teléfono --";

        public string Email { get; set; } = "-- Sin Email --";

        public List<CitaMedica> CitasMedicas { get; set; } = new List<CitaMedica>();

        public List<Factura> Facturas { get; set; } = new List<Factura>();

        public List<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

        //puede ser null
        public Contrato? Contrato { get; set; }
    }
}
