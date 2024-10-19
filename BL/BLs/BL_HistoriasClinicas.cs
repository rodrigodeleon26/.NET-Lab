using Shared;
using BL.IBLs;
using DAL.IDALs;
using DAL.Models;

namespace BL.BLs
{
    public class BL_HistoriasClinicas : IBL_HistoriasClinicas
    {
        private readonly IDAL_HistoriasClinicas dal;

        public BL_HistoriasClinicas(IDAL_HistoriasClinicas dal)
        {
            this.dal = dal;
        }

        public List<ConsultaMedica> getConsultasMedicas()
        {
            return dal.getConsultasMedicas();
        }

        public ConsultaMedica getConsultaMedica(int id)
        {
            return dal.getConsultaMedica(id);
        }

        public ConsultaMedica createConsultaMedica(ConsultaMedica consultaMedica)
        {
            return dal.createConsultaMedica(consultaMedica);
        }

        public ConsultaMedica createConsultaMedicaSimple(ConsultaMedica consultaMedica)
        {
            return dal.createConsultaMedicaSimple(consultaMedica);
        }

        public ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica)
        {
            return dal.updateConsultaMedica(consultaMedica);
        }

        public ConsultaMedica addReceta(int idConsultaMedica, Receta receta)
        {
            return dal.addReceta(idConsultaMedica, receta);
        }
        
        public ConsultaMedica updateReceta(int idConsultaMedica, Receta receta)
        {
            return dal.updateReceta(idConsultaMedica, receta);
        }

        public ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta)
        {
            return dal.deleteReceta(idConsultaMedica, idReceta);
        }

        public ConsultaMedica addEstudio(int idConsultaMedica, Estudio estudio)
        {
            return dal.addEstudio(idConsultaMedica, estudio);
        }

        public ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio)
        {
            return dal.updateEstudio(idConsultaMedica, estudio);
        }

        public ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio)
        {
            return dal.deleteEstudio(idConsultaMedica, idEstudio);
        }

        public ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, string resultado, DateOnly fechaResultado)
        {
            return dal.addResultadoEstudio(idConsultaMedica, idEstudio, resultado, fechaResultado);
        }
    }
}
