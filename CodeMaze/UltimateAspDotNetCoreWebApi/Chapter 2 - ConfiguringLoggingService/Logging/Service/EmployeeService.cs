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

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = await _repository.EmployeeRepository.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        return employeesDto;
    }

    public async Task<EmployeeResponseDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, trackChanges);

        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeResponseDto> CreateAsync(Guid companyId,
        EmployeeForCreationRequestDto employeeForCreationRequestDto,
        bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreationRequestDto);
        _repository.EmployeeRepository.Create(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeResponseDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, trackChanges);

        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        _repository.EmployeeRepository.Delete(employee);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid employeeId,
        EmployeeForUpdateRequestDto employeeForUpdateRequest,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, compTrackChanges);

        if (company is null) throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, empTrackChanges);

        if (employeeEntity is null) throw new EmployeeNotFoundException(employeeId);

        _mapper.Map(employeeForUpdateRequest, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(
        Guid companyId,
        Guid employeeId, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);

        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, empTrackChanges);
        if (employee is null) throw new EmployeeNotFoundException(employeeId);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateRequestDto>(employee);
        return (employeeToPatch, employee);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)
    {
        _mapper.Map(employeeToPatch, employee);
        await _repository.SaveAsync();
    }
}