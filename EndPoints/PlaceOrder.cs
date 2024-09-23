using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using CustomObjects;

namespace EndPoints {
    public class PlaceOrder {
        [Function(nameof(PlaceOrder))]
        public static async Task<OrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req, FunctionContext context) {
            var logger = context.GetLogger(nameof(PlaceOrder));
            OrderItem? orderJson = null;

            var response = req.CreateResponse(HttpStatusCode.OK);
            var orderData = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                orderJson = JsonSerializer.Deserialize<OrderItem>(orderData);
                await response.WriteAsJsonAsync(orderJson);
                logger.LogInformation("Order processed for {}", orderJson?.Customer);
            } catch(Exception e) {
                logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return new OrderResponse() {
                Messages = [JsonSerializer.Serialize(orderJson)],
                HttpResponse = response
            };
        }
    }
}