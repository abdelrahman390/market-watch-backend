using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class DailyPrice
{
    public long PriceId { get; set; }

    public int? CompanyId { get; set; }

    public DateOnly? TradeDate { get; set; }

    public decimal? OpenPrice { get; set; }

    public decimal? HighPrice { get; set; }

    public decimal? LowPrice { get; set; }

    public decimal? ClosePrice { get; set; }

    public long? Volume { get; set; }

    public decimal? ValueTraded { get; set; }

    public virtual Company? Company { get; set; }
}
