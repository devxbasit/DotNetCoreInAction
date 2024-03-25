using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Services.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyResponseDto> GetAllCompanies(bool trackChanges);
    CompanyResponseDto GetCompany(Guid companyId, bool trackChanges);
    CompanyResponseDto CreateCompany(CompanyRequestDto companyRequestDto);
    IEnumerable<CompanyResponseDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    (IEnumerable<CompanyResponseDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyRequestDto> companyCollection);
    void DeleteCompany(Guid companyId, bool trackChanges);
}