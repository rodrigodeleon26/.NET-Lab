namespace Shared
{
    public class Notificacion
    {
        public long Id { get; set; }

        public string Mensaje { get; set; } = "-- Sin Mensaje --";

        public DateTime FechaEnvio { get; set; } = new DateTime();

        public bool Visto { get; set; } = false;
    }
}
