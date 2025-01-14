using Microsoft.AspNetCore.Mvc;
using ProductWebApi.Models;

namespace ProductWebApi.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController() : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        await Task.CompletedTask;
        await Task.Delay(TimeSpan.FromSeconds(3));
        return Ok(new Product() { ProductId = 1, ProductName = "Product 1" });
    }
}
