using AutoMapper;
using Contracts;
using Services.Contracts;
using Shared.DataTransferObjects.ResponseDtos;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IRepositoryManager repository, ILogManager logger, IMapper mapper, IDataShaper<EmployeeResponseDto> dataShaper)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repository, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repository, logger, mapper, dataShaper));
    }
    
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}