using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace PlaceOrder {
    public class OrderResponse {
        [QueueOutput("placed-orders")]
        public string[] Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}