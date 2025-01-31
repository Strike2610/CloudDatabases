using Application.Processing;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;

namespace WebApi.Controllers;

public class Processing(
    IProcessOrderComponent processOrderComponent,
    IProcessShipmentComponent processShipmentComponent,
    IStoreCommentComponent storeCommentComponent
    ) {
    [Function(nameof(ProcessOrder))]
    public async Task ProcessOrder([QueueTrigger("placed-orders")] QueueMessage message) {
        await processOrderComponent.Execute(message);
    }

    [Function(nameof(ProcessShipment))]
    public async Task ProcessShipment([QueueTrigger("placed-orders")] QueueMessage message) {
        await processShipmentComponent.Execute(message);
    }

    [Function(nameof(StoreComment))]
    public async Task StoreComment([QueueTrigger("placed-orders")] QueueMessage message) {
        await storeCommentComponent.Execute(message);
    }
}