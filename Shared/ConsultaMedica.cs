namespace Shared
{
    public class ConsultaMedica
    {
        public long Id { get; set; }

        public string Descripcion { get; set; } = "-- Sin Descripción --";

        public string Diagnostico { get; set; } = "-- Sin Descripción --";

        //public CitaMedica CitaMedica { get; set; } = new CitaMedica();  

        public long CitaMedicaId { get; set; }

        public List<Estudio> Estudios { get; set; } = new List<Estudio>();

        public List<Receta> Recetas { get; set; } = new List<Receta>();
    }

    public class ConsultaMedicaDTO
    {
        public long Id { get; set; }
        public string Descripcion { get; set; }
        public string Diagnostico { get; set; }
        public long CitaMedicaId { get; set; }
    }

    public class ConsultaMedicaConCitaDTO
    {
        public ConsultaMedica ConsultaMedica { get; set; }
        public CitaMedica CitaMedica { get; set; }
    }

    public class ConsultaMedicaCompletaDTO
    {
        public ConsultaMedica ConsultaMedica { get; set; }
        public CitaMedica CitaMedica { get; set; }  
        public Paciente Paciente { get; set; }
    }
}