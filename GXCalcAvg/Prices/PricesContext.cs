using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PortlandCredentials;

namespace PortlandPublishedCalculator.Prices
{
    public partial class PricesContext : DbContext
    {
        public PricesContext()
        {
        }

        public PricesContext(DbContextOptions<PricesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PrimaForwardPrice> PrimaForwardPrices { get; set; } = null!;
        public virtual DbSet<PrimaSpotPrice> PrimaSpotPrices { get; set; } = null!;
        public virtual DbSet<YArgusomrBiofuelsNewer> YArgusomrBiofuelsNewers { get; set; } = null!;
        public virtual DbSet<YArgusomrThg> YArgusomrThgs { get; set; } = null!;
        public virtual DbSet<YFteur> YFteurs { get; set; } = null!;
        public virtual DbSet<YFtgbp> YFtgbps { get; set; } = null!;
        public virtual DbSet<YGimid> YGimids { get; set; } = null!;
        public virtual DbSet<YHvoBlendPercentage> YHvoBlendPercentages { get; set; } = null!;
        public virtual DbSet<YHvoProductionCost> YHvoProductionCosts { get; set; } = null!;
        public virtual DbSet<YPublishedWholesale> YPublishedWholesales { get; set; } = null!;
        public virtual DbSet<YSupplierPrice> YSupplierPrices { get; set; } = null!;
        public virtual DbSet<ZIcefutLsg> ZIcefutLsgs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var creds = Credentials.GetExternalDBCreds();
                optionsBuilder.UseNpgsql($"Server={creds.DatabaseExternalHost};Database=prices;User Id={creds.DatabaseUserName};Password={creds.DatabasePassword};Port={creds.DatabaseExternalPortNumber}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrimaForwardPrice>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.PublishedDate, e.Grade, e.Currency, e.Delivery })
                    .HasName("prima_forward_price_pkey");

                entity.ToTable("prima_forward_price");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Grade)
                    .HasColumnType("character varying")
                    .HasColumnName("grade");

                entity.Property(e => e.Currency)
                    .HasColumnType("character varying")
                    .HasColumnName("currency");

                entity.Property(e => e.Delivery)
                    .HasColumnType("character varying")
                    .HasColumnName("delivery");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<PrimaSpotPrice>(entity =>
            {
                entity.HasKey(e => new { e.PublishedDate, e.Grade, e.Currency })
                    .HasName("prima_spot_price_pkey");

                entity.ToTable("prima_spot_price");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Grade)
                    .HasColumnType("character varying")
                    .HasColumnName("grade");

                entity.Property(e => e.Currency)
                    .HasColumnType("character varying")
                    .HasColumnName("currency");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<YArgusomrBiofuelsNewer>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_argusomr_biofuels_newer_pkey");

                entity.ToTable("y_argusomr_biofuels_newer");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Ethanol).HasColumnName("ethanol");

                entity.Property(e => e.EthanolChange).HasColumnName("ethanol_change");

                entity.Property(e => e.Fame0).HasColumnName("fame_0");

                entity.Property(e => e.Fame0Change).HasColumnName("fame_0_change");

                entity.Property(e => e.Fame10).HasColumnName("fame_-10");

                entity.Property(e => e.Fame10Change).HasColumnName("fame_-10_change");

                entity.Property(e => e.HvoClassI).HasColumnName("hvo_class_i");

                entity.Property(e => e.HvoClassIi).HasColumnName("hvo_class_ii");

                entity.Property(e => e.HvoClassiChange).HasColumnName("hvo_classi_change");

                entity.Property(e => e.HvoClassiiChange).HasColumnName("hvo_classii_change");

                entity.Property(e => e.Rme).HasColumnName("rme");

                entity.Property(e => e.RmeChange).HasColumnName("rme_change");

                entity.Property(e => e.Ucome).HasColumnName("ucome");

                entity.Property(e => e.UcomeChange).HasColumnName("ucome_change");
            });

            modelBuilder.Entity<YArgusomrThg>(entity =>
            {
                entity.HasKey(e => new { e.PublishedDate, e.Grade })
                    .HasName("y_argusomr_thg_pkey");

                entity.ToTable("y_argusomr_thg");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Grade)
                    .HasColumnType("character varying")
                    .HasColumnName("grade");

                entity.Property(e => e.High).HasColumnName("high");

                entity.Property(e => e.Low).HasColumnName("low");
            });

            modelBuilder.Entity<YFteur>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("pkey_y_fteur");

                entity.ToTable("y_fteur");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Aed).HasColumnName("aed");

                entity.Property(e => e.All).HasColumnName("all");

                entity.Property(e => e.Amd).HasColumnName("amd");

                entity.Property(e => e.Ang).HasColumnName("ang");

                entity.Property(e => e.Aoa).HasColumnName("aoa");

                entity.Property(e => e.Ars).HasColumnName("ars");

                entity.Property(e => e.Aud).HasColumnName("aud");

                entity.Property(e => e.Awg).HasColumnName("awg");

                entity.Property(e => e.Azn).HasColumnName("azn");

                entity.Property(e => e.Bam).HasColumnName("bam");

                entity.Property(e => e.Bbd).HasColumnName("bbd");

                entity.Property(e => e.Bdt).HasColumnName("bdt");

                entity.Property(e => e.Bgn).HasColumnName("bgn");

                entity.Property(e => e.Bhd).HasColumnName("bhd");

                entity.Property(e => e.Bif).HasColumnName("bif");

                entity.Property(e => e.Bnd).HasColumnName("bnd");

                entity.Property(e => e.Bob).HasColumnName("bob");

                entity.Property(e => e.Brl).HasColumnName("brl");

                entity.Property(e => e.Bsd).HasColumnName("bsd");

                entity.Property(e => e.Bwp).HasColumnName("bwp");

                entity.Property(e => e.Byn).HasColumnName("byn");

                entity.Property(e => e.Bzd).HasColumnName("bzd");

                entity.Property(e => e.Cad).HasColumnName("cad");

                entity.Property(e => e.Cdf).HasColumnName("cdf");

                entity.Property(e => e.Chf).HasColumnName("chf");

                entity.Property(e => e.Clf).HasColumnName("clf");

                entity.Property(e => e.Cny).HasColumnName("cny");

                entity.Property(e => e.Cop).HasColumnName("cop");

                entity.Property(e => e.Crc).HasColumnName("crc");

                entity.Property(e => e.Csk).HasColumnName("csk");

                entity.Property(e => e.Cuc).HasColumnName("cuc");

                entity.Property(e => e.Cve).HasColumnName("cve");

                entity.Property(e => e.Djf).HasColumnName("djf");

                entity.Property(e => e.Dkk).HasColumnName("dkk");

                entity.Property(e => e.Dop).HasColumnName("dop");

                entity.Property(e => e.Dzd).HasColumnName("dzd");

                entity.Property(e => e.Egp).HasColumnName("egp");

                entity.Property(e => e.Ern).HasColumnName("ern");

                entity.Property(e => e.Etb).HasColumnName("etb");

                entity.Property(e => e.Eur).HasColumnName("eur");

                entity.Property(e => e.Fjd).HasColumnName("fjd");

                entity.Property(e => e.Fkp).HasColumnName("fkp");

                entity.Property(e => e.Gbp).HasColumnName("gbp");

                entity.Property(e => e.Gel).HasColumnName("gel");

                entity.Property(e => e.Ghs).HasColumnName("ghs");

                entity.Property(e => e.Gip).HasColumnName("gip");

                entity.Property(e => e.Gmd).HasColumnName("gmd");

                entity.Property(e => e.Gnf).HasColumnName("gnf");

                entity.Property(e => e.Gtq).HasColumnName("gtq");

                entity.Property(e => e.Gyd).HasColumnName("gyd");

                entity.Property(e => e.Hkd).HasColumnName("hkd");

                entity.Property(e => e.Hnl).HasColumnName("hnl");

                entity.Property(e => e.Hrk).HasColumnName("hrk");

                entity.Property(e => e.Huf).HasColumnName("huf");

                entity.Property(e => e.Idr).HasColumnName("idr");

                entity.Property(e => e.Ils).HasColumnName("ils");

                entity.Property(e => e.Inr).HasColumnName("inr");

                entity.Property(e => e.Iqd).HasColumnName("iqd");

                entity.Property(e => e.Isk).HasColumnName("isk");

                entity.Property(e => e.Jmd).HasColumnName("jmd");

                entity.Property(e => e.Jod).HasColumnName("jod");

                entity.Property(e => e.Jpy).HasColumnName("jpy");

                entity.Property(e => e.Kes).HasColumnName("kes");

                entity.Property(e => e.Kgs).HasColumnName("kgs");

                entity.Property(e => e.Khr).HasColumnName("khr");

                entity.Property(e => e.Kmf).HasColumnName("kmf");

                entity.Property(e => e.Krw).HasColumnName("krw");

                entity.Property(e => e.Kwd).HasColumnName("kwd");

                entity.Property(e => e.Kyd).HasColumnName("kyd");

                entity.Property(e => e.Kzt).HasColumnName("kzt");

                entity.Property(e => e.Lak).HasColumnName("lak");

                entity.Property(e => e.Lbp).HasColumnName("lbp");

                entity.Property(e => e.Lkr).HasColumnName("lkr");

                entity.Property(e => e.Lrd).HasColumnName("lrd");

                entity.Property(e => e.Lsl).HasColumnName("lsl");

                entity.Property(e => e.Lyd).HasColumnName("lyd");

                entity.Property(e => e.Mad).HasColumnName("mad");

                entity.Property(e => e.Mdl).HasColumnName("mdl");

                entity.Property(e => e.Mga).HasColumnName("mga");

                entity.Property(e => e.Mkd).HasColumnName("mkd");

                entity.Property(e => e.Mmk).HasColumnName("mmk");

                entity.Property(e => e.Mnt).HasColumnName("mnt");

                entity.Property(e => e.Mop).HasColumnName("mop");

                entity.Property(e => e.Mru).HasColumnName("mru");

                entity.Property(e => e.Mur).HasColumnName("mur");

                entity.Property(e => e.Mvr).HasColumnName("mvr");

                entity.Property(e => e.Mwk).HasColumnName("mwk");

                entity.Property(e => e.Mxn).HasColumnName("mxn");

                entity.Property(e => e.Myr).HasColumnName("myr");

                entity.Property(e => e.Mzn).HasColumnName("mzn");

                entity.Property(e => e.Nad).HasColumnName("nad");

                entity.Property(e => e.Ngn).HasColumnName("ngn");

                entity.Property(e => e.Nio).HasColumnName("nio");

                entity.Property(e => e.Nok).HasColumnName("nok");

                entity.Property(e => e.Npr).HasColumnName("npr");

                entity.Property(e => e.Nzd).HasColumnName("nzd");

                entity.Property(e => e.Omr).HasColumnName("omr");

                entity.Property(e => e.Pen).HasColumnName("pen");

                entity.Property(e => e.Pgk).HasColumnName("pgk");

                entity.Property(e => e.Php).HasColumnName("php");

                entity.Property(e => e.Pkr).HasColumnName("pkr");

                entity.Property(e => e.Pln).HasColumnName("pln");

                entity.Property(e => e.Pyg).HasColumnName("pyg");

                entity.Property(e => e.Qar).HasColumnName("qar");

                entity.Property(e => e.Ron).HasColumnName("ron");

                entity.Property(e => e.Rsd).HasColumnName("rsd");

                entity.Property(e => e.Rub).HasColumnName("rub");

                entity.Property(e => e.Rwf).HasColumnName("rwf");

                entity.Property(e => e.Sar).HasColumnName("sar");

                entity.Property(e => e.Sbd).HasColumnName("sbd");

                entity.Property(e => e.Scr).HasColumnName("scr");

                entity.Property(e => e.Sek).HasColumnName("sek");

                entity.Property(e => e.Sgd).HasColumnName("sgd");

                entity.Property(e => e.Shp).HasColumnName("shp");

                entity.Property(e => e.Sll).HasColumnName("sll");

                entity.Property(e => e.Sos).HasColumnName("sos");

                entity.Property(e => e.Srd).HasColumnName("srd");

                entity.Property(e => e.Ssp).HasColumnName("ssp");

                entity.Property(e => e.Stn).HasColumnName("stn");

                entity.Property(e => e.Szl).HasColumnName("szl");

                entity.Property(e => e.Thb).HasColumnName("thb");

                entity.Property(e => e.Tmt).HasColumnName("tmt");

                entity.Property(e => e.Tnd).HasColumnName("tnd");

                entity.Property(e => e.Top).HasColumnName("top");

                entity.Property(e => e.Try).HasColumnName("try");

                entity.Property(e => e.Ttd).HasColumnName("ttd");

                entity.Property(e => e.Twd).HasColumnName("twd");

                entity.Property(e => e.Tzs).HasColumnName("tzs");

                entity.Property(e => e.Uah).HasColumnName("uah");

                entity.Property(e => e.Ugx).HasColumnName("ugx");

                entity.Property(e => e.Usd).HasColumnName("usd");

                entity.Property(e => e.Uyi).HasColumnName("uyi");

                entity.Property(e => e.Uzs).HasColumnName("uzs");

                entity.Property(e => e.Vef).HasColumnName("vef");

                entity.Property(e => e.Vnd).HasColumnName("vnd");

                entity.Property(e => e.Vuv).HasColumnName("vuv");

                entity.Property(e => e.Wst).HasColumnName("wst");

                entity.Property(e => e.Xaf).HasColumnName("xaf");

                entity.Property(e => e.Xcd).HasColumnName("xcd");

                entity.Property(e => e.Xof).HasColumnName("xof");

                entity.Property(e => e.Xpf).HasColumnName("xpf");

                entity.Property(e => e.Yer).HasColumnName("yer");

                entity.Property(e => e.Zar).HasColumnName("zar");

                entity.Property(e => e.Zmw).HasColumnName("zmw");

                entity.Property(e => e.Zwl).HasColumnName("zwl");
            });

            modelBuilder.Entity<YFtgbp>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("pkey_y_ftgbp");

                entity.ToTable("y_ftgbp");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Aed).HasColumnName("aed");

                entity.Property(e => e.All).HasColumnName("all");

                entity.Property(e => e.Amd).HasColumnName("amd");

                entity.Property(e => e.Ang).HasColumnName("ang");

                entity.Property(e => e.Aoa).HasColumnName("aoa");

                entity.Property(e => e.Ars).HasColumnName("ars");

                entity.Property(e => e.Aud).HasColumnName("aud");

                entity.Property(e => e.Awg).HasColumnName("awg");

                entity.Property(e => e.Azn).HasColumnName("azn");

                entity.Property(e => e.Bam).HasColumnName("bam");

                entity.Property(e => e.Bbd).HasColumnName("bbd");

                entity.Property(e => e.Bdt).HasColumnName("bdt");

                entity.Property(e => e.Bgn).HasColumnName("bgn");

                entity.Property(e => e.Bhd).HasColumnName("bhd");

                entity.Property(e => e.Bif).HasColumnName("bif");

                entity.Property(e => e.Bnd).HasColumnName("bnd");

                entity.Property(e => e.Bob).HasColumnName("bob");

                entity.Property(e => e.Brl).HasColumnName("brl");

                entity.Property(e => e.Bsd).HasColumnName("bsd");

                entity.Property(e => e.Bwp).HasColumnName("bwp");

                entity.Property(e => e.Byn).HasColumnName("byn");

                entity.Property(e => e.Bzd).HasColumnName("bzd");

                entity.Property(e => e.Cad).HasColumnName("cad");

                entity.Property(e => e.Cdf).HasColumnName("cdf");

                entity.Property(e => e.Chf).HasColumnName("chf");

                entity.Property(e => e.Clf).HasColumnName("clf");

                entity.Property(e => e.Cny).HasColumnName("cny");

                entity.Property(e => e.Cop).HasColumnName("cop");

                entity.Property(e => e.Crc).HasColumnName("crc");

                entity.Property(e => e.Csk).HasColumnName("csk");

                entity.Property(e => e.Cuc).HasColumnName("cuc");

                entity.Property(e => e.Cve).HasColumnName("cve");

                entity.Property(e => e.Djf).HasColumnName("djf");

                entity.Property(e => e.Dkk).HasColumnName("dkk");

                entity.Property(e => e.Dop).HasColumnName("dop");

                entity.Property(e => e.Dzd).HasColumnName("dzd");

                entity.Property(e => e.Egp).HasColumnName("egp");

                entity.Property(e => e.Ern).HasColumnName("ern");

                entity.Property(e => e.Etb).HasColumnName("etb");

                entity.Property(e => e.Eur).HasColumnName("eur");

                entity.Property(e => e.Fjd).HasColumnName("fjd");

                entity.Property(e => e.Fkp).HasColumnName("fkp");

                entity.Property(e => e.Gbp).HasColumnName("gbp");

                entity.Property(e => e.Gel).HasColumnName("gel");

                entity.Property(e => e.Ghs).HasColumnName("ghs");

                entity.Property(e => e.Gip).HasColumnName("gip");

                entity.Property(e => e.Gmd).HasColumnName("gmd");

                entity.Property(e => e.Gnf).HasColumnName("gnf");

                entity.Property(e => e.Gtq).HasColumnName("gtq");

                entity.Property(e => e.Gyd).HasColumnName("gyd");

                entity.Property(e => e.Hkd).HasColumnName("hkd");

                entity.Property(e => e.Hnl).HasColumnName("hnl");

                entity.Property(e => e.Hrk).HasColumnName("hrk");

                entity.Property(e => e.Huf).HasColumnName("huf");

                entity.Property(e => e.Idr).HasColumnName("idr");

                entity.Property(e => e.Ils).HasColumnName("ils");

                entity.Property(e => e.Inr).HasColumnName("inr");

                entity.Property(e => e.Iqd).HasColumnName("iqd");

                entity.Property(e => e.Isk).HasColumnName("isk");

                entity.Property(e => e.Jmd).HasColumnName("jmd");

                entity.Property(e => e.Jod).HasColumnName("jod");

                entity.Property(e => e.Jpy).HasColumnName("jpy");

                entity.Property(e => e.Kes).HasColumnName("kes");

                entity.Property(e => e.Kgs).HasColumnName("kgs");

                entity.Property(e => e.Khr).HasColumnName("khr");

                entity.Property(e => e.Kmf).HasColumnName("kmf");

                entity.Property(e => e.Krw).HasColumnName("krw");

                entity.Property(e => e.Kwd).HasColumnName("kwd");

                entity.Property(e => e.Kyd).HasColumnName("kyd");

                entity.Property(e => e.Kzt).HasColumnName("kzt");

                entity.Property(e => e.Lak).HasColumnName("lak");

                entity.Property(e => e.Lbp).HasColumnName("lbp");

                entity.Property(e => e.Lkr).HasColumnName("lkr");

                entity.Property(e => e.Lrd).HasColumnName("lrd");

                entity.Property(e => e.Lsl).HasColumnName("lsl");

                entity.Property(e => e.Lyd).HasColumnName("lyd");

                entity.Property(e => e.Mad).HasColumnName("mad");

                entity.Property(e => e.Mdl).HasColumnName("mdl");

                entity.Property(e => e.Mga).HasColumnName("mga");

                entity.Property(e => e.Mkd).HasColumnName("mkd");

                entity.Property(e => e.Mmk).HasColumnName("mmk");

                entity.Property(e => e.Mnt).HasColumnName("mnt");

                entity.Property(e => e.Mop).HasColumnName("mop");

                entity.Property(e => e.Mru).HasColumnName("mru");

                entity.Property(e => e.Mur).HasColumnName("mur");

                entity.Property(e => e.Mvr).HasColumnName("mvr");

                entity.Property(e => e.Mwk).HasColumnName("mwk");

                entity.Property(e => e.Mxn).HasColumnName("mxn");

                entity.Property(e => e.Myr).HasColumnName("myr");

                entity.Property(e => e.Mzn).HasColumnName("mzn");

                entity.Property(e => e.Nad).HasColumnName("nad");

                entity.Property(e => e.Ngn).HasColumnName("ngn");

                entity.Property(e => e.Nio).HasColumnName("nio");

                entity.Property(e => e.Nok).HasColumnName("nok");

                entity.Property(e => e.Npr).HasColumnName("npr");

                entity.Property(e => e.Nzd).HasColumnName("nzd");

                entity.Property(e => e.Omr).HasColumnName("omr");

                entity.Property(e => e.Pen).HasColumnName("pen");

                entity.Property(e => e.Pgk).HasColumnName("pgk");

                entity.Property(e => e.Php).HasColumnName("php");

                entity.Property(e => e.Pkr).HasColumnName("pkr");

                entity.Property(e => e.Pln).HasColumnName("pln");

                entity.Property(e => e.Pyg).HasColumnName("pyg");

                entity.Property(e => e.Qar).HasColumnName("qar");

                entity.Property(e => e.Ron).HasColumnName("ron");

                entity.Property(e => e.Rsd).HasColumnName("rsd");

                entity.Property(e => e.Rub).HasColumnName("rub");

                entity.Property(e => e.Rwf).HasColumnName("rwf");

                entity.Property(e => e.Sar).HasColumnName("sar");

                entity.Property(e => e.Sbd).HasColumnName("sbd");

                entity.Property(e => e.Scr).HasColumnName("scr");

                entity.Property(e => e.Sek).HasColumnName("sek");

                entity.Property(e => e.Sgd).HasColumnName("sgd");

                entity.Property(e => e.Shp).HasColumnName("shp");

                entity.Property(e => e.Sll).HasColumnName("sll");

                entity.Property(e => e.Sos).HasColumnName("sos");

                entity.Property(e => e.Srd).HasColumnName("srd");

                entity.Property(e => e.Ssp).HasColumnName("ssp");

                entity.Property(e => e.Stn).HasColumnName("stn");

                entity.Property(e => e.Szl).HasColumnName("szl");

                entity.Property(e => e.Thb).HasColumnName("thb");

                entity.Property(e => e.Tmt).HasColumnName("tmt");

                entity.Property(e => e.Tnd).HasColumnName("tnd");

                entity.Property(e => e.Top).HasColumnName("top");

                entity.Property(e => e.Try).HasColumnName("try");

                entity.Property(e => e.Ttd).HasColumnName("ttd");

                entity.Property(e => e.Twd).HasColumnName("twd");

                entity.Property(e => e.Tzs).HasColumnName("tzs");

                entity.Property(e => e.Uah).HasColumnName("uah");

                entity.Property(e => e.Ugx).HasColumnName("ugx");

                entity.Property(e => e.Usd).HasColumnName("usd");

                entity.Property(e => e.Uyi).HasColumnName("uyi");

                entity.Property(e => e.Uzs).HasColumnName("uzs");

                entity.Property(e => e.Vef).HasColumnName("vef");

                entity.Property(e => e.Vnd).HasColumnName("vnd");

                entity.Property(e => e.Vuv).HasColumnName("vuv");

                entity.Property(e => e.Wst).HasColumnName("wst");

                entity.Property(e => e.Xaf).HasColumnName("xaf");

                entity.Property(e => e.Xcd).HasColumnName("xcd");

                entity.Property(e => e.Xof).HasColumnName("xof");

                entity.Property(e => e.Xpf).HasColumnName("xpf");

                entity.Property(e => e.Yer).HasColumnName("yer");

                entity.Property(e => e.Zar).HasColumnName("zar");

                entity.Property(e => e.Zmw).HasColumnName("zmw");

                entity.Property(e => e.Zwl).HasColumnName("zwl");
            });

            modelBuilder.Entity<YGimid>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_gimid_pkey");

                entity.ToTable("y_gimid");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Gx0000006).HasColumnName("gx0000006");

                entity.Property(e => e.Gx0000010).HasColumnName("gx0000010");

                entity.Property(e => e.Gx0000015).HasColumnName("gx0000015");

                entity.Property(e => e.Gx0000016).HasColumnName("gx0000016");

                entity.Property(e => e.Gx0000082).HasColumnName("gx0000082");

                entity.Property(e => e.Gx0000084).HasColumnName("gx0000084");

                entity.Property(e => e.Gx0000087).HasColumnName("gx0000087");

                entity.Property(e => e.Gx0000093).HasColumnName("gx0000093");

                entity.Property(e => e.Gx0000257).HasColumnName("gx0000257");

                entity.Property(e => e.Gx0000258).HasColumnName("gx0000258");

                entity.Property(e => e.Gx0000266).HasColumnName("gx0000266");

                entity.Property(e => e.Gx0000686).HasColumnName("gx0000686");

                entity.Property(e => e.Gx0001032).HasColumnName("gx0001032");
            });

            modelBuilder.Entity<YHvoBlendPercentage>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_hvo_blend_percentages_pk");

                entity.ToTable("y_hvo_blend_percentages");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.Diesel).HasColumnName("diesel");

                entity.Property(e => e.Fame).HasColumnName("fame");

                entity.Property(e => e.Hvo).HasColumnName("hvo");
            });

            modelBuilder.Entity<YHvoProductionCost>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_hvo_production_cost_pk");

                entity.ToTable("y_hvo_production_cost");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.ProductionCost).HasColumnName("production_cost");
            });

            modelBuilder.Entity<YPublishedWholesale>(entity =>
            {
                entity.HasKey(e => e.PublishedDate)
                    .HasName("y_published_wholesale_pkey");

                entity.ToTable("y_published_wholesale");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.DieselCifNwe).HasColumnName("diesel_cif_nwe");

                entity.Property(e => e.DieselFrb).HasColumnName("diesel_frb");

                entity.Property(e => e.EthanolEurCbm).HasColumnName("ethanol_eur_cbm");

                entity.Property(e => e.Fame0).HasColumnName("fame0");

                entity.Property(e => e.Fame10).HasColumnName("fame_10");

                entity.Property(e => e.Fueloil35Frb).HasColumnName("fueloil_35_frb");

                entity.Property(e => e.Gasoil01CifNwe).HasColumnName("gasoil_01_cif_nwe");

                entity.Property(e => e.HvoCifNwe).HasColumnName("hvo_cif_nwe");

                entity.Property(e => e.HvoFrb).HasColumnName("hvo_frb");

                entity.Property(e => e.JetCifNwe).HasColumnName("jet_cif_nwe");

                entity.Property(e => e.Mfo05Frb).HasColumnName("mfo_05_frb");

                entity.Property(e => e.NyhDiesel).HasColumnName("nyh_diesel");

                entity.Property(e => e.PropaneCifNwe).HasColumnName("propane_cif_nwe");

                entity.Property(e => e.UnleadedCifNwe).HasColumnName("unleaded_cif_nwe");
            });

            modelBuilder.Entity<YSupplierPrice>(entity =>
            {
                entity.ToTable("y_supplier_prices");

                entity.HasComment("This table houses the prices that we receive from suppliers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            });

            modelBuilder.Entity<ZIcefutLsg>(entity =>
            {
                entity.HasKey(e => new { e.PublishedDate, e.PricingDate })
                    .HasName("z_icefut_lsg_pkey");

                entity.ToTable("z_icefut_lsg");

                entity.Property(e => e.PublishedDate).HasColumnName("published_date");

                entity.Property(e => e.PricingDate).HasColumnName("pricing_date");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.RelativeMonth)
                    .HasMaxLength(6)
                    .HasColumnName("relative_month");
            });

            modelBuilder.HasSequence<int>("fg_cardstatuses_id_seq").StartsAt(101);

            modelBuilder.HasSequence("testuseraccess_id_seq")
                .StartsAt(14)
                .HasMax(2147483647);

            modelBuilder.HasSequence<int>("u_pricing_suppliers_id_seq");

            modelBuilder.HasSequence("u_region_id_seq");

            modelBuilder.HasSequence("u_spotcalc_id_seq").HasMax(2147483647);

            modelBuilder.HasSequence("u_terminal_id_seq");

            modelBuilder.HasSequence<int>("y_supplier_prices_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
