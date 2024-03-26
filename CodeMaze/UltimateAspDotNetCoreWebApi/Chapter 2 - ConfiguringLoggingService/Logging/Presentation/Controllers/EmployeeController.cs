using Microsoft.AspNetCore.JsonPatch;
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
    public async Task<IActionResult> GetEmployeesForCompanies(Guid companyId)
    {
        var employees = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployees(Guid companyId, Guid employeeId)
    {
        var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
        return Ok(employee);
    }

    public async Task<IActionResult> CreateEmployee(Guid companyId,
        [FromBody] EmployeeForCreationRequestDto employeeForCreationRequestDto)
    {
        if (employeeForCreationRequestDto is null) return BadRequest("Employee object is null");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(nameof(employeeForCreationRequestDto.Name), "Added Error Message");
            return UnprocessableEntity(ModelState);
        }

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