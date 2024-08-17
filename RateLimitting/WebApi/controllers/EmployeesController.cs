using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.controllers;

[Route("api/v1/employee")]
public class EmployeesController : ControllerBase
{

    [HttpGet("fixed")]
    [EnableRateLimiting("FixedPolicy1")]
    public string Fixed()
    {
        return "Hello";
    }

    [HttpGet("sliding")]
    [EnableRateLimiting("SlidingPolicy1")]
    public string Sliding()
    {
        return "Hello";
    }

    [HttpGet("concurrent")]
    [EnableRateLimiting("ConcurrentPolicy1")]
    public string Concurrent()
    {
        return "Hello";
    }

    [HttpGet("token")]
    [EnableRateLimiting("TokenPolicy1")]
    public string Token()
    {
        return "Hello";
    }
}
