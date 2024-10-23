using Microsoft.EntityFrameworkCore;
using Domain;

namespace DAL;

public class CloudContext(DbContextOptions<CloudContext> options) : DbContext(options) {
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Customer>(entity => {
            entity.HasKey(customer => customer.Id);
        });

        modelBuilder.Entity<Order>(entity => {
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

        modelBuilder.Entity<Product>(entity => {
            entity.HasKey(product => product.Id);
            entity
                .Property(product => product.Price)
                .HasPrecision(8, 2);
        });

        modelBuilder.Entity<Comment>(entity => {
            entity.HasKey(comment => comment.Id);
            entity
                .HasOne(comment => comment.Product)
                .WithMany(product => product.Comments)
                .HasForeignKey(comment => comment.ProductId)
                .IsRequired();
        });
    }
}
