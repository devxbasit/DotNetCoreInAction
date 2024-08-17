using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers;

public class SignInController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> VerifyUserCredentials(SignInDto signInDto)
    {
        if (!ModelState.IsValid) return View("Index");

        if (User.Identity is not null && !User.Identity.IsAuthenticated)
        {
            List<Claim> claims = GetUserClaimsList(signInDto);
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            var authenticationProperties = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                IsPersistent = signInDto.RememberMe
            };

            await HttpContext.SignInAsync("MyCookieAuth", principal, authenticationProperties);
            return Redirect($"/Admin/Index/");
        }
        else
        {
            await Task.CompletedTask;
            return Redirect($"NotFound/Index");
        }
    }

    private List<Claim> GetUserClaimsList(SignInDto signInDto)
    {
        if (signInDto.Username == "admin")
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Admin"),
                new Claim(ClaimTypes.Email, "Admin@gmail.com"),
                new Claim("Department", "Admin"),
                new Claim("EmploymentDate", "2024-01-25"),
                new Claim("Admin", "true")
            };
        } else if (signInDto.Username == "hr")
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "HR"),
                new Claim(ClaimTypes.Email, "HR@gmail.com"),
                new Claim("Department", "HR"),
                new Claim("EmploymentDate", "2024-01-25"),
                new Claim("Hr", "true")
            };
        }
        else
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Valid User"),
                new Claim(ClaimTypes.Email, "Valid@gmail.com"),
                new Claim("Department", "Valid"),
                new Claim("EmploymentDate", "2024-01-25"),
            };
        }
    }
}
