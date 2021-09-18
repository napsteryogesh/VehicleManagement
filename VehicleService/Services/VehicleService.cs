using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScalableVehicleService.DAL;
using ScalableVehicleService.Model;

namespace ScalableVehicleService
{
    public class VehicleService : IVehicleService
    {
        private readonly IGenericRepository<Vehicle> _vehicleRepository;
        private const string eagerLoadingProeprties = "Location";

        public VehicleService(IGenericRepository<Vehicle> vehicleRepository)
        {
           this._vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }
        public async Task<Location> GetLastLocationAsync(string vehicleNumber)
        {
            var vehicles = await this._vehicleRepository.GetAsync(obj => obj.VehicleNumber == vehicleNumber);
            if(vehicles == null || !vehicles.Any())
            {
                throw new Exception($"No Vehicle is registered with the vehicle number {vehicleNumber}");
            }
            return vehicles.FirstOrDefault().Location;
        }

        public async Task<bool> RegisterAsync(Vehicle vehicle)
        {
            var existingVehicle = await this._vehicleRepository.GetAsync(obj => obj.VehicleNumber == vehicle.VehicleNumber);
            if(existingVehicle !=null && existingVehicle.Any())
            {
                throw new Exception("Vehicle is already Registered");
            }
            await this._vehicleRepository.InsertAsync(vehicle);
            return true;
        }

        public async Task<bool> RecordLocationAsync(string vehicleNumber, Location location)
        {
            var existingVehicles = await this._vehicleRepository.GetAsync(obj => obj.VehicleNumber == vehicleNumber);
            if (existingVehicles==null && ! existingVehicles.Any())
            {
                throw new Exception($"No Vehicle is registered with vehicle Id{vehicleNumber}");
            }
            var vehicle = existingVehicles.FirstOrDefault();

            vehicle.Location = location;
            await this._vehicleRepository.UpdateAsync(vehicle);
            return true;
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles()
        {
            return await this._vehicleRepository.GetAsync(includeProperties: eagerLoadingProeprties);
        }

        public async Task<Vehicle> GetVehicle(string vehicleNumber)
        {
            var results = await this._vehicleRepository.GetAsync(obj => obj.VehicleNumber == vehicleNumber,includeProperties: eagerLoadingProeprties);
            return results.FirstOrDefault();
        }

        public Task<IEnumerable<Location>> GetLastLocations(int lastLocationCount)
        {
            throw new NotImplementedException();
        }
    }
}
