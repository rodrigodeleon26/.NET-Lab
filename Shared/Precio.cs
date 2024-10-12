using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Precio
    {
        public long Id { get; set; }

        public Copago? Copago { get; set; } = new Copago();

        public SeguroMedico? SeguroMedico { get; set; } = new SeguroMedico();

        public float PrecioBase { get; set; }

        public DateTime FechaInicio { get; set; }
    }
}
