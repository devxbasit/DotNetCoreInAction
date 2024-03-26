using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.RequestDtos;

public abstract record EmployeeForManipulationRequestDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Employee age is a required field.")]
    [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
    public int Age { get; init; }

    [Required(ErrorMessage = "Employee Position is a required field.")]
    public string? Position { get; init; }
}
