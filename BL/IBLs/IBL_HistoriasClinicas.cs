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
        ConsultaMedica getConsultaMedica(int id);
        ConsultaMedica createConsultaMedica(ConsultaMedicaDTO consultaMedica);
        ConsultaMedica createConsultaMedicaSD(long consultaMedicaId);
        ConsultaMedica updateConsultaMedica(ConsultaMedica consultaMedica);
        ConsultaMedica addReceta(int idConsultaMedica, Receta receta);
        ConsultaMedica updateReceta(int idConsultaMedica, Receta receta);
        ConsultaMedica deleteReceta(int idConsultaMedica, int idReceta);
        ConsultaMedica addEstudio(int idConsultaMedica, Estudio estudio);
        ConsultaMedica updateEstudio(int idConsultaMedica, Estudio estudio);
        ConsultaMedica deleteEstudio(int idConsultaMedica, int idEstudio);
        ConsultaMedica addResultadoEstudio(int idConsultaMedica, int idEstudio, DateOnly fechaResultado, string imagenUrl);
    }
}
