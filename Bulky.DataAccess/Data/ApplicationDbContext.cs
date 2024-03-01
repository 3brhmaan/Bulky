using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasData(
                new Category { Name = "Action" , DisplayOrder = 1, Id = 1 },
                new Category { Name = "Scifi"  , DisplayOrder = 2, Id = 2 },
                new Category { Name = "History", DisplayOrder = 3, Id = 3 }
            );
    }

    public  virtual DbSet<Category> Categories { get; set; }
}