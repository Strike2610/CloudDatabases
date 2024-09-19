using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PlaceOrder.UserFacing {
    public class PlaceOrder {
        [Function("PlaceOrder")]
        public static async Task<OrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context) {
            var logger = context.GetLogger("PlaceOrder");
            OrderItem? data = null;
            HttpResponseData response;

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            response = req.CreateResponse(HttpStatusCode.OK);
            try {
                data = JsonSerializer.Deserialize<OrderItem>(requestBody);
            } catch(Exception) {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            return new OrderResponse() {
                Messages = [data != null ? JsonSerializer.Serialize(data) : "Invalid data"],
                HttpResponse = response
            };
        }
    }
}