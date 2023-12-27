using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PartyProductWebApi.Models;

public partial class EvaluationTaskDbContext : DbContext
{
    public EvaluationTaskDbContext()
    {
    }

    public EvaluationTaskDbContext(DbContextOptions<EvaluationTaskDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssignParty> AssignParties { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoiceproduct> Invoiceproducts { get; set; }

    public virtual DbSet<Party> Parties { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductRate> ProductRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=EvaluationTaskDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignParty>(entity =>
        {
            entity.HasKey(e => e.AssignPartyId).HasName("PK_dbo.AssignParties");

            entity.HasOne(d => d.Party).WithMany(p => p.AssignParties)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_dbo.AssignParties_dbo.Parties_Party_PartyId");

            entity.HasOne(d => d.Product).WithMany(p => p.AssignParties)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_dbo.AssignParties_dbo.Products_Product_ProductId");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK_dbo.Invoices");

            entity.Property(e => e.InvoiceId).HasColumnName("invoiceId");
        });

        modelBuilder.Entity<Invoiceproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__invoicep__3213E83F72BCFF52");

            entity.ToTable("invoiceproducts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rate).HasColumnName("rate");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Invoiceproducts)
                .HasForeignKey(d => d.Invoiceid)
                .HasConstraintName("FK__invoicepr__invoi__5BE2A6F2");

            entity.HasOne(d => d.Product).WithMany(p => p.Invoiceproducts)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("FK__invoicepr__produ__46E78A0C");
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.HasKey(e => e.PartyId).HasName("PK_dbo.Parties");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_dbo.Products");
        });

        modelBuilder.Entity<ProductRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ProductRates");

            entity.Property(e => e.DateOfRate).HasColumnType("datetime");
            entity.Property(e => e.ProductNameProductId).HasColumnName("ProductName_ProductId");

            entity.HasOne(d => d.ProductNameProduct).WithMany(p => p.ProductRates)
                .HasForeignKey(d => d.ProductNameProductId)
                .HasConstraintName("FK_dbo.ProductRates_dbo.Products_ProductName_ProductId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
