using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class Trade
{
    public long TradeId { get; set; }

    public long? OrderId { get; set; }

    public int? CompanyId { get; set; }

    public int? InvestorId { get; set; }

    public int? BrokerId { get; set; }

    public DateTime? TradeDate { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? TotalValue { get; set; }

    public string? Side { get; set; }

    public virtual Broker? Broker { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Investor? Investor { get; set; }

    public virtual Order? Order { get; set; }
}
