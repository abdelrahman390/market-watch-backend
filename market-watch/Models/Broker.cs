using market_watch.Models;
using System;
using System.Collections.Generic;

namespace market_watch;

public partial class Broker
{
    public int BrokerId { get; set; }

    public string? BrokerCode { get; set; }

    public string? BrokerName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
