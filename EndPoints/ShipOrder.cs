using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using CustomObjects;

namespace EndPoints {
    public class ShipOrder(ILoggerFactory loggerFactory) {
        private readonly ILogger _logger = loggerFactory.CreateLogger<ShipOrder>();

        [Function(nameof(ShipOrder))]
        public async Task<ShippedOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship")] HttpRequestData req, [TableInput("orders", "Ordered")] IEnumerable<DbOrder> orders) {
            var response = req.CreateResponse(HttpStatusCode.OK);
            var orderNumber = await new StreamReader(req.Body).ReadToEndAsync();
            DbOrder? order = null;
            try {
                order = orders.First(row => row.RowKey == orderNumber);
                await response.WriteAsJsonAsync(order);
                _logger.LogInformation("Order shipped: {}", orderNumber);
            } catch(Exception e) {
                _logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return new ShippedOrderResponse() {
                Messages = [JsonSerializer.Serialize(order)],
                HttpResponse = response
            };
        }
    }

    public class ShippedOrderResponse {
        [QueueOutput("shipped-orders")]
        public string[] Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}