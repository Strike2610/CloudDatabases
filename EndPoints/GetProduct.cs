using System.Net;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using EntityFramework;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EndPoints;

public class GetProduct(CloudDbContext database, ILoggerFactory logger) {
    private readonly ILogger _logger = logger.CreateLogger<GetProduct>();

    [Function(nameof(GetProduct))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:int}")] HttpRequestData req, int? id) {
        var blobClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "product-thumbnails");
        Product product;
        try {
            product = database.Products.First(product => product.Id == id);
        } catch(Exception e) {
            _logger.LogWarning(e.Message);
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new Dictionary<string, object> {
            {"Thumbnail", blobClient.Uri.AbsoluteUri + product.Thumbnail},
            {"Name", product.Name},
            {"Price", product.Price}
        });

        return response;
    }
}