using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public int CompanyId { get; set; }

    public int InvestorId { get; set; }

    public int BrokerId { get; set; }

    public string? OrderType { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? OrderStatus { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual Broker Broker { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Investor Investor { get; set; } = null!;

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
