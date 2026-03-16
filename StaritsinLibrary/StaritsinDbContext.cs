using Microsoft.EntityFrameworkCore;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary
{
    public class StaritsinDbContext : DbContext
    {
        public StaritsinDbContext(DbContextOptions<StaritsinDbContext> options) : base(options) { }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerType> PartnerTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PartnerSale> PartnerSales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            modelBuilder.Entity<PartnerType>(entity =>
            {
                entity.ToTable("partner_types");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeName)
                      .HasColumnName("type_name")
                      .HasMaxLength(100)
                      .IsRequired();
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partners");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PartnerTypeId).HasColumnName("partner_type_id");
                entity.Property(e => e.CompanyName).HasColumnName("company_name").HasMaxLength(255).IsRequired();
                entity.Property(e => e.DirectorName).HasColumnName("director_name").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(500).IsRequired();
                entity.Property(e => e.Rating).HasColumnName("rating").HasDefaultValue(0);

                entity.HasOne(e => e.PartnerType)
                      .WithMany(t => t.Partners)
                      .HasForeignKey(e => e.PartnerTypeId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductName).HasColumnName("product_name").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Unit).HasColumnName("unit").HasMaxLength(50).HasDefaultValue("шт");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("numeric(12,2)").HasDefaultValue(0m);
            });

            modelBuilder.Entity<PartnerSale>(entity =>
            {
                entity.ToTable("partner_sales");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PartnerId).HasColumnName("partner_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.SaleDate).HasColumnName("sale_date");
                entity.Property(e => e.BasePrice).HasColumnName("base_price").HasColumnType("numeric(12,2)").HasDefaultValue(0m);
                entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent").HasDefaultValue(0);
                entity.Property(e => e.UnitPrice).HasColumnName("unit_price").HasColumnType("numeric(12,2)").HasDefaultValue(0m);
                entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("numeric(14,2)").HasDefaultValue(0m);
                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.HasOne(e => e.Partner)
                      .WithMany(p => p.Sales)
                      .HasForeignKey(e => e.PartnerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Sales)
                      .HasForeignKey(e => e.ProductId);
            });
        }
    }
}
