using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;
using Shared.RequestFeatures;

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
    public async Task<IActionResult> GetEmployeesForCompanies(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.employees);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
        return Ok(employee);
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployee(Guid companyId,
        [FromBody] EmployeeForCreationRequestDto employeeForCreationRequestDto)
    {
        var employeeToReturn =
            await _serviceManager.EmployeeService.CreateAsync(companyId, employeeForCreationRequestDto, trackChanges: false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id },
            employeeToReturn);
    }

    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId)
    {
        await _serviceManager.EmployeeService.DeleteAsync(companyId, employeeId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{employeeId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Update(Guid companyId, Guid employeeId,
        [FromBody] EmployeeForUpdateRequestDto employee)
    {
        if (employee is null) return BadRequest("Employee object is null");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(nameof(employee.Name), "Added Error Message");
            return UnprocessableEntity(ModelState);
        }

        await _serviceManager.EmployeeService.UpdateEmployeeAsync(companyId, employeeId, employee,
            compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }

    [HttpPatch("{employeeId:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId,
        [FromBody] JsonPatchDocument<EmployeeForUpdateRequestDto> patchDoc)
    {
        if (patchDoc is null) return BadRequest("patchDoc object sent from client is null.");

        var result = await _serviceManager.EmployeeService
            .GetEmployeeForPatchAsync(companyId, employeeId, compTrackChanges: false, empTrackChanges: true);

        // if we patch to remove Age, it will
        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        // if we patch to remove Age, re-trigger validation
        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await _serviceManager.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employee);
        return NoContent();
    }
}