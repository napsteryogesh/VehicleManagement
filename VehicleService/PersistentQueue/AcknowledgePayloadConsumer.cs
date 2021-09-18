using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScalableVehicleService.PersistentQueue
{
    public interface IAcknowledgePayload
    {
        public Guid AcknoledgementID { get; }
        public bool IsProcessed { get; }
    }

    public interface IAcknowledgePayloadConsumer<TPayload> where TPayload : IAcknowledgePayload
    {
        Task<bool> ProcessPayload(TPayload payload);
    }
}
