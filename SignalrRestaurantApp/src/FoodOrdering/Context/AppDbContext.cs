using FoodOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions options) :base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDb");
    }

    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<Order> Orders { get; set; }
}
