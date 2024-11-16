using System.ComponentModel.DataAnnotations;
using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Models.Dto.Requests;

public class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
