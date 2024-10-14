using System.Net;
using System.Text.Json;
using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CloudDatabases.View;

public class PostComment(CloudContext database, ILoggerFactory logger) {
    ILogger _logger = logger.CreateLogger(nameof(PostComment));

    [Function(nameof(PostComment))]
    public async Task<PlaceCommentResponse> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "comments/{id:int}")] HttpRequestData req, int id) {
        var commentData = await new StreamReader(req.Body).ReadToEndAsync();
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Comment Saved";
        var writable = JsonSerializer.Serialize(new Comment() {
            Content = commentData,
            ProductId = id
        });

        if(await database.Products.FindAsync(id) is null) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Product not found";
            writable = "";
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new PlaceCommentResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}

public class PlaceCommentResponse {
    [QueueOutput("placed-comments")]
    public required string QueuedMessages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}