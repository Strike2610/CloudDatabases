using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using CustomObjects;

namespace EndPoints {
    public class ShipOrder {
        [Function(nameof(ShipOrder))]
        public static async Task<ShippedResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship")] HttpRequestData req, [TableInput("orders", "Ordered")] IEnumerable<DbOrder> orders, FunctionContext context) {
            var logger = context.GetLogger(nameof(ShipOrder));
            DbOrder? order = null;

            var response = req.CreateResponse(HttpStatusCode.OK);
            var orderNumber = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                order = orders.First(row => row.RowKey == orderNumber);
                await response.WriteAsJsonAsync(order);
                logger.LogInformation("Order shipped: {}", orderNumber);
            } catch(Exception e) {
                logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return new ShippedResponse() {
                Messages = [JsonSerializer.Serialize(order)],
                HttpResponse = response
            };
        }
    }
}