using Domain.Entities;
using Domain.Interfaces;
using Domain.Responses;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Application.Endpoints;

public interface IPostCommentComponent {
    Task<PostCommentResponse> Execute(HttpRequestData req, Guid id);
}

public class PostCommentComponent(IProductRepository productRepository) : IPostCommentComponent {
    public async Task<PostCommentResponse> Execute(HttpRequestData req, Guid id) {
        var commentData = await new StreamReader(req.Body).ReadToEndAsync();
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Comment Saved";

        var product = await productRepository.Get(id);

        var writable = JsonSerializer.Serialize(new Comment {
            Content = commentData,
            Product = product ?? new Product()
        });

        if(product == null || commentData == "") {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Unable to store comment.";
            writable = "";
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new PostCommentResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}