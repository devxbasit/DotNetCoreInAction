using Shared.DataTransferObjects;

namespace Services.Contracts;

public interface IEmployeeService
{
    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
}