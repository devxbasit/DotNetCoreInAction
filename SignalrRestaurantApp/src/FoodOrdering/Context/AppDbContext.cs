using FoodOrdering.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<Order> Orders { get; set; }
}
