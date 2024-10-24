namespace Shared
{
    public class Medico
    {
        public long Id { get; set; }

        public string Nombres { get; set; } = "-- Sin Nombre --";

        public string Apellidos { get; set; } = "-- Sin Apellidos --";

        public string Documento { get; set; } = "-- Sin Documento --";

        public string Email { get; set; } = "-- Sin Email --";

        public string Telefono { get; set; } = "-- Sin Teléfono --";

        public List<Calendario> Calendarios { get; set; } = new List<Calendario>();

        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
    }
}
