using Application.Endpoints;
using Domain.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace WebApi.Controllers;

public class Endpoints(
    IGetProductComponent getProductComponent,
    IPlaceOrderComponent placeOrderComponent,
    IPostCommentComponent postCommentComponent,
    IShipOrderComponent shipOrderComponent
    ) {
    [Function(nameof(GetProduct))]
    public async Task<HttpResponseData> GetProduct([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id:Guid}")] HttpRequestData req, Guid id) {
        return await getProductComponent.Execute(req, id);
    }

    [Function(nameof(PlaceOrder))]
    public async Task<PlaceOrderResponse> PlaceOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/place")] HttpRequestData req) {
        return await placeOrderComponent.Execute(req);
    }

    [Function(nameof(PostComment))]
    public async Task<PostCommentResponse> PostComment([HttpTrigger(AuthorizationLevel.Function, "post", Route = "comments/{id:Guid}")] HttpRequestData req, Guid id) {
        return await postCommentComponent.Execute(req, id);
    }

    [Function(nameof(ShipOrder))]
    public async Task<ShippedOrderResponse> ShipOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "order/ship/{id:Guid}")] HttpRequestData req, Guid id) {
        return await shipOrderComponent.Execute(req, id);
    }
}