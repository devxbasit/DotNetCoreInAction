namespace Shared.DataTransferObjects.ResponseDtos;

public record EmployeeResponseDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public int Age { get; init; }
    public string? Position { get; init; }
}