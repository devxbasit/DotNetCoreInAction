using Microsoft.EntityFrameworkCore;
using TicketWebApi.Models;

namespace TicketWebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasMany(x => x.Tickets)
                .WithOne()
                .HasForeignKey(x => x.EventId)
                .IsRequired();

            var demoEvent = new Event()
            {
                Id = 1,
                Name = "Teachers Day",
                Location = "India",
            };

            entity.HasData(demoEvent);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            var tickets = Enumerable
                .Range(1, 5000)
                .Select(x => new Ticket()
                {
                    EventId = 1,
                    Price = 100
                });

            entity.HasData(tickets);
        });


    }
}
