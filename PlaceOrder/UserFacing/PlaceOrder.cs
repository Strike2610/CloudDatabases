using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace PlaceOrder.UserFacing {
    public class PlaceOrder {
        [Function(nameof(PlaceOrder))]
        public static async Task<OrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context) {
            var logger = context.GetLogger(nameof(PlaceOrder));
            OrderItem? data = null;
            HttpResponseData response;

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            response = req.CreateResponse(HttpStatusCode.OK);
            try {
                data = JsonSerializer.Deserialize<OrderItem>(requestBody);
            } catch(Exception e) {
                logger.LogWarning("Something went wrong: {}", e);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            logger.LogInformation("Order processed for {}", data?.Customer);
            return new OrderResponse() {
                Messages = [data != null ? JsonSerializer.Serialize(data) : "Invalid data"],
                HttpResponse = response
            };
        }
    }
}