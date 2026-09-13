using Microsoft.Extensions.DependencyInjection;
using Sd.SolutionOffering.Adapter.Database;
using Sd.SolutionOffering.Domain.Ports.In;
using Sd.SolutionOffering.Domain.Ports.Out;
using Sd.SolutionOffering.Domain.Services;

namespace Sd.SolutionOffering.Adapter.Db;

public static class DependencyInjection
{
    // Composition root helper: adapters are selected here while Domain remains framework independent.
    public static IServiceCollection AddServiceOfferingAdapters(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<SqlUnitOfWork>(_ => new SqlUnitOfWork(connectionString));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SqlUnitOfWork>());
        services.AddScoped<IProductRepository>(provider => provider.GetRequiredService<SqlUnitOfWork>().Products);
        services.AddScoped<ICustomerRepository>(provider => provider.GetRequiredService<SqlUnitOfWork>().Customers);
        services.AddScoped<IOrderRepository>(provider => provider.GetRequiredService<SqlUnitOfWork>().Orders);
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}