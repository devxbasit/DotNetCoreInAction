using Contracts;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;

    public CompanyService(IRepositoryManager repository, ILogManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repository.CompanyRepository.GetAllCompanies(trackChanges);

            var companiesDto = companies
                .Select(c => new CompanyDto(c.Id, c.Name, c.Address))
                .ToList();

            return companiesDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}