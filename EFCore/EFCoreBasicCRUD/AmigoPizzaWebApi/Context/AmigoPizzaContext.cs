using AmigoPizzaWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmigoPizzaWebApi.Context;

public class AmigoPizzaContext : DbContext
{
    public AmigoPizzaContext(DbContextOptions options) :base(options)
    {
    }
    
    
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     string connectionString = "Data Source = localhost,1433; Initial Catalog = AmigoPizza; Integrated Security = false; User Id = sa; Password = docker-147852369; TrustServerCertificate = true";
    //     optionsBuilder.UseSqlServer(connectionString);
    // }
}