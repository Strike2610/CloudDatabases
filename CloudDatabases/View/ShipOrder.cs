using DAL;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace CloudDatabases.View;

public class ShipOrder(CloudContext database) {
    [Function(nameof(ShipOrder))]
    public async Task<ShippedOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship/{id:int}")] HttpRequestData req, int id) {
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Order Shipped";
        var writable = id.ToString();

        if(!database.Orders.Any(order => order.Id == id && order.ShipDate == null)) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Unable to ship order.";
            writable = null;
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new ShippedOrderResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}

public class ShippedOrderResponse {
    [QueueOutput("shipped-orders")]
    public required string? QueuedMessages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}