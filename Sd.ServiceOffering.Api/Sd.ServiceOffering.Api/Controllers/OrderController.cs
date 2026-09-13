using Microsoft.AspNetCore.Mvc;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;

namespace Sd.ServiceOffering.Api.Controllers;

// Inbound adapter: order requests are mapped to the order inbound port and never touch SQL directly.
[ApiController]
[Route("api/orders")]
public sealed class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet("{orderNumber}")]
    public async Task<ActionResult<OrderDomain>> GetByOrderNumber(string orderNumber, CancellationToken cancellationToken)
    {
        var order = await orderService.GetByOrderNumberAsync(orderNumber, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDomain>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orderService.CreateAsync(new OrderDomain
        {
            OrderNumber = request.OrderNumber,
            CustomerId = request.CustomerId,
            OrderStatus = request.OrderStatus,
            Products = request.Products.Select(product => new OrderProductDomain
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                CurrencyCode = product.CurrencyCode
            }).ToArray()
        }, cancellationToken);
        return CreatedAtAction(nameof(GetByOrderNumber), new { orderNumber = order.OrderNumber }, order);
    }
}

public sealed record CreateOrderRequest(
    string OrderNumber,
    int CustomerId,
    IReadOnlyList<CreateOrderProductRequest> Products,
    string OrderStatus = "Pending");

public sealed record CreateOrderProductRequest(
    int ProductId,
    int Quantity,
    decimal UnitPrice,
    string CurrencyCode);