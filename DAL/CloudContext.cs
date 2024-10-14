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

        modelBuilder.Entity<Customer>().HasData(
            new Customer() { Id = 1, Name = "Anakin Skywalker", Address = "Jedi Temple" },
            new Customer() { Id = 2, Name = "Harry Potter", Address = "Privet Drive 4" },
            new Customer() { Id = 3, Name = "Bilbo Baggins", Address = "Bag End" }
        );
        modelBuilder.Entity<Product>().HasData(
            new Product() { Id = 1, Name = "Lightsaber", Price = 200, Thumbnail = "bf4084e2-c63d-4a26-a1f8-29585d107b33.jpg" },
            new Product() { Id = 2, Name = "Wand", Price = 5, Thumbnail = "b2ac5f77-a354-47f0-a69d-69a01c268385.jpg" },
            new Product() { Id = 3, Name = "Ring", Price = 999, Thumbnail = "236eb394-4170-4e3a-ad68-20bd9c592ddf.jpg" }
        );
        modelBuilder.Entity<Order>().HasData(
            new Order() { Id = 1, CustomerId = 1, ProductId = 3, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(16)) },
            new Order() { Id = 2, CustomerId = 2, ProductId = 1, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(10)) },
            new Order() { Id = 3, CustomerId = 3, ProductId = 2, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(66)) },
            new Order() { Id = 4, CustomerId = 1, ProductId = 1, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(51)), ShipDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(51)).AddHours(10), OrderProcessed = TimeSpan.FromHours(10) },
            new Order() { Id = 5, CustomerId = 2, ProductId = 2, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(92)), ShipDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(92)).AddHours(11), OrderProcessed = TimeSpan.FromHours(11) },
            new Order() { Id = 6, CustomerId = 3, ProductId = 3, OrderDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(94)), ShipDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(94)).AddHours(23), OrderProcessed = TimeSpan.FromHours(23) }
        );
    }
}
