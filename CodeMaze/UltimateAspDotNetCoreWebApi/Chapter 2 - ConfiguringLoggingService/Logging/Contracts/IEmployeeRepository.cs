using Entities.Models;

namespace Contracts;

public interface IEmployeeRepository
{
    public Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    public Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    public void Create(Guid companyId, Employee employee);
    public void Delete(Employee employee);
}