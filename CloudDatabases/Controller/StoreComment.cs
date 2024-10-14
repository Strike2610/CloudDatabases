using System.Text.Json;
using Azure.Storage.Queues.Models;
using DAL;
using Domain;
using Microsoft.Azure.Functions.Worker;

namespace CloudDatabases.Controller;

public class StoreComment(CloudContext database) {
    [Function(nameof(StoreComment))]
    public void Run([QueueTrigger("placed-comments")] QueueMessage message) {
        var commentData = JsonSerializer.Deserialize<Comment>(message.Body.ToString())!;
        commentData.PostDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        database.Comments.Add(commentData);
        database.SaveChanges();
    }
}