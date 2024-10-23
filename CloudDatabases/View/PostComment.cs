using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace CloudDatabases.View;

public class PostComment(CloudContext database) {
    [Function(nameof(PostComment))]
    public async Task<PlaceCommentResponse> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "comments/{id:int}")] HttpRequestData req, int id) {
        var commentData = await new StreamReader(req.Body).ReadToEndAsync();
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Comment Saved";
        var writable = JsonSerializer.Serialize(new Comment {
            Content = commentData,
            ProductId = id
        });

        if(database.Products.Find(id) == null || commentData == "") {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Unable to store comment.";
            writable = null;
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
    public required string? QueuedMessages { get; set; }
    public required HttpResponseData HttpResponse { get; set; }
}