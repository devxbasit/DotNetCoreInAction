using System.ComponentModel.DataAnnotations;

namespace TodoApp.WebApi.Dto.Requests;

public class RefreshTokenRequestDto
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
