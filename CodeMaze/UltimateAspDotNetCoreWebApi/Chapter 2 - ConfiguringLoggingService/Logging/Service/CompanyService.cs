using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;
using Shared.RequestFeatures;

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

    public async Task<(IEnumerable<CompanyResponseDto> companies, MetaData metaData)> GetAllCompaniesAsync(
        CompanyParameters companyParameters, bool trackChanges)
    {
        var companiesWithMetaData =
            await _repository.CompanyRepository.GetAllCompaniesAsync(companyParameters, trackChanges);

        var companiesDto = _mapper.Map<IEnumerable<CompanyResponseDto>>(companiesWithMetaData);
        return (companies: companiesDto, metaData: companiesWithMetaData.MetaData);
    }

    public async Task<CompanyResponseDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);
        var companyDto = _mapper.Map<CompanyResponseDto>(company);
        return companyDto;
    }

    public async Task<CompanyResponseDto> CreateCompanyAsync(CompanyForCreationRequestDto companyForCreationRequestDto)
    {
        var companyEntity = _mapper.Map<Company>(companyForCreationRequestDto);
        _repository.CompanyRepository.CreateCompany(companyEntity);
        await _repository.SaveAsync();

        var companyResponseDto = _mapper.Map<CompanyResponseDto>(companyEntity);
        return companyResponseDto;
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null) throw new IdParametersBadRequestException();

        var companyEntities = await _repository.CompanyRepository.GetByIdsAsync(ids, trackChanges);
        if (ids.Count() != companyEntities.Count()) throw new CollectionByIdsBadRequestException();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyResponseDto>>(companyEntities);
        return companiesToReturn;
    }

    public async Task<(IEnumerable<CompanyResponseDto> companies, string ids)> CreateCompanyCollectionAsync(
        IEnumerable<CompanyForCreationRequestDto> companyRequestDto)
    {
        if (companyRequestDto is null) throw new CompanyCollectionBadRequestException();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyRequestDto);

        foreach (var company in companyEntities)
        {
            _repository.CompanyRepository.CreateCompany(company);
        }

        await _repository.SaveAsync();

        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyResponseDto>>(companyEntities);

        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companyCollectionToReturn, ids);
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);
        _repository.CompanyRepository.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateAsync(Guid companyId, CompanyForUpdateRequestDto companyForUpdate, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfExists(companyId, trackChanges);

        _mapper.Map(companyForUpdate, company);
        await _repository.SaveAsync();
    }

    private async Task<Company> GetCompanyAndCheckIfExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
        return company ?? throw new CompanyNotFoundException(companyId);
    }
}