using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace PlaceOrder.UserFacing {
    public class OrderResponse {
        [QueueOutput("placed-orders")]
        public required string[] Messages { get; set; }
        public required HttpResponseData HttpResponse { get; set; }
    }
}