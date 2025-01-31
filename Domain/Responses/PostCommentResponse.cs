using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Responses;

public class PostCommentResponse {
    [QueueOutput("placed-comments")]
    public string QueuedMessages { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}