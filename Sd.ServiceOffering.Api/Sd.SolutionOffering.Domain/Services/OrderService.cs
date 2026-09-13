using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Domain.Services;

// Application service: the order use case commits the order and its products as one unit.
public sealed class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public Task<OrderDomain?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken) =>
        unitOfWork.Orders.GetByOrderNumberAsync(orderNumber, cancellationToken);

    public async Task<OrderDomain> CreateAsync(OrderDomain order, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(order.OrderNumber);
        if (order.CustomerId <= 0 || order.Products.Count == 0)
        {
            throw new ArgumentException("An order requires a customer and at least one product.", nameof(order));
        }

        var created = await unitOfWork.Orders.AddAsync(order, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return created;
    }
}