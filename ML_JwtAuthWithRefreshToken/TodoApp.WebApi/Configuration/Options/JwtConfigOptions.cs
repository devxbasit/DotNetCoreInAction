namespace TodoApp.WebApi.Configuration.Options;

public class JwtConfigOptions
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
}
