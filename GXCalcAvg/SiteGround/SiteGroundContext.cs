using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PortlandCredentials;

namespace PortlandPublishedCalculator.SiteGround
{
    public partial class SiteGroundContext : DbContext
    {
        public SiteGroundContext()
        {
        }

        public SiteGroundContext(DbContextOptions<SiteGroundContext> options)
            : base(options)
        {
        }

        public virtual DbSet<YPublishedWholesale> YPublishedWholesales { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var creds = Credentials.GetSitegroundDBCreds();
                optionsBuilder.UseNpgsql($"Server={creds.SiteGroundDbHost};Database={creds.SiteGroundDbName};User Id={creds.SiteGroundDbUserName};Password={creds.SiteGroundDbPassword};Port={creds.SiteGroundDbPortNumber}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YPublishedWholesale>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_published_wholesale_pkey");

                entity.ToTable("y_published_wholesale");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.DieselCifNwe).HasColumnName("diesel_cif_nwe");

                entity.Property(e => e.EthanolEurCbm).HasColumnName("ethanol_eur_cbm");

                entity.Property(e => e.Fame10).HasColumnName("fame-10");

                entity.Property(e => e.HvoCifNwe).HasColumnName("hvo_cif_nwe");

                entity.Property(e => e.HvoFrb).HasColumnName("hvo_frb");

                entity.Property(e => e.JetCifNwe).HasColumnName("jet_cif_nwe");

                entity.Property(e => e.PropaneCifNwe).HasColumnName("propane_cif_nwe");

                entity.Property(e => e.UnleadedCifNwe).HasColumnName("unleaded_cif_nwe");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
