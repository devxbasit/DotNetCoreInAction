using Microsoft.AspNetCore.Mvc;

namespace MVCApp.Controllers;

public class ManagerController : Controller
{
    private readonly ILogger<ManagerController> _logger;

    public ManagerController(ILogger<ManagerController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

}
