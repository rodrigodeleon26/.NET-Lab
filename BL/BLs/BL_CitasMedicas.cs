using BL.IBLs;
using DAL.IDALs;
using Shared;

namespace BL.BLs
{
    public class BL_CitasMedicas : IBL_CitasMedicas
    {
        private readonly IDAL_CitasMedicas dal;

        public BL_CitasMedicas(IDAL_CitasMedicas dal)
        {
            this.dal = dal;
        }

        public List<CitaMedica> getCitasMedicas()
        {
            return dal.getCitasMedicas();
        }
    }
}
