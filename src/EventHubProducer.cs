using Azure.Messaging.EventHubs.Producer;
using System.Text;
using Azure.Messaging.EventHubs;

namespace EventHubForwarder
{
    public static class EventHubProducer
    {
        private static EventHubBufferedProducerClient? _producerClient;
        private static long _totalReceived;
        private static long _totalSent;
        private static long _totalFailed;
        private static long _lastTotalSent;
        private static DateTime _lastStatsTime = DateTime.UtcNow;

        public static void InitProducer(string? connectionString)
        {
            _producerClient = new EventHubBufferedProducerClient(connectionString);
            _producerClient.SendEventBatchFailedAsync += ProducerClient_SendEventBatchFailedAsync;
            _producerClient.SendEventBatchSucceededAsync += ProducerClient_SendEventBatchSucceededAsync;
        }



        public static async void WriteToEventHub(byte[] bytes)
        {
            if (_producerClient == null)
            {
                return;

            }

            Interlocked.Increment(ref _totalReceived);

            if (_totalReceived % 10000 == 0)
            {
                var now = DateTime.UtcNow;
                var sendRate = (long)((_totalSent - _lastTotalSent) / (now - _lastStatsTime).TotalSeconds);
                Console.WriteLine($"{DateTime.UtcNow} : Dispatched={_totalReceived} Sent={_totalSent} Failed={_totalFailed} Rate:{sendRate}/sec");
                _lastTotalSent = _totalSent;
                _lastStatsTime = now;
            }

            await _producerClient.EnqueueEventAsync(new EventData(bytes));
        }
        private static Task ProducerClient_SendEventBatchFailedAsync(SendEventBatchFailedEventArgs arg)
        {
            
            Interlocked.Add(ref _totalFailed, arg.EventBatch.Count);
            Console.WriteLine(arg.Exception);
            return Task.CompletedTask;
        }

        private static Task ProducerClient_SendEventBatchSucceededAsync(SendEventBatchSucceededEventArgs arg)
        {
            Interlocked.Add(ref _totalSent, arg.EventBatch.Count);
            return Task.CompletedTask;
        }
    }


}
