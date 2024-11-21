using BL.IBLs;
using DAL.DALs;
using DAL.IDALs;
using DAL.Models;
using Shared;

namespace BL.BLs
{
    public class BL_Pacientes : IBL_Pacientes
    {
        private readonly IDAL_Pacientes dal;
        private readonly IDAL_HistoriasClinicas dalHistoriasClinicas;
        private readonly IDAL_CitasMedicas dalCitasMedicas;
        private readonly IDAL_Administrativo dalAdministrativo;

        public BL_Pacientes(
            IDAL_Pacientes dal,
            IDAL_HistoriasClinicas dalHistoriasClinicas,
            IDAL_CitasMedicas dalCitasMedicas,
            IDAL_Administrativo dalAdministrativo
            )
        {
            this.dal = dal;
            this.dalHistoriasClinicas = dalHistoriasClinicas;
            this.dalCitasMedicas = dalCitasMedicas;
            this.dalAdministrativo = dalAdministrativo;
        }

        public List<Paciente> getPacientes()
        {
            return dal.getPacientes();
        }

        public void addPaciente(Paciente paciente)
        {
            dal.addPaciente(paciente);
        }

        public Paciente getXDocumento(string documento)
        {
            return dal.getXDocumento(documento);
        }

        public Paciente GetPaciente(long id)
        {
            return dal.GetPaciente(id);
        }

        //Notificaciones

        public void AddNotificacion(Notificacion notificacion, long idPaciente)
        {
            dal.AddNotificacion(notificacion, idPaciente);
        }
        
        public Paciente getMisDatos(string dni)
        {
            if (string.IsNullOrEmpty(dni))
            {
                return null;
            }
            return dalAdministrativo.GetPacienteByDNI(dni);
        }

        public void actualizarDatos(Paciente paciente)
        {
           
            if (paciente == null)
            {
                //return null;
            }

            dalAdministrativo.UpdatePaciente(paciente);
        }

        public object GetHistoriaClinica(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds)
        {
            var paciente = dalAdministrativo.GetPacienteByDNI(dni);
            if (paciente == null)
            {
                return null;
            }

            Console.WriteLine($"Paciente encontrado: {paciente.Nombres} {paciente.Apellidos}");
            Console.WriteLine($"Especialidades: {string.Join(", ", especialidadesIds)}");

            var citasMedicas = dalCitasMedicas.GetCitasMedicasByPacienteId(paciente.Id, pageNumber, pageSize, fechaInicio, fechaFin, orden, especialidadesIds);

            Console.WriteLine($"Número de citas encontradas: {citasMedicas.Count}");

            List<ConsultaMedicaConCitaDTO> consultasMedicasConCitas = new List<ConsultaMedicaConCitaDTO>();
            foreach (var cita in citasMedicas)
            {
                var consulta = dalHistoriasClinicas.getConsultaMedica(cita.ConsultaMedicaId ?? 0);
                Console.WriteLine($"Consulta encontrada: {consulta?.Descripcion}");
                consultasMedicasConCitas.Add(new ConsultaMedicaConCitaDTO
                {
                    ConsultaMedica = consulta,
                    CitaMedica = cita
                });
            }
            // Para obtener el total de citas, útil para calcular el número total de páginas
            int totalCitas = dalCitasMedicas.CountCitasMedicasByPacienteId(paciente.Id, fechaInicio, fechaFin, orden, especialidadesIds);

            return new
            {
                ConsultasMedicasConCitas = consultasMedicasConCitas,
                Paciente = paciente,
                TotalItems = totalCitas,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCitas / pageSize)
            };
        }

        public object getNotificaciones(string dni, int pageNumber, int pageSize)
        {
            Paciente paciente = dalAdministrativo.GetPacienteByDNI(dni);
            if (paciente == null)
            {
                return null;
            }
            if (pageSize == 0) {
                pageSize = 5;
            }

            int totalNotificaciones = dalAdministrativo.CountNotificaciones(paciente.Id);

            List<Notificacion> notificaciones = dalAdministrativo.getNotificaciones(paciente.Id, pageNumber, pageSize);

            return new
            {
                notificaciones = notificaciones,
                totalItems = totalNotificaciones,
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalNotificaciones / pageSize)
            };
        }

        public bool notificacionVista(long idNotificacion)
        {
            Console.WriteLine($"Notificacion vista PBL: {idNotificacion}");
            if (idNotificacion == 0 || idNotificacion == null)
            {
                return false;
            }
            return dal.notificacionVista(idNotificacion);
        }
    }
}
