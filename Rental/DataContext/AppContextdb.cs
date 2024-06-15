using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Xml;
using Rental.Models;

namespace Rental
{
    public class AppContextdb : DbContext
    {
        public AppContextdb(DbContextOptions<AppContextdb> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Tenant> Tenant { get; set; } = null!;
        public DbSet<RealEstate> RealEstate { get; set; } = null!;
        public DbSet<Favorite> Favorite { get; set; }

    }
}
