using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IEmployeeRepository
{
    public Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters,  bool trackChanges);
    public Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    public void Create(Guid companyId, Employee employee);
    public void Delete(Employee employee);
}