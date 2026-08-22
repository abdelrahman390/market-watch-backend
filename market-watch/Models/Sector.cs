using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class Sector
{
    public int SectorId { get; set; }

    public string SectorName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
