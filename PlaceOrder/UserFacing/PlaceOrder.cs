using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace PlaceOrder.UserFacing {
    public class PlaceOrder {
        [Function(nameof(PlaceOrder))]
        public static async Task<OrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req, FunctionContext context) {
            var logger = context.GetLogger(nameof(PlaceOrder));
            OrderItem? orderJson = null;
            HttpResponseData response;

            var orderData = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                orderJson = JsonSerializer.Deserialize<OrderItem>(orderData);
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(orderJson);
            } catch(Exception e) {
                logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            logger.LogInformation("Order processed for {}", orderJson?.Customer);
            return new OrderResponse() {
                Messages = [orderJson != null ? JsonSerializer.Serialize(orderJson) : "Invalid data"],
                HttpResponse = response
            };
        }
    }
}