namespace AmigoPizzaWebApi.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderPlacedDateTime { get; set; }
    public DateTime? OrderFulFilled { get; set; }
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = null!;

}