using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Services.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyResponseDto> GetAllCompanies(bool trackChanges);
    CompanyResponseDto GetCompany(Guid companyId, bool trackChanges);
    CompanyResponseDto CreateCompany(CompanyForCreationRequestDto companyForCreationRequestDto);
    IEnumerable<CompanyResponseDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    (IEnumerable<CompanyResponseDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationRequestDto> companyCollection);
    void DeleteCompany(Guid companyId, bool trackChanges);
    void Update(Guid companyId, CompanyForUpdateRequestDto company, bool trackChange);
}