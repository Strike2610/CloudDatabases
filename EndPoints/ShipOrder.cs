using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using EntityFramework;

namespace EndPoints;

public class ShipOrder(CloudDbContext database) {

    [Function(nameof(ShipOrder))]
    public async Task<ShippedOrderResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship/{id:int}")] HttpRequestData req, int id) {
        var response = req.CreateResponse(HttpStatusCode.OK);

        if(!database.Orders.Any(order => order.Id == id)) return new ShippedOrderResponse {
            Messages = "Order not found!",
            HttpResponse = req.CreateResponse(HttpStatusCode.BadRequest)
        };

        await response.WriteStringAsync(id.ToString());

        return new ShippedOrderResponse {
            Messages = "Order Shipped",
            HttpResponse = response
        };
    }
}

public class ShippedOrderResponse {
    [QueueOutput("shipped-orders")]
    public string Messages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}