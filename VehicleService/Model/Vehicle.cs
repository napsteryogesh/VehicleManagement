using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleService.Model
{
    public class Location
    {
        public Location(double longitude, double Latitude)
        {
            this.Longitude = longitude;
            this.Latitude = Latitude;
        }
        public double Longitude { get; private set; }

        public double Latitude { get; private set; }
    }

    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public Location Location { get; set; }
    }
}
