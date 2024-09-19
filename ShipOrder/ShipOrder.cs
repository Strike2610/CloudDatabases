using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PlaceOrder;
using PlaceOrder.UserFacing;
using System.Net;
using System.Text.Json;

namespace ShipOrder {
    public class ShipOrder {
        [Function(nameof(ShipOrder))]
        public async Task<ShippedResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship")] HttpRequestData req,
            [TableInput("orders", "Ordered")] IEnumerable<DbOrder> orders,
            FunctionContext context) {
            var logger = context.GetLogger(nameof(ShipOrder));

            var orderNumber = await new StreamReader(req.Body).ReadToEndAsync();
            DbOrder? order = null;
            foreach(var order1 in orders) {
                logger.LogDebug(order1.RowKey);
            }

            try {
                order = orders.First(row => row.RowKey == orderNumber);
            } catch(Exception) {
                logger.LogWarning("Order not found: {}", orderNumber);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(order);

            logger.LogInformation("Order shipped: {}", orderNumber);
            return new ShippedResponse() {
                Messages = [JsonSerializer.Serialize(order)],
                HttpResponse = response
            };
        }
    }
}
