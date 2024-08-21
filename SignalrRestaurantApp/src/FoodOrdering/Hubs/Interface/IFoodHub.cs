using FoodOrdering.Models;

namespace FoodOrdering.Hubs.Interface;

public interface IFoodHub
{
    Task PendingFoodUpdated(List<Order> orders);
}
