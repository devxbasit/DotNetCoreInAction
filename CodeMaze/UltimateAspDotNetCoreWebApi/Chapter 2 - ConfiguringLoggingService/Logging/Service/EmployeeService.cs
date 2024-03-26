using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILogManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<EmployeeResponseDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = _repository.EmployeeRepository.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        return employeesDto;
    }

    public EmployeeResponseDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = _repository.EmployeeRepository.GetEmployee(companyId, employeeId, trackChanges);

        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);
        return employeeDto;
    }

    public EmployeeResponseDto Create(Guid companyId, EmployeeForCreationRequestDto employeeForCreationRequestDto,
        bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreationRequestDto);
        _repository.EmployeeRepository.Create(companyId, employeeEntity);
        _repository.Save();

        var employeeToReturn = _mapper.Map<EmployeeResponseDto>(employeeEntity);
        return employeeToReturn;
    }

    public void Delete(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = _repository.EmployeeRepository.GetEmployee(companyId, employeeId, trackChanges);

        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        _repository.EmployeeRepository.Delete(employee);
        _repository.Save();
    }

    public void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateRequestDto employeeForUpdateRequest,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = _repository.EmployeeRepository.GetEmployee(companyId, employeeId, empTrackChanges);

        if (employeeEntity is null) throw new EmployeeNotFoundException(employeeId);

        _mapper.Map(employeeForUpdateRequest, employeeEntity);
        _repository.Save();
    }

    public (EmployeeForUpdateRequestDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId,
        Guid employeeId, bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.CompanyRepository.GetCompany(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = _repository.EmployeeRepository.GetEmployee(companyId, employeeId, empTrackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateRequestDto>(employee);
        return (employeeToPatch, employee);
    }

    public void SaveChangesForPatch(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)
    {
        _mapper.Map(employeeToPatch, employee);
        _repository.Save();
    }
}