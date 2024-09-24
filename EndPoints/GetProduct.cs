using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CustomObjects;

namespace EndPoints {
    public class GetProduct(ILoggerFactory loggerFactory) {
        private readonly ILogger _logger = loggerFactory.CreateLogger<GetProduct>();

        [Function(nameof(GetProduct))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:int}")] HttpRequest req, int? id) {
            DbProduct dbProduct;

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var blobClient = new BlobContainerClient(connectionString, "product-thumbnails");
            var tableClient = new TableClient(connectionString, "catalogue");

            try {
                var productResponse = await tableClient.GetEntityAsync<DbProduct>("Product", id.ToString());
                dbProduct = productResponse.Value;
            } catch(Exception e) {
                _logger.LogError(e, "Error getting product with id {}", id);
                return new NotFoundResult();
            }

            return new OkObjectResult(new Dictionary<string, object>() {
                {"Thumbnail", blobClient.Uri + dbProduct.Thumbnail.ToString() + ".jpeg"},
                {"Name", dbProduct.Name},
                {"Price", dbProduct.Price}
            });
        }
    }
}
