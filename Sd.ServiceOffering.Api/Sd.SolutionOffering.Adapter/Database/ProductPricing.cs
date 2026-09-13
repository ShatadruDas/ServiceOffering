using System;
using System.Collections.Generic;

namespace Sd.SolutionOffering.Adapter.Db.Database;

public partial class ProductPricing
{
    public int PricingId { get; set; }

    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int IsCurrent { get; set; }

    public virtual Product Product { get; set; } = null!;
}
