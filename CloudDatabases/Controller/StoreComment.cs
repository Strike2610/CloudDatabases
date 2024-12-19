using Azure.Storage.Queues.Models;
using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

namespace CloudDatabases.Controller;

public class StoreComment(CloudContext database) {
    [Function(nameof(StoreComment))]
    public async Task RunAsync([QueueTrigger("placed-comments")] QueueMessage message) {
        var commentData = JsonSerializer.Deserialize<IComment>(message.Body.ToString())!;
        commentData.PostDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        database.Comments.Add(commentData);
        await database.SaveChangesAsync();
    }
}