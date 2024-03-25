using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;

namespace Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public EmployeeController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public IActionResult GetEmployeesForCompanies(Guid companyId)
    {
        var employees = _serviceManager.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = _serviceManager.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false);
        return Ok(employee);
    }

    public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeRequestDto employeeRequestDto)
    {
        if (employeeRequestDto is null) return BadRequest("Employee object is null");
        var employeeToReturn =
            _serviceManager.EmployeeService.Create(companyId, employeeRequestDto, trackChanges: false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id },
            employeeToReturn);
    }
}