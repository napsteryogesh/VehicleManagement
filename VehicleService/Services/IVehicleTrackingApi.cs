using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleService
{
    public interface IVehicleTrackingApi
    {
        Task<bool> RegisterVehicle();

        Task<bool> UpdateLocation();
    }
}
