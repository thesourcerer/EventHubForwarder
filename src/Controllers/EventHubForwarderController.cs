using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventHubForwarder.Controllers
{
    [ApiController]
    [Route("/")]
    public class EventHubForwarderController : ControllerBase
    {
        private readonly ILogger<EventHubForwarderController> _logger;

        public EventHubForwarderController(ILogger<EventHubForwarderController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public async Task SendPayload()
        {
            try
            {
                EventHubProducer.WriteToEventHub(await GetBytesRequestBody());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private async Task<byte[]> GetBytesRequestBody()
        {
            var memoryStream = new MemoryStream();
            try
            {
                await HttpContext.Request.Body.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
            finally
            {
                await memoryStream.DisposeAsync();
            }

        }

    }
}