using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBLs
{
    public interface IBL_Pacientes
    {
        List <Paciente> getPacientes();

        public void addPaciente(Paciente paciente);

        public Paciente getXDocumento(string documento);

        public Paciente GetPaciente(long id);

        public void AddNotificacion(Notificacion notificacion, long idPaciente);
        
        public Paciente getMisDatos(string dni);

        public void actualizarDatos(Paciente paciente);

        object GetHistoriaClinica(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);

        object getNotificaciones(string dni, int pageNumber, int pageSize);

        bool notificacionVista(long idNotificacion);

        List<CitaMedica> getMisCitas(string documento);

        bool CancelarCita(string dni, long id);

        object getHistorialFacturacion(string dni, int pageNumber, int pageSize);
    }
}
