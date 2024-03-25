using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Services.Contracts;

public interface IEmployeeService
{
    public IEnumerable<EmployeeResponseDto> GetEmployees(Guid companyId, bool trackChanges);
    public EmployeeResponseDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    public EmployeeResponseDto Create(Guid companyId, EmployeeRequestDto employeeRequestDto, bool trackChanges);
    public void Delete(Guid companyId, Guid employeeId, bool trackChanges);
}