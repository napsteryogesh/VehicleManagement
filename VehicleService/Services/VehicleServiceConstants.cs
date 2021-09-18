using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleService.Services
{
    public class VehicleServiceConstants
    {
        public const string SCALE_KEY = "isScalableMode";

        public const string Registartion_Successful = "Vehicle has been been registered with:{0}";

        public const string Registartion_Request_Submitted = "Vehicle resgistration has been accepted and being processed with:{0}. Keep Polling the GetVehicle with regitration number";

        public const string Location_Updated_Successful = "Location has been updated successfully for vehicle :{0}";

        public const string Location_Update_Request_Submitted = "Location update request has been submitted for vehicle :{0}.Keep Polling the GetLocation API";



    }
}
