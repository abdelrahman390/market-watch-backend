using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using market_watch.Models;

namespace market_watch.Data;


public partial class MarketWatchTrainingContext : DbContext
{
    public MarketWatchTrainingContext()
    {
    }

    public MarketWatchTrainingContext(DbContextOptions<MarketWatchTrainingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddCompaniesRequest> AddCompaniesRequests { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Broker> Brokers { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<DailyPrice> DailyPrices { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<Market> Markets { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-8DTTOUJ\\SQLEXPRESS;Database=MarketWatchTraining;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddCompaniesRequest>(entity =>
        {
            entity.HasKey(e => e.RequistId).HasName("PK_AddComaniesRequists");

            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.FaceValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Symbol).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.AddCompaniesRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AddComaniesRequists_Users");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E54864841A2B3AA");

            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionName).HasMaxLength(200);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.TableName).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AuditLogs_Users");
        });

        modelBuilder.Entity<Broker>(entity =>
        {
            entity.HasKey(e => e.BrokerId).HasName("PK__Brokers__5D1D9A50589AFB2F");

            entity.Property(e => e.BrokerCode).HasMaxLength(20);
            entity.Property(e => e.BrokerName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Phone).HasMaxLength(30);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CAC0345FD80");

            entity.HasIndex(e => e.MarketId, "IX_Company_Market");

            entity.HasIndex(e => e.SectorId, "IX_Company_Sector");

            entity.HasIndex(e => e.Symbol, "UQ__Companie__B7CC3F01C5EFF7C2").IsUnique();

            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.FaceValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Symbol).HasMaxLength(20);

            entity.HasOne(d => d.Market).WithMany(p => p.Companies)
                .HasForeignKey(d => d.MarketId)
                .HasConstraintName("FK_CompanyMarket");

            entity.HasOne(d => d.Sector).WithMany(p => p.Companies)
                .HasForeignKey(d => d.SectorId)
                .HasConstraintName("FK_CompanySector");
        });

        modelBuilder.Entity<DailyPrice>(entity =>
        {
            entity.HasKey(e => e.PriceId).HasName("PK__DailyPri__49575BAF69E6986D");

            entity.HasIndex(e => e.TradeDate, "IX_DailyPrice_Date");

            entity.Property(e => e.ClosePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HighPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LowPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpenPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ValueTraded).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Company).WithMany(p => p.DailyPrices)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_DailyPriceCompany");
        });

        modelBuilder.Entity<Investor>(entity =>
        {
            entity.HasKey(e => e.InvestorId).HasName("PK__Investor__41996EDEA6403533");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.InvestorType).HasMaxLength(50);
            entity.Property(e => e.Mobile).HasMaxLength(30);
            entity.Property(e => e.NationalId).HasMaxLength(20);
            entity.Property(e => e.Nationality).HasMaxLength(50);
        });

        modelBuilder.Entity<Market>(entity =>
        {
            entity.HasKey(e => e.MarketId).HasName("PK__Markets__74B186AF6CB3DBB5");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MarketName).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3F1687274");

            entity.Property(e => e.NewsTitle).HasMaxLength(300);
            entity.Property(e => e.PublishDate).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.News)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_NewsCompany");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF83097781");

            entity.HasIndex(e => e.OrderDate, "IX_Order_Date");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus).HasMaxLength(30);
            entity.Property(e => e.OrderType).HasMaxLength(10);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Broker).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BrokerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderBroker");

            entity.HasOne(d => d.Company).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderCompany");

            entity.HasOne(d => d.Investor).WithMany(p => p.Orders)
                .HasForeignKey(d => d.InvestorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderInvestor");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.SectorId).HasName("PK__Sectors__755E57E9E6AC98A0");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SectorName).HasMaxLength(100);
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(e => e.TradeId).HasName("PK__Trades__3028BB5B6A98009E");

            entity.HasIndex(e => e.CompanyId, "IX_Trade_Company");

            entity.HasIndex(e => e.TradeDate, "IX_Trade_Date");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Side)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TotalValue)
                .HasComputedColumnSql("([Quantity]*[Price])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.TradeDate).HasColumnType("datetime");

            entity.HasOne(d => d.Broker).WithMany(p => p.Trades)
                .HasForeignKey(d => d.BrokerId)
                .HasConstraintName("FK_TradeBroker");

            entity.HasOne(d => d.Company).WithMany(p => p.Trades)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_TradeCompany");

            entity.HasOne(d => d.Investor).WithMany(p => p.Trades)
                .HasForeignKey(d => d.InvestorId)
                .HasConstraintName("FK_TradeInvestor");

            entity.HasOne(d => d.Order).WithMany(p => p.Trades)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_TradeOrder");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(32);
            entity.Property(e => e.Salt)
                .HasMaxLength(16)
                .HasColumnName("salt");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserRole).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
