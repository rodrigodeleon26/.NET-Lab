using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DAL.IDALs
{
    public interface IDAL_Pacientes
    {
        public List<Paciente> getPacientes();

        public void addPaciente(Paciente paciente);

        public Paciente getXDocumento(string documento);

        public Paciente GetPaciente(long id);

        public void AddNotificacion(Notificacion notificacion, long idPaciente);
        
        public bool notificacionVista(long idNotificacion);
        Task<string> GetAccessToken(string pacienteId, string code);
        bool DesvincularGoogle(long id);
    }
}
