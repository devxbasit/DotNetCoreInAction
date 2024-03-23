using AmigoPizzaWebApi.Context;
using AmigoPizzaWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpPost]
    public string AddProduct(Product product)
    {
        // http://localhost:5176/api/v1/Product/AddProduct
        // {
        //     "Name": "Speaker",
        //     "Price": 100
        // }
        _amigoPizzaContext.Products.Add(product);
        _amigoPizzaContext.SaveChanges();
        return "Product Added!";
    }

    [HttpPatch]
    public string UpdatePrice(int productId, decimal newPrice)
    {
        // http://localhost:5176/api/v1/Product/UpdatePrice?productId=2&newPrice=10
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

    [HttpDelete]
    public string DeleteProduct(int productId)
    {
        // http://localhost:5176/api/v1/Product/DeleteProduct?productId=1
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

    [HttpGet]
    public IEnumerable<Product> GetProductList(decimal price)
    {
        // http://localhost:5176/api/v1/Product/GetProductList
        return _amigoPizzaContext.Products;
        //return _amigoPizzaContext.Products.AsNoTracking();
    }
    
    [HttpGet]
    public Product GetProductById(int productId)
    {
        // http://localhost:5176/api/v1/Product/GetProductById?productId=10
        var product = _amigoPizzaContext
            .Products
            .FirstOrDefault(p => p.Id == productId);
        
        if (product is null)
        {
            throw new Exception("No product found");
        }

        return product;
    }
}