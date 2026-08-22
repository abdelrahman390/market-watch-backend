using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? Symbol { get; set; }

    public string? CompanyName { get; set; }

    public int? SectorId { get; set; }

    public int? MarketId { get; set; }

    public decimal? FaceValue { get; set; }

    public DateOnly? ListedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<DailyPrice> DailyPrices { get; set; } = new List<DailyPrice>();

    public virtual Market? Market { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Sector? Sector { get; set; }

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
