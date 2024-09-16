using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PlaceOrder
{
    public class PlaceOrder
    {
        private readonly ILogger<PlaceOrder> _logger;

        public PlaceOrder(ILogger<PlaceOrder> logger)
        {
            _logger = logger;
        }

        [Function("PlaceOrder")]
        [QueueOutput("placed-orders")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
