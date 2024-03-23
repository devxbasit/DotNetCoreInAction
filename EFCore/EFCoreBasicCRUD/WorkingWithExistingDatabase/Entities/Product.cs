using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkingWithExistingDatabase.Entities;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
