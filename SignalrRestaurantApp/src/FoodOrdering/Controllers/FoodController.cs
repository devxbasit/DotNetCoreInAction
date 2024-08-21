using FoodOrdering.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Controllers;

[Route("api/[controller]/[action]")]
public class FoodController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public FoodController(AppDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetFoodItems()
    {
        var foodItems = await _dbContext.FoodItems.ToListAsync();
        return Ok(foodItems);
    }
}
