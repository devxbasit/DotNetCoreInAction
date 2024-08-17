using Microsoft.AspNetCore.Mvc;

namespace MVCApp.Controllers;

public class HrOnlyController : Controller
{
    private readonly ILogger<HrOnlyController> _logger;

    public HrOnlyController(ILogger<HrOnlyController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

}
