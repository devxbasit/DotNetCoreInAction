namespace Shared.DataTransferObjects.RequestDtos;

public record CompanyRequestDto(string Name, string Address, String Country, IEnumerable<EmployeeRequestDto> Employees);
