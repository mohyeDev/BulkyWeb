using Microsoft.EntityFrameworkCore;
using Builky.Models.Models;

namespace Builky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option) { }

        public DbSet<Category> Categrories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasData(
                    new Category
                    {
                        CategoryId = 1,
                        Name = "Action",
                        DisplayOrder = 1,
                    },

                    new Category
                    {
                        CategoryId = 2 ,
                        Name = "SciFi",
                        DisplayOrder = 1,
                    },

                    new Category
                    {
                        CategoryId = 3 ,
                        Name = "History",
                        DisplayOrder = 1,
                    }
                );
        }
    }
}
