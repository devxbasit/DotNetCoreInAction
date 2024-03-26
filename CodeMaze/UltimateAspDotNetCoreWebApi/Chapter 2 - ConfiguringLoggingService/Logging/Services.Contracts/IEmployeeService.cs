using Entities.Models;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Services.Contracts;

public interface IEmployeeService
{
    public IEnumerable<EmployeeResponseDto> GetEmployees(Guid companyId, bool trackChanges);
    public EmployeeResponseDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    public EmployeeResponseDto Create(Guid companyId, EmployeeForCreationRequestDto employeeForCreationRequestDto, bool trackChanges);
    public void Delete(Guid companyId, Guid employeeId, bool trackChanges);
    void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateRequestDto employeeForUpdateRequest, bool compTrackChanges, bool empTrackChanges);
    (EmployeeForUpdateRequestDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid employeeId, bool compTrackChanges, bool empTrackChanges);
    void SaveChangesForPatch(EmployeeForUpdateRequestDto employeeToPatch, Employee employee);
}