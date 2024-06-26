using System.Dynamic;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;
using Shared.RequestFeatures;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeResponseDto> _dataShaper;

    public EmployeeService(IRepositoryManager repository, ILogManager logger, IMapper mapper, IDataShaper<EmployeeResponseDto> dataShaper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<(IEnumerable<ShapedEntity> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {

        if (!employeeParameters.ValidAgeRange) throw new MaxAgeRangeBadRequestException();
        
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeesWithMetaData = await _repository.EmployeeRepository.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employeesWithMetaData);

        var shapedData = _dataShaper.ShapeData(employeesDto, employeeParameters.Fields);
        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeResponseDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, employeeId, trackChanges);

        var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);
        return employeeDto;
    }

    public async Task<EmployeeResponseDto> CreateAsync(Guid companyId,
        EmployeeForCreationRequestDto employeeForCreationRequestDto,
        bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreationRequestDto);
        _repository.EmployeeRepository.Create(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeResponseDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, employeeId, trackChanges);

        _repository.EmployeeRepository.Delete(employee);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeAsync(Guid companyId, Guid employeeId,
        EmployeeForUpdateRequestDto employeeForUpdateRequest,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, employeeId, empTrackChanges);

        _mapper.Map(employeeForUpdateRequest, employee);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(
        Guid companyId,
        Guid employeeId, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employee = await GetEmployeeForCompanyAndCheckIfExists(companyId, employeeId, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateRequestDto>(employee);
        return (employeeToPatch, employee);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateRequestDto employeeToPatch, Employee employee)
    {
        _mapper.Map(employeeToPatch, employee);
        await _repository.SaveAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfExists(Guid companyId, Guid employeeId,
        bool trackChanges)
    {
        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(companyId, employeeId, trackChanges);
        return employee ?? throw new EmployeeNotFoundException(employeeId);
    }
}