using FoodOrdering.Enums;

namespace FoodOrdering.Models;

public class Order
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int FoodItemId { get; set; }

    public FoodItem FoodItem { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public OrderState OrderState { get; set; }
}
