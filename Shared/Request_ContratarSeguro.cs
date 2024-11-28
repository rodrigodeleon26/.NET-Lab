using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared
{
    public class Request_ContratarSeguro
    {
        public long IdPaciente { get; set; }
        public long IdSeguroMedico { get; set; }
    }

    public class Request_DatosAgendarCita
    {
        public string Cedula { get; set; }
        public long CalendarioId { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public long ArticuloId { get; set; }
        public bool CitaOnline { get; set; }
    }
}
