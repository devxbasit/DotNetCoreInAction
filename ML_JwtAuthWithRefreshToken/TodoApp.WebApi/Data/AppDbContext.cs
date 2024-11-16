using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Configuration;
using TodoApp.WebApi.Configuration.Entity;
using TodoApp.WebApi.Models;

namespace TodoApp.WebApi.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration<TodoItem>(new TodoItemConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
