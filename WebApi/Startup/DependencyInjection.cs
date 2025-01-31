using Application.Endpoints;
using Application.Processing;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Startup;

public static class DependencyInjection {
    public static void ConfigureDependencies(this IServiceCollection services) {
        ConfigureApplicationDependencies(services);
        ConfigureInfrastructureDependencies(services);
    }

    private static void ConfigureInfrastructureDependencies(IServiceCollection services) {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    private static void ConfigureApplicationDependencies(IServiceCollection services) {
        services.AddScoped<IGetProductComponent, GetProductComponent>();
        services.AddScoped<IPlaceOrderComponent, PlaceOrderComponent>();
        services.AddScoped<IPostCommentComponent, PostCommentComponent>();
        services.AddScoped<IShipOrderComponent, ShipOrderComponent>();
        services.AddScoped<IProcessOrderComponent, ProcessOrderComponent>();
        services.AddScoped<IProcessShipmentComponent, ProcessShipmentComponent>();
        services.AddScoped<IStoreCommentComponent, StoreCommentComponent>();
    }
}