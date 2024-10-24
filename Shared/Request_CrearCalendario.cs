using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Request_CrearCalendario
    {
        public long MedicoId { get; set; }
        public long EspecialidadId { get; set; }
        public long ConsultorioId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int Tiempo { get; set; }
        public int Cantidad { get; set; }
        public string[]? Dias { get; set; }
    }
}
