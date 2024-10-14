using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using DAL;

namespace CloudDatabases.View;

public class ShipOrder(CloudContext database) {
    [Function(nameof(ShipOrder))]
    public async Task<ShippedOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship/{id:int}")] HttpRequestData req, int id) {
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Order Shipped";
        var writable = id.ToString();

        if(!database.Orders.Any(order => order.Id == id)) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Order not found";
            writable = "";
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
    public string QueuedMessages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}