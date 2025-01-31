using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Responses;

public class ShippedOrderResponse {
    [QueueOutput("shipped-orders")]
    public string QueuedMessages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}