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
    // {
    //     "Name": "Speaker",
    //     "Price": 100
    // }
    [HttpPost]
    public string AddProduct(Product product)
    {
        _amigoPizzaContext.Products.Add(product);
        _amigoPizzaContext.SaveChanges();
        return "Product Added!";
    }

    // http://localhost:5176/api/v1/Product/GetProductList
    [HttpGet]
    public IEnumerable<Product> GetProductList(decimal price)
    {
        return _amigoPizzaContext.Products;
    }

    // http://localhost:5176/api/v1/Product/UpdatePrice?productId=2&newPrice=10
    [HttpPatch]
    public string UpdatePrice(int productId, decimal newPrice)
    {
        var product = _amigoPizzaContext
            .Products
            .FirstOrDefault(p => p.Id == productId);

        if (product is null)
        {
            return "product not found";
        }

        product.Price = newPrice;
        _amigoPizzaContext.SaveChanges();
        return "price updated";
    }

    // http://localhost:5176/api/v1/Product/DeleteProduct?productId=1
    [HttpDelete]
    public string DeleteProduct(int productId)
    {
        var product = _amigoPizzaContext
            .Products
            .FirstOrDefault(p => p.Id == productId);

        if (product is null)
        {
            return $"No product found with id {productId}";
        }

        _amigoPizzaContext.Products.Remove(product);
        _amigoPizzaContext.SaveChanges();

        return "Product Deleted";
    }
}