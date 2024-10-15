using BL.IBLs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLs
{
    public class BL_Administrativo : IBL_Administrativo
    {
        public string getFuncaLaApi()
        {
            return "La API funciona correctamente";
        }
    }
}
