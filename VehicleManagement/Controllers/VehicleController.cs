using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScalableVehicleService;
using ScalableVehicleService.Model;
using ScalableVehicleService.Services;
using Microsoft.Extensions.Configuration;
using VehicleService.Services;

namespace VehicleManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;
        private bool _isScalabledMode = false;

        public VehicleController(ILogger<VehicleController> logger, IConfiguration configuration, IVehicleService vehicleService, IScalableVehicleService scalableVehicleService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (bool.TryParse(configuration.GetValue<string>(VehicleServiceConstants.SCALE_KEY), out bool isScalableMode))
            {
                _isScalabledMode = isScalableMode;
                if (_isScalabledMode)
                {
                    this._vehicleService = scalableVehicleService;
                }
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetVehicle([FromQuery]string vehicleNumber)
        {
            _logger.LogInformation($"Get request received for {vehicleNumber}");
            if(string.IsNullOrEmpty(vehicleNumber))
            {
                return BadRequest("Invalid vehicle id");
            }
            try
            {
                var response = await this._vehicleService.GetVehicle(vehicleNumber);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception.StackTrace);
                return NotFound(exception.Message);
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GeAllVehicle()
        {
            _logger.LogInformation($"Get request received for all Vehicles");
            try
            {
                var response = await this._vehicleService.GetVehicles();
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception.StackTrace);
                return NotFound(exception.Message);
            }
        }



        [HttpGet("{vehicleId}/Location")]
        public async Task<IActionResult> GetVehicleLocation(string vehicleNumber)
        {
            _logger.LogInformation($"Get request received for {vehicleNumber}");
            try
            {
              var response = await  this._vehicleService.GetLastLocationAsync(vehicleNumber);
                return Ok(response);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message, exception.StackTrace);
                return NotFound(exception.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> RegisterVehicle([FromBody] Vehicle vehicle)
        {
            if(vehicle == null)
            {
                _logger.LogError("No vehilce object received for Registrations");
                return BadRequest();
            }
            _logger.LogInformation($"Get request received for {vehicle.VehicleNumber}");
            try
            {
                var response = await this._vehicleService.RegisterAsync(vehicle);
                string message = _isScalabledMode ? VehicleServiceConstants.Registartion_Request_Submitted : VehicleServiceConstants.Registartion_Successful;
                return Ok(string.Format(message,vehicle.VehicleNumber));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception.StackTrace);
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{vehicleNumber}/RecordLocation")]
        public async Task<IActionResult> RecordLocation(string vehicleNumber,[FromBody] Location location )
        {
            _logger.LogInformation($"Record Location Request received for {vehicleNumber}");
            try
            {
                var response = await this._vehicleService.RecordLocationAsync(vehicleNumber, location);
                string message = _isScalabledMode ? VehicleServiceConstants.Location_Update_Request_Submitted : VehicleServiceConstants.Location_Updated_Successful;
                return Ok(string.Format(message, vehicleNumber));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception.StackTrace);
                return NotFound(exception.Message);
            }
        }


    }
}

