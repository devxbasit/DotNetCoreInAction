namespace Shared.DataTransferObjects.ResponseDtos;

public record CompanyResponseDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? FullAddress { get; init; }
}