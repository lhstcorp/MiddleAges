using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleAges.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Land> Lands { get; set; }

        public DbSet<Country> Countries { get; set; }
    }
}
