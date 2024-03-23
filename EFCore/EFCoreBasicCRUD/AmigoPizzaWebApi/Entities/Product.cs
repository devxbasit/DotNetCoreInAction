using System.ComponentModel.DataAnnotations.Schema;

namespace AmigoPizzaWebApi.Entities;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    
    [Column(TypeName = "decimal(9, 6)")]
    public decimal Price { get; set; } 

}