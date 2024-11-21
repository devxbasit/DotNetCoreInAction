using System.ComponentModel.DataAnnotations;

namespace TodoApp.WebApi.Dto.Requests;

public class UserLoginRequestDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
