using Microsoft.EntityFrameworkCore;
using Sd.SolutionOffering.Adapter.Db.Database;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.Out;

namespace Sd.SolutionOffering.Adapter.Database;

public sealed class SqlOrderRepository(SqlUnitOfWork unitOfWork) : IOrderRepository
{
    public async Task<OrderDomain?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken)
    {
        var orderRow = await unitOfWork.Context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(row => row.OrderNumber == orderNumber, cancellationToken);
        if (orderRow is null)
        {
            return null;
        }

        var products = await unitOfWork.Context.OrderItems
            .AsNoTracking()
            .Where(row => row.OrderId == orderRow.OrderId)
            .Select(row => new OrderProductDomain
            {
                ProductId = row.ProductId,
                Quantity = row.Quantity,
                UnitPrice = row.UnitPrice,
                CurrencyCode = "USD"
            })
            .ToListAsync(cancellationToken);

        return new OrderDomain
        {
            OrderId = orderRow.OrderId,
            OrderNumber = orderRow.OrderNumber,
            CustomerId = orderRow.CustomerId,
            OrderStatus = orderRow.Status,
            OrderedUtc = orderRow.OrderDate,
            Products = products
        };
    }

    public async Task<OrderDomain> AddAsync(OrderDomain order, CancellationToken cancellationToken)
    {
        await unitOfWork.EnsureTransactionAsync(cancellationToken);
        var orderRow = new Order
        {
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            Status = order.OrderStatus,
            OrderDate = order.OrderedUtc
        };
        unitOfWork.Context.Orders.Add(orderRow);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var productRows = order.Products.Select(product => new OrderItem
        {
            OrderId = orderRow.OrderId,
            ProductId = product.ProductId,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice
        }).ToList();

        unitOfWork.Context.OrderItems.AddRange(productRows);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new OrderDomain
        {
            OrderId = orderRow.OrderId,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus,
            OrderedUtc = orderRow.OrderDate,
            Products = order.Products
        };
    }
}