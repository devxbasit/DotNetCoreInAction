using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CompaniesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public IActionResult GetCompanies()
    {
        // removed try catch in favor of global exception handler middleware
        // try
        // {
            throw new Exception("Exception");
        var companies = _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);

        // }
        // catch
        // {
        //     return StatusCode(500, "Internal server error");
        // }
    }
}