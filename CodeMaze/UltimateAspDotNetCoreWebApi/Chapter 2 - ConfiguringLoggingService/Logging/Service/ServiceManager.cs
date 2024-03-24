using AutoMapper;
using Contracts;
using Services.Contracts;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IRepositoryManager repository, ILogManager logger, IMapper mapper)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repository, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repository, logger, mapper));
    }
    
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}