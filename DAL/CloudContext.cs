using Microsoft.EntityFrameworkCore;
using Domain;

namespace DAL;

public class CloudContext(DbContextOptions<CloudContext> options) : DbContext(options) {
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Customer>(entity => {
            entity.HasKey(customer => customer.Id);
            entity
                .Property(customer => customer.Id)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Order>(entity => {
            entity.HasKey(order => order.Id);
            entity
                .Property(order => order.Id)
                .ValueGeneratedOnAdd();
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
                .Property(product => product.Id)
                .ValueGeneratedOnAdd();
            entity
                .Property(product => product.Price)
                .HasPrecision(8, 2);
        });
    }
}
