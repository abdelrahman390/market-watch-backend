using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class AddCompaniesRequest
{
    public int RequistId { get; set; }

    public int? UserId { get; set; }

    public int MangerId { get; set; }

    public string? Status { get; set; }

    public string? Symbol { get; set; }

    public string? CompanyName { get; set; }

    public int? SectorId { get; set; }

    public int? MarketId { get; set; }

    public decimal? FaceValue { get; set; }

    public DateOnly? ListedDate { get; set; }

    public bool? IsActive { get; set; }

    public string? Reason { get; set; }

    public virtual User? User { get; set; }
}
