using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace MiddleAges.Data
{
    public class ApplicationDbContext : IdentityDbContext<Player>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Unit>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<Unit>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<Building>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<Building>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<Country>()
                .HasOne(q => q.Ruler)
                .WithMany()
                .HasForeignKey(q => q.RulerId);

            builder.Entity<Land>()
                .HasOne(q => q.Country)
                .WithMany()
                .HasForeignKey(q => q.CountryId);

            builder.Entity<Land>()
                .HasOne(q => q.Governor)
                .WithMany()
                .HasForeignKey(q => q.GovernorId);

            builder.Entity<BorderLand>()
                .HasKey(q => new { q.LandId, q.BorderLandId });

            builder.Entity<BorderLand>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<Player>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.CurrentLand);

            builder.Entity<Law>()
                .HasOne(q => q.Country)
                .WithMany()
                .HasForeignKey(q => q.CountryId);

            builder.Entity<Law>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<ChatMessage>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<Army>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<Army>()
                .HasOne(q => q.War)
                .WithMany()
                .HasForeignKey(q => q.WarId);

            builder.Entity<Army>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<PlayerStatistics>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<War>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.RebelId);

            builder.Entity<PlayerAttribute>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<Rating>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<PlayerInformation>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<PlayerLocalEvent>()
                .HasOne(q => q.Player)
                .WithMany()
                .HasForeignKey(q => q.PlayerId);

            builder.Entity<LandBuilding>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<LandDevelopmentShare>()
                .HasOne(q => q.Land)
                .WithMany()
                .HasForeignKey(q => q.LandId);

            builder.Entity<WarLog>()
                .HasOne(q => q.War)
                .WithMany()
                .HasForeignKey(q => q.WarId);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<BorderLand> BorderLands { get; set; }
        public DbSet<Law> Laws { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<War> Wars { get; set; }
        public DbSet<Army> Armies { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        public DbSet<PlayerAttribute> PlayerAttributes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PlayerInformation> PlayerInformations { get; set; }
        public DbSet<PlayerLocalEvent> PlayerLocalEvents { get; set; }
        public DbSet<LandBuilding> LandBuildings { get; set; }
        public DbSet<LandDevelopmentShare> LandDevelopmentShares { get; set; }
        public DbSet<WarLog> WarLogs { get; set; }
    }
}