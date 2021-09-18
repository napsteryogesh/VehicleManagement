using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScalableVehicleService.DAL;
using ScalableVehicleService.Model;
using ScalableVehicleService.PersistentQueue;
using ScalableVehicleService.Services.Payload;

namespace ScalableVehicleService.Services
{
   public class VehiclePayloadConsumer : IAcknowledgePayloadConsumer<VehiclePayload>
    {
        private readonly ILogger<VehiclePayloadConsumer> _logger;
        private readonly IGenericRepository<Vehicle> vehicleRepository;

        public VehiclePayloadConsumer(ILogger<VehiclePayloadConsumer> logger, IGenericRepository<Vehicle> vehicleRepository)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.vehicleRepository = vehicleRepository ?? throw new ArgumentNullException();
        }
        public async Task<bool> ProcessPayload(VehiclePayload payload)
        {
            try
            {
                var vehicles = await vehicleRepository.GetAsync(obj => obj.VehicleNumber == payload.Vehicle.VehicleNumber);
                if (payload.Action == VehicleAction.RecordLocation)
                {
                    if (vehicles == null && !vehicles.Any())
                    {
                        _logger.LogError($"No Vehicle found with Vehicle number {payload.Vehicle.VehicleNumber}");
                        return false;
                    }
                    var vehicle = vehicles.First();
                    vehicle.Location = payload.Vehicle.Location;
                    await this.vehicleRepository.UpdateAsync(vehicle);
                }
                if (payload.Action == VehicleAction.Register)
                {
                    if (vehicles == null && vehicles.Any())
                    {
                        _logger.LogError($"Vehicle is already registered with number {payload.Vehicle.VehicleNumber}");
                        return false;
                    }

                    await this.vehicleRepository.InsertAsync(payload.Vehicle);
                }
                return true;
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                return false;
            }
        }
    }
}
