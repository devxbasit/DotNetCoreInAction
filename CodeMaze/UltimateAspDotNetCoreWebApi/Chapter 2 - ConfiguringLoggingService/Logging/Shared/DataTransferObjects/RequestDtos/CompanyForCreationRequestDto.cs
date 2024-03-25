namespace Shared.DataTransferObjects.RequestDtos;

public record CompanyForCreationRequestDto(string Name, string Address, String Country, IEnumerable<EmployeeForCreationRequestDto> Employees);
