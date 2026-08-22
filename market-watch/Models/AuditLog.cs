using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class AuditLog
{
    public long LogId { get; set; }

    public int? UserId { get; set; }

    public string? ActionName { get; set; }

    public string? TableName { get; set; }

    public DateTime? ActionDate { get; set; }

    public string? Ipaddress { get; set; }

    public virtual User? User { get; set; }
}
