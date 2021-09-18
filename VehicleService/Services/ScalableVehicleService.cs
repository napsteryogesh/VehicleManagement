using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ScalableVehicleService.Model;
using ScalableVehicleService.PersistentQueue;
using ScalableVehicleService.Services.Payload;

namespace ScalableVehicleService.Services
{
    public interface IScalableVehicleService : IVehicleService
    {

    }
    public class ScalableVehicleService : IScalableVehicleService
    {

        private readonly IVehicleService _vehicleService ;
        public readonly IPersistentConcurrentQueue<VehiclePayload> _persistentConcurrentQueue;

        public ScalableVehicleService(IVehicleService vehicleService, IPersistentConcurrentQueue<VehiclePayload> persistentConcurrentQueue)
        {
            this._vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            this._persistentConcurrentQueue = persistentConcurrentQueue ?? throw new ArgumentNullException(nameof(persistentConcurrentQueue));
        }
        public async Task<Location> GetLastLocationAsync(string vehicleNumber)
        {
            return await this._vehicleService.GetLastLocationAsync(vehicleNumber);
        }

        public async Task<bool> RecordLocationAsync(string vehicleNumber, Location location)
        {
            var payload = new VehiclePayload(VehicleAction.RecordLocation, new Vehicle() { VehicleNumber= vehicleNumber, Location = location },Guid.NewGuid());
            await _persistentConcurrentQueue.PublishPayload(payload);
            return true;
        }

        public async Task<bool> RegisterAsync(Vehicle vehicle)
        {
            var payload = new VehiclePayload(VehicleAction.Register, vehicle, Guid.NewGuid());
            await _persistentConcurrentQueue.PublishPayload(payload);
            return true;
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles()
        {
            return await this._vehicleService.GetVehicles();
        }

        public async Task<Vehicle> GetVehicle(string vehicleNumber)
        {
            return await this._vehicleService.GetVehicle(vehicleNumber);
        }

        public async Task<IEnumerable<Location>> GetLastLocations(int lastLocationCount)
        {
            return await this._vehicleService.GetLastLocations(lastLocationCount);
        }
    }
}
