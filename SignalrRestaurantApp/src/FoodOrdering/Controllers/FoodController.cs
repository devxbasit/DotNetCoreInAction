using FoodOrdering.Context;
using FoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Controllers;

[Route("api/food")]
public class FoodController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public FoodController(AppDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Route("available/all")]
    public async Task<List<FoodItem>> GetFoodItems()
    {
        var foodItems = await _dbContext.FoodItems.ToListAsync();
        return foodItems;
    }
}
