using Entities.Models;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;
using Shared.RequestFeatures;

namespace Services.Contracts;

public interface IEmployeeService
{
    public Task<(IEnumerable<EmployeeResponseDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    public Task<EmployeeResponseDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);

    public Task<EmployeeResponseDto> CreateAsync(Guid companyId, EmployeeForCreationRequestDto employeeForCreationRequestDto,
        bool trackChanges);

    public Task DeleteAsync(Guid companyId, Guid employeeId, bool trackChanges);

    Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateRequestDto employeeForUpdateRequest,
        bool compTrackChanges, bool empTrackChanges);

    Task<(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(Guid companyId,
        Guid employeeId, bool compTrackChanges, bool empTrackChanges);

    Task SaveChangesForPatchAsync(EmployeeForUpdateRequestDto employeeToPatch, Employee employee);
}