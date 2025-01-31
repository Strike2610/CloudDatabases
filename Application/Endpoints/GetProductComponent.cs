using Azure.Storage.Blobs;
using Domain.Interfaces;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Application.Endpoints;

public interface IGetProductComponent {
    Task<HttpResponseData> Execute(HttpRequestData req, Guid id);
}

public class GetProductComponent(IProductRepository productRepository, ICommentRepository commentRepository) : IGetProductComponent {
    public async Task<HttpResponseData> Execute(HttpRequestData req, Guid id) {
        var blobClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "product-thumbnails");
        var product = await productRepository.Get(id);
        HttpResponseData response;

        if(product == null) {
            response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync("Product not found.");
            return response;
        }

        response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new Dictionary<string, object> {
            {"Name", product.Name},
            {"Price", product.Price},
            {"Thumbnail", blobClient.Uri.AbsoluteUri + product.Thumbnail},
            {"Comments", commentRepository.GetAllByProduct(product)}
        });

        return response;
    }
}