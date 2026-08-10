using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class News
{
    public int NewsId { get; set; }

    public int? CompanyId { get; set; }

    public string? NewsTitle { get; set; }

    public string? NewsBody { get; set; }

    public DateTime? PublishDate { get; set; }

    public int? CreatedBy { get; set; }

    public virtual Companies? Company { get; set; }

    public virtual User? CreatedByNavigation { get; set; }
}
