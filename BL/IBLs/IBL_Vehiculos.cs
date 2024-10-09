using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace BL.IBLs
{
    public interface IBL_Vehiculos
    {
        List<Vehiculo> GetVehiculos();

        Vehiculo GetVehiculo(long id);

        void AddVehiculo(Vehiculo vehiculo);

        void DeleteVehiculo(long id);

        void UpdateVehiculo(Vehiculo vehiculo);

        List<Vehiculo> GetVehiculosByPersona(long id);
    }
}
