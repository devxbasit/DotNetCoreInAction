
namespace TodoApp.WebApi.Domain;

public class AuthResponse
{
    public string Token { get; set; }
    public bool Result { get; set; }
    public List<string> Errors { get; set; }
}
