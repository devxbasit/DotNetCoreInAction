using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WorkingWithExistingDatabase.Entities;

namespace WorkingWithExistingDatabase.Context;

public partial class AmigoPizzaContext : DbContext
{
    public AmigoPizzaContext()
    {
    }

    public AmigoPizzaContext(DbContextOptions<AmigoPizzaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = localhost,1433; Initial Catalog = AmigoPizza; Integrated Security = false; User Id = sa; Password = strongPA55WORD!; TrustServerCertificate = true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
