using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option) { }

        public DbSet<Categrory> Categrories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Categrory>()
                .HasData(
                    new Categrory
                    {
                        CategoryId = 1,
                        Name = "Action",
                        DisplayOrder = 1,
                    },

                    new Categrory
                    {
                        CategoryId = 2 ,
                        Name = "SciFi",
                        DisplayOrder = 1,
                    },

                    new Categrory
                    {
                        CategoryId = 3 ,
                        Name = "History",
                        DisplayOrder = 1,
                    }
                );
        }
    }
}
