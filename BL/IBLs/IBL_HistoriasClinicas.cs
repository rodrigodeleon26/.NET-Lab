using DAL.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBLs
{
    public interface IBL_HistoriasClinicas
    {
        List<ConsultaMedica> getConsultasMedicas();
        ConsultaMedica getConsultaMedica(long id);
        ConsultaMedicaCompletaDTO getConsultaMedicaCompleta(long id);
        ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica);
        ConsultaMedica createConsultaMedicaSD(long consultaMedicaId);
        ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica);
        ConsultaMedica deleteConsultaMedica(int id);
        ConsultaMedica addReceta(int idConsultaMedica, Receta receta);
        ConsultaMedica updateReceta(int idConsultaMedica, Receta receta);
        ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta);
        Task<ConsultaMedica> addEstudio(int idConsultaMedica, Estudio estudio);
        ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio);
        ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio);
        ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl);
        object GetHistoriaClinica(string dni, int pageNumber, int pageSize, DateTime? fechaInicio, DateTime? fechaFin, string orden, List<long> especialidadesIds);
        ConsultaMedica GuardarConsulta(long id);
        public List<Medicamento> getMedicamentos();

    }
}
