using market_watch.Models;
using System;
using System.Collections.Generic;

namespace market_watch;

public partial class Investor
{
    public int InvestorId { get; set; }

    public string? NationalId { get; set; }

    public string? FullName { get; set; }

    public string? Nationality { get; set; }

    public string? InvestorType { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
