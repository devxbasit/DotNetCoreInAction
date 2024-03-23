using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkingWithExistingDatabase.Entities;

[Index("CustomerId", Name = "IX_Orders_CustomerId")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    public DateTime OrderPlacedDateTime { get; set; }

    public DateTime? OrderFulFilled { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
