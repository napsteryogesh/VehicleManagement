using System;
using System.Collections.Generic;
using System.Text;
using ScalableVehicleService.Model;
using ScalableVehicleService.PersistentQueue;

namespace ScalableVehicleService.Services.Payload
{
    public enum VehicleAction
    {
        Register,
        RecordLocation
    }
    public class VehiclePayload : IAcknowledgePayload
    {
        public VehiclePayload(VehicleAction action, Vehicle vehicle,Guid ackId)
        {
            Action = action;
            Vehicle = vehicle;
            AcknoledgementID = ackId;
        }
        public VehicleAction Action { get; private set; }

        public Vehicle Vehicle { get; private set; }

        public Guid AcknoledgementID {get; set;}

        public bool IsProcessed { get; set; }
    }
}
