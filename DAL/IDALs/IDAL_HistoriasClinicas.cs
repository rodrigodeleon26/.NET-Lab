using DAL.Models;
using Shared;

namespace DAL.IDALs
{
    public interface IDAL_HistoriasClinicas
    {
        public List<ConsultaMedica> getConsultasMedicas();
        public ConsultaMedica getConsultaMedica(int id);
        public ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica);
        public ConsultaMedica createConsultaMedicaSD(long consultaMedicaId);
        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica);
        public ConsultaMedica deleteConsultaMedica(int id);
        public ConsultaMedica addReceta(int idConsultaMedica, Receta receta);
        public ConsultaMedica updateReceta(int idConsultaMedica, Receta receta);
        public ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta);
        public ConsultaMedica addEstudio(int idConsultaMedica, Estudio estudio);
        public ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio);
        public ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio);
        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl);
    }
}
