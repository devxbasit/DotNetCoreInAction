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
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _serviceManager.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationRequestDto companyForCreationRequestDto)
    {
        if (companyForCreationRequestDto is null)
            return BadRequest("CompanyForCreationRequestDto object is null");
        var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(companyForCreationRequestDto);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids)
    {
        var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost("Collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationRequestDto> companyCollection)
    {
        var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }


    [HttpDelete("{companyId:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] CompanyForUpdateRequestDto company)
    {
        if (company is null) return BadRequest("Company object is null");

        await _serviceManager.CompanyService.UpdateAsync(companyId, company, trackChange: true);
        return NoContent();
    }
}