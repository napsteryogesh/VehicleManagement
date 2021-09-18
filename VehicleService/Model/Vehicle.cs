using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScalableVehicleService.Model
{
    public class Location
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Location(double longitude, double Latitude)
        {
            this.Longitude = longitude;
            this.Latitude = Latitude;
           // this.Vehicle = vehicle;
        }


        public double Longitude { get; private set; }

        public double Latitude { get; private set; }

       // public Vehicle Vehicle { get; set; }
    }

    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public Location Location { get; set; }

    }
}
