namespace Shared
{
    public class Especialidad
    {
        public long Id { get; set; }

        public string Nombre { get; set; } = "-- Sin Nombre --";

        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public List<Calendario> Calendarios { get; set; } = new List<Calendario>();
    }
}
