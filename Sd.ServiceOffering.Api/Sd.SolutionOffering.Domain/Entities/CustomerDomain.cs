namespace Sd.SolutionOffering.Domain.Entities;

public sealed class CustomerDomain
{
    public int CustomerId { get; init; }
    public required string CustomerNumber { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string EmailAddress { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedUtc { get; init; }
}