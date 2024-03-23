using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EmployeeController : ControllerBase
{

    public EmployeeController()
    {
    }

    public string GetMessage()
    {
        return "Hello World";
    }
    
}