using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CloudDatabases.View;

public class PlaceOrder(CloudContext database, ILoggerFactory logger) {
    private readonly ILogger _logger = logger.CreateLogger<PlaceOrder>();

    [Function(nameof(PlaceOrder))]
    public async Task<PlaceOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req) {
        var orderData = await new StreamReader(req.Body).ReadToEndAsync();
        var responseCode = HttpStatusCode.OK;
        string responseMessage;
        string? writable;

        try {
            var order = JsonSerializer.Deserialize<OrderItem>(orderData);
            responseMessage = JsonSerializer.Serialize(order);
            writable = responseMessage;
            if(!database.Products.Any(product => product.Id == order!.Product)) throw new Exception();
        } catch(Exception) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Your order could not be placed.";
            writable = null;
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
    public required string? QueuedMessages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}