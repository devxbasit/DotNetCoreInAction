using AmigoPizzaWebApi.Context;
using AmigoPizzaWebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AmigoPizzaWebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly AmigoPizzaContext _amigoPizzaContext;
    
    public ProductController(AmigoPizzaContext amigoPizzaContext)
    {
        _amigoPizzaContext = amigoPizzaContext;
    }

    // http://localhost:5176/api/v1/Product/AddProduct
    [HttpGet]
    public void AddProduct()
    {
        var product1 = new Product()
        {
            Name = "Test Product 1",
            Price = 10
        };

        var product2 = new Product()
        {
            Name = "Test Product 2",
            Price = 20
        };

        _amigoPizzaContext.Products.Add(product1);
        _amigoPizzaContext.Add(product2);
        _amigoPizzaContext.SaveChangesAsync();
    }

    // http://localhost:5176/api/v1/Product/GetProductsByPrice?price=5
    public IEnumerable<Product> GetProductsByPrice(decimal price)
    {
        var products = from product in _amigoPizzaContext.Products
            where product.Price > price
            orderby product.Price descending 
            select product;
        
        return products;
    }
}