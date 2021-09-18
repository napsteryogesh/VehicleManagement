using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScalableVehicleService.PersistentQueue
{
    public interface IPersistentConcurrentQueue<TPayload> 
    {
        Task<Guid> PublishPayload(TPayload payload);
    }

    public class PersistentConcurrentQueue<TPayload> : IPersistentConcurrentQueue<TPayload> where TPayload : IAcknowledgePayload
    {
        private readonly BlockingCollection<TPayload> persistentQueue = new BlockingCollection<TPayload>();
        private readonly IAcknowledgePayloadConsumer<TPayload> _consumer;
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly CancellationTokenSource cancellationTokenSource;
        public PersistentConcurrentQueue(IAcknowledgePayloadConsumer<TPayload> payloadConsumer,int consumerCount = 1)
        {
            _consumer = payloadConsumer ?? throw new ArgumentNullException(nameof(payloadConsumer));
            semaphoreSlim = new SemaphoreSlim(consumerCount);
            cancellationTokenSource = new CancellationTokenSource();
            while (consumerCount > 0)
            {
                Task.Run(async () =>
                {
                    await ProcessPayloadFromQueue(cancellationTokenSource.Token);
                });
                consumerCount--;
            }
        }
        public Task<Guid> PublishPayload(TPayload payload)
        {
            try
            {
                persistentQueue.TryAdd(payload);
                return Task.FromResult(payload.AcknoledgementID);
            }
            catch (InvalidOperationException exception)
            {
                throw new Exception($"paylod with {payload.AcknoledgementID} is already added in the queue.");
            }
        }


        private async Task ProcessPayloadFromQueue(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                if(persistentQueue.TryTake(out TPayload payload,Timeout.Infinite))
                {
                   await _consumer.ProcessPayload(payload);
                }
            }
        }
    }
}
