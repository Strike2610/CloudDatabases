using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using EntityFramework;

namespace EndPoints;

public class PlaceOrder {
    [Function(nameof(PlaceOrder))]
    public async Task<PlaceOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req) {
        var orderData = await new StreamReader(req.Body).ReadToEndAsync();

        var order = JsonSerializer.Deserialize<OrderItem>(orderData);
        if(order == null) return new PlaceOrderResponse {
            Messages = "Your order could not be processed",
            HttpResponse = req.CreateResponse(HttpStatusCode.BadRequest)
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(order);

        return new PlaceOrderResponse {
            Messages = JsonSerializer.Serialize(order),
            HttpResponse = response
        };
    }
}

public class PlaceOrderResponse {
    [QueueOutput("placed-orders")]
    public required string Messages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}