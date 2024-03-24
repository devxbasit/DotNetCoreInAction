using AutoMapper;
using Contracts;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILogManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repository.CompanyRepository.GetAllCompanies(trackChanges);

            // var companiesDto = companies
            //     .Select(c => new CompanyDto(c.Id, c.Name, c.Address))
            //     .ToList();

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}