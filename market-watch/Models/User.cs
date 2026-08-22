using System;
using System.Collections.Generic;

namespace market_watch.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? Salt { get; set; }

    public string? UserRole { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<AddCompaniesRequest> AddCompaniesRequests { get; set; } = new List<AddCompaniesRequest>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
