using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Services.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync(bool trackChanges);
    Task<CompanyResponseDto> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyResponseDto> CreateCompanyAsync(CompanyForCreationRequestDto companyForCreationRequestDto);
    Task<IEnumerable<CompanyResponseDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<(IEnumerable<CompanyResponseDto> companies, string ids)> CreateCompanyCollectionAsync(
        IEnumerable<CompanyForCreationRequestDto> companyCollection);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
    Task UpdateAsync(Guid companyId, CompanyForUpdateRequestDto company, bool trackChange);
}