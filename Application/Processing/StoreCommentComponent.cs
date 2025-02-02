using System.Globalization;
using Azure.Storage.Queues.Models;
using Domain.Entities;
using Domain.Interfaces;
using System.Text.Json;

namespace Application.Processing;

public interface IStoreCommentComponent {
    Task Execute(QueueMessage message);
}

public class StoreCommentComponent(ICommentRepository commentRepository) : IStoreCommentComponent {
    public async Task Execute(QueueMessage message) {
        var commentData = JsonSerializer.Deserialize<Comment>(message.Body.ToString())!;
        commentData.PostDate = message.InsertedOn ?? DateTimeOffset.MinValue;
        await commentRepository.Add(commentData);
        await commentRepository.SaveChanges();
    }
}