using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IDALs
{
    public interface IDAL_Administrativo_Service
    {
        public Paciente GetPacienteById(long id);
        public Paciente GetPacienteByDNI(string dni);

        Task<bool> AddNotificacionService(Notificacion notificacion, long idPaciente);

    }
}
