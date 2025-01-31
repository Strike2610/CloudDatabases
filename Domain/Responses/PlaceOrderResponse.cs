using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Responses;

public class PlaceOrderResponse {
    [QueueOutput("placed-orders")]
    public string QueuedMessages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}