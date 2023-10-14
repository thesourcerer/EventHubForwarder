using Azure.Messaging.EventHubs.Producer;
using System.Text;
using Azure.Messaging.EventHubs;

namespace EventHubForwarder
{
    public static class EventHubProducer
    {
        private static EventHubProducerClient? _producerClient;

        public static void InitProducer(string? connectionString)
        {
            _producerClient = new EventHubProducerClient(connectionString);
        }

        public static async void WriteToEventHub(byte[] bytes)
        {
            if (_producerClient == null)
            {
                return;

            }
            using (var eventBatch = await _producerClient.CreateBatchAsync())
            {
                eventBatch.TryAdd(new EventData(bytes));
                await _producerClient.SendAsync(eventBatch);
            }
        }

    }


}
