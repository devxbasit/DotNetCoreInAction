using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MVCApp.Controllers;

public class SignOut : Controller
{

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            HttpContext.SignOutAsync();
        }

        return RedirectToRoute('/');
    }
}
