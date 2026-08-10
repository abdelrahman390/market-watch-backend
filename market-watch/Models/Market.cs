using System;
using System.Collections.Generic;

namespace market_watch;

public partial class Market
{
    public int MarketId { get; set; }

    public string MarketName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Companies> Companies { get; set; } = new List<Companies>();
}
