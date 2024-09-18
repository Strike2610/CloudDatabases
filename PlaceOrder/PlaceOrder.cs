using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using PlaceOrder.DTO;

namespace PlaceOrder
{
    public class PlaceOrder
    {
        [Function("PlaceOrder")]
        [QueueOutput("placed-orders")]
        public static async Task<OrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            OrderItem? data = null;
            HttpResponseData response;

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                data = JsonSerializer.Deserialize<OrderItem>(requestBody);
                response = req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            return new OrderResponse()
            {
                Messages = [data != null ? JsonSerializer.Serialize(data) : "Invalid data"],
                HttpResponse = response
            };
        }
    }
}