using Microsoft.EntityFrameworkCore;
using Domain;

namespace DAL;

public class CloudContext(DbContextOptions<CloudContext> options) : DbContext(options) {
    public DbSet<ICustomer> Customers { get; set; }
    public DbSet<IOrder> Orders { get; set; }
    public DbSet<IProduct> Products { get; set; }
    public DbSet<IComment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ICustomer>(entity => {
            entity.HasKey(customer => customer.Id);
        });

        modelBuilder.Entity<IOrder>(entity => {
            entity.HasKey(order => order.Id);
            entity
                .HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId)
                .IsRequired();
            entity
                .HasOne(order => order.Product)
                .WithMany()
                .HasForeignKey(order => order.ProductId)
                .IsRequired();
            entity.Property(order => order.OrderProcessed)
                .HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<IProduct>(entity => {
            entity.HasKey(product => product.Id);
            entity
                .Property(product => product.Price)
                .HasPrecision(8, 2);
        });

        modelBuilder.Entity<IComment>(entity => {
            entity.HasKey(comment => comment.Id);
            entity
                .HasOne(comment => comment.Product)
                .WithMany(product => product.Comments)
                .HasForeignKey(comment => comment.ProductId)
                .IsRequired();
        });
    }
}
