using Microsoft.AspNetCore.Mvc;

namespace MVCApp.Controllers;

public class NotFoundController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
