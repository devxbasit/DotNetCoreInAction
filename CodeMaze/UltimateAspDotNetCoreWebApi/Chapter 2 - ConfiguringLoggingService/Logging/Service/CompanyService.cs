using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

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

    public IEnumerable<CompanyResponseDto> GetAllCompanies(bool trackChanges)
    {
        var companies = _repository.CompanyRepository.GetAllCompanies(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyResponseDto>>(companies);
        return companiesDto;
    }

    public CompanyResponseDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var companyDto = _mapper.Map<CompanyResponseDto>(company);
        return companyDto;
    }

    public CompanyResponseDto CreateCompany(CompanyRequestDto companyRequestDto)
    {
        var companyEntity = _mapper.Map<Company>(companyRequestDto);
        _repository.CompanyRepository.CreateCompany(companyEntity);
        _repository.Save();

        var companyResponseDto = _mapper.Map<CompanyResponseDto>(companyEntity);
        return companyResponseDto;
    }

    public IEnumerable<CompanyResponseDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null) throw new IdParametersBadRequestException();

        var companyEntities = _repository.CompanyRepository.GetByIds(ids, trackChanges);
        if (ids.Count() != companyEntities.Count()) throw new CollectionByIdsBadRequestException();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyResponseDto>>(companyEntities);
        return companiesToReturn;
    }

    public (IEnumerable<CompanyResponseDto> companies, string ids) CreateCompanyCollection(
        IEnumerable<CompanyRequestDto> companyRequestDto)
    {
        if (companyRequestDto is null) throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyRequestDto);

        foreach (var company in companyEntities)
        {
            _repository.CompanyRepository.CreateCompany(company);
        }

        _repository.Save();

        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyResponseDto>>(companyEntities);

        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companyCollectionToReturn, ids);
    }

    public void DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);
        _repository.CompanyRepository.DeleteCompany(company);
        _repository.Save();
    }
}