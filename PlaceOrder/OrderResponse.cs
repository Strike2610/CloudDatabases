using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace PlaceOrder {
    public class OrderResponse {
        [QueueOutput("placed-orders")]
        public required string[] Messages { get; set; }
        public required HttpResponseData HttpResponse { get; set; }
    }
}