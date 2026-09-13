using Microsoft.AspNetCore.Mvc;
using Sd.SolutionOffering.Domain.Entities;
using Sd.SolutionOffering.Domain.Ports.In;

namespace Sd.ServiceOffering.Api.Controllers;

// Inbound adapter: controllers depend on the customer use-case port, preserving Dependency Inversion.
[ApiController]
[Route("api/customers")]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<CustomerDomain>> GetAll(CancellationToken cancellationToken) =>
        customerService.GetAllAsync(cancellationToken);

    [HttpGet("{customerId:int}")]
    public async Task<ActionResult<CustomerDomain>> GetById(int customerId, CancellationToken cancellationToken)
    {
        var customer = await customerService.GetByIdAsync(customerId, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDomain>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerService.CreateAsync(new CustomerDomain
        {
            CustomerNumber = request.CustomerNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailAddress = request.EmailAddress,
            IsActive = request.IsActive
        }, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { customerId = customer.CustomerId }, customer);
    }
}

public sealed record CreateCustomerRequest(
    string CustomerNumber,
    string FirstName,
    string LastName,
    string EmailAddress,
    bool IsActive = true);