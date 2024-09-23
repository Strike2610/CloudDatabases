using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CustomObjects;

namespace EndPoints {
    public class GetProduct {
        [Function(nameof(GetProduct))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:int}")] HttpRequest req, int? id, FunctionContext context) {
            var logger = context.GetLogger(nameof(GetProduct));
            Product product;

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var blobClient = new BlobContainerClient(connectionString, "product-thumbnails");
            var tableClient = new TableClient(connectionString, "catalogue");

            try {
                var productResponse = await tableClient.GetEntityAsync<Product>("Product", id.ToString());
                product = productResponse.Value;
            } catch(Exception e) {
                logger.LogError(e, "Error getting product with id {}", id);
                return new NotFoundResult();
            }

            logger.LogInformation(blobClient.Uri.ToString());

            //blobClient.GetBlobClient(product.Thumbnail.ToString()).DownloadTo(Response.Body);

            return new OkObjectResult(product);
        }
    }
}
