using Microsoft.AspNetCore.Mvc;
using Presentation.ModelBinders;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;

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
        var companies = _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _serviceManager.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyRequestDto companyRequestDto)
    {
        if (companyRequestDto is null)
            return BadRequest("CompanyRequestDto object is null");
        var createdCompany = _serviceManager.CompanyService.CreateCompany(companyRequestDto);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids)
    {
        var companies = _serviceManager.CompanyService.GetByIds(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost("Collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyRequestDto> companyCollection)
    {
        var result = _serviceManager.CompanyService.CreateCompanyCollection(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }


    [HttpDelete("{companyId:guid}")]
    public IActionResult DeleteCompany(Guid companyId)
    {
        _serviceManager.CompanyService.DeleteCompany(companyId, trackChanges: false);
        return NoContent();
    }
}