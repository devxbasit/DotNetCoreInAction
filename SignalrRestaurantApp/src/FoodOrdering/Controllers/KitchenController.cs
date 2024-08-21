using FoodOrdering.Context;
using FoodOrdering.Enums;
using FoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Controllers;
[Route("api/[controller]/[action]")]
public class KitchenController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public KitchenController(AppDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public List<Order> GetExistingOrders()
    {
        var orders = _dbContext.Orders.Include(x => x.FoodItem).Where(x => x.OrderState != OrderState.Completed);
        return orders.ToList();
    }

}
