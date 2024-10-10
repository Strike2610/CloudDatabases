using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using Domain;

namespace CloudDatabases.View;

public class PlaceOrder {
    [Function(nameof(PlaceOrder))]
    public async Task<PlaceOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req) {
        var orderData = await new StreamReader(req.Body).ReadToEndAsync();
        var order = JsonSerializer.Deserialize<OrderItem>(orderData);

        var responseCode = HttpStatusCode.OK;
        var responseMessage = JsonSerializer.Serialize(order);
        var writable = JsonSerializer.Serialize(order);

        if(order == null) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Your order could not be processed";
            writable = "";
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new PlaceOrderResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}

public class PlaceOrderResponse {
    [QueueOutput("placed-orders")]
    public required string QueuedMessages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}