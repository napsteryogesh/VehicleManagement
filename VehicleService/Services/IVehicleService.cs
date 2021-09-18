using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ScalableVehicleService.Model;

namespace ScalableVehicleService
{
    public interface IVehicleService
    {
        Task<bool> RegisterAsync(Vehicle vehicle);

        Task<bool> RecordLocationAsync(string vehicleNumber, Location location);

        Task<Location> GetLastLocationAsync(string vehicleNumber);

        Task<IEnumerable<Vehicle>> GetVehicles();

        Task<Vehicle> GetVehicle(string vehicleNumber);

        Task<IEnumerable<Location>> GetLastLocations(int lastLocationCount);
    }
}
