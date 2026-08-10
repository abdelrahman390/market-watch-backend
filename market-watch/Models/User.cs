using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? UserRole { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
