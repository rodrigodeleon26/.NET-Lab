using BL.IBLs;
using DAL.IDALs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLs
{
    public class BL_Vehiculos : IBL_Vehiculos
    {
        private readonly IDAL_Vehiculos dal;

        public BL_Vehiculos(IDAL_Vehiculos _dal)
        {
            dal = _dal;
        }

        public void AddVehiculo(Vehiculo vehiculo)
        {
            dal.AddVehiculo(vehiculo);
        }

        public void DeleteVehiculo(long id)
        {
            dal.DeleteVehiculo(id);
        }

        public void UpdateVehiculo(Vehiculo vehiculo)
        {
            dal.UpdateVehiculo(vehiculo);
        }

        public Vehiculo GetVehiculo(long id)
        {
            return dal.GetVehiculo(id);
        }

        public List<Vehiculo> GetVehiculos()
        {
            return dal.GetVehiculos();
        }

        public List<Vehiculo> GetVehiculosByPersona(long id)
        {
            return dal.GetVehiculosByPersona(id);
        }
    }
}
