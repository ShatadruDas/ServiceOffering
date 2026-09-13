using Microsoft.AspNetCore.Mvc;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;

namespace Sd.ServiceOffering.Api.Controllers;

// Inbound adapter: HTTP is translated into the Domain inbound port at this boundary.
[ApiController]
[Route("api/products")]
public sealed class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken) =>
        productService.GetAllAsync(cancellationToken);

    [HttpGet("{productId:int}")]
    public async Task<ActionResult<Product>> GetById(int productId, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(productId, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.CreateAsync(new Product
        {
            ProductCode = request.ProductCode,
            ProductName = request.ProductName,
            Description = request.Description,
            IsActive = request.IsActive
        }, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { productId = product.ProductId }, product);
    }
}

public sealed record CreateProductRequest(
    string ProductCode,
    string ProductName,
    string? Description,
    bool IsActive = true);