using Domain.Entities;
using Domain.Interfaces;
using Domain.Responses;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace Application.Endpoints;

public interface IPlaceOrderComponent {
    Task<PlaceOrderResponse> Execute(HttpRequestData req);
}

public class PlaceOrderComponent(IProductRepository productRepository) : IPlaceOrderComponent {
    public async Task<PlaceOrderResponse> Execute(HttpRequestData req) {
        var orderData = await new StreamReader(req.Body).ReadToEndAsync();
        var responseCode = HttpStatusCode.OK;

        var order = JsonSerializer.Deserialize<Order>(orderData);
        var responseMessage = JsonSerializer.Serialize(order);
        var writable = responseMessage;

        if(order == null || !await productRepository.Exists(order.ProductId)) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Your order could not be placed.";
            writable = "";
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new PlaceOrderResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}