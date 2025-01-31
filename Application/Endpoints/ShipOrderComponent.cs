using Domain.Interfaces;
using Domain.Responses;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Application.Endpoints;

public interface IShipOrderComponent {
    Task<ShippedOrderResponse> Execute(HttpRequestData req, Guid id);
}

public class ShipOrderComponent(IOrderRepository orderRepository) : IShipOrderComponent {
    public async Task<ShippedOrderResponse> Execute(HttpRequestData req, Guid id) {
        var responseCode = HttpStatusCode.OK;
        var responseMessage = "Order Shipped";
        var writable = id.ToString();

        var order = await orderRepository.Get(id);

        if(order is not { ShipDate: null }) {
            responseCode = HttpStatusCode.BadRequest;
            responseMessage = "Unable to ship order.";
            writable = "";
        }

        var response = req.CreateResponse(responseCode);
        await response.WriteStringAsync(responseMessage);
        return new ShippedOrderResponse {
            QueuedMessages = writable,
            HttpResponse = response
        };
    }
}