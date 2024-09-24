using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using CustomObjects;

namespace EndPoints {
    public class PlaceOrder(ILoggerFactory loggerFactory) {
        private readonly ILogger _logger = loggerFactory.CreateLogger<PlaceOrder>();

        [Function(nameof(PlaceOrder))]
        public async Task<PlaceOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req) {
            var response = req.CreateResponse(HttpStatusCode.OK);
            var orderData = await new StreamReader(req.Body).ReadToEndAsync();
            OrderItem? orderJson = null;
            try {
                orderJson = JsonSerializer.Deserialize<OrderItem>(orderData);
                await response.WriteAsJsonAsync(orderJson);
                _logger.LogInformation("Order processed for {}", orderJson?.Customer);
            } catch(Exception e) {
                _logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return new PlaceOrderResponse {
                Messages = [JsonSerializer.Serialize(orderJson)],
                HttpResponse = response
            };
        }
    }

    public class PlaceOrderResponse {
        [QueueOutput("placed-orders")]
        public string[] Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}