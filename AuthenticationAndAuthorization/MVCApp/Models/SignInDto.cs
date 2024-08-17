namespace MVCApp.Models;

public class SignInDto
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public bool RememberMe { get; set; }
}
