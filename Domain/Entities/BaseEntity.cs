using Microsoft.WindowsAzure.Storage.Table;

namespace Domain.Entities;

public abstract class BaseEntity : TableEntity {
    public Guid Id { get; set; }
}