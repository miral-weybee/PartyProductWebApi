using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PartyProductWebApi.Models;

public partial class PartyProductWebApiContext : DbContext
{
    public PartyProductWebApiContext()
    {
    }

    public PartyProductWebApiContext(DbContextOptions<PartyProductWebApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssignParty> AssignParties { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Party> Parties { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductRate> ProductRates { get; set; }

    public virtual DbSet<InvoiceDTO> InvoiceDTOs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=PartyProductWebApi;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

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
            entity.HasKey(e => e.Id).HasName("PK_dbo.Invoices");

            entity.HasOne(d => d.Party).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_dbo.Invoices_dbo.Parties_PartyId");

            entity.HasOne(d => d.Product).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_dbo.Invoices_dbo.Products_ProductId");
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
