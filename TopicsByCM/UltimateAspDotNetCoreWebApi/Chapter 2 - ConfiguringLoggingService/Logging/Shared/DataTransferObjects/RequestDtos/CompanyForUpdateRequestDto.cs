namespace Shared.DataTransferObjects.RequestDtos;

public record CompanyForUpdateRequestDto(string Name, string Address, string Country, IEnumerable<EmployeeForCreationRequestDto> Employees);
