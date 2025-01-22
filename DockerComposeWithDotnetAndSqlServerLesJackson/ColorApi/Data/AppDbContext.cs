using ColorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Color> Colors { get; set; }
}
