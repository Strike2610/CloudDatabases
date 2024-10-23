using Azure.Storage.Blobs;
using DAL;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace CloudDatabases.View;

public class GetProduct(CloudContext database) {
    [Function(nameof(GetProduct))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:int}")] HttpRequestData req, int? id) {
        var blobClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "product-thumbnails");
        var product = database.Products.FirstOrDefault(product => product.Id == id);
        HttpResponseData response;

        if(product == null) {
            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Product not found.");
            return response;
        }

        response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new Dictionary<string, object> {
            {"Name", product.Name},
            {"Price", product.Price},
            {"Thumbnail", blobClient.Uri.AbsoluteUri + product.Thumbnail},
            {"Comments", database.Comments.Where(comment => comment.ProductId == id).Select(comment => new {
                comment.PostDate,
                comment.Content
            })}
        });

        return response;
    }
}
