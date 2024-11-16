using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.WebApi.Configuration.Options;
using TodoApp.WebApi.Models.Dto.Requests;
using TodoApp.WebApi.Models.Dto.Responses;

namespace TodoApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthManagementController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfigOptions _jwtConfigOptions;

    public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfigOptions> jwtConfigOptions)
    {
        _userManager = userManager;
        _jwtConfigOptions = jwtConfigOptions.CurrentValue;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequestDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new UserRegistrationResponseDto()
            {
                Result = false,
                Errors = new List<string>() { "Invalid Payload" }
            });
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser is not null)
        {
            return BadRequest(new UserRegistrationResponseDto()
            {
                Result = false,
                Errors = new List<string>() { "Email already exists" }
            });
        }

        var newIdentityUser = new IdentityUser()
        {
            Email = user.Email,
            UserName = user.Email
        };

        var identityResult = await _userManager.CreateAsync(newIdentityUser, user.Password);

        if (!identityResult.Succeeded)
        {
            return new JsonResult(new UserRegistrationResponseDto()
            {
                Result = false,
                Errors = identityResult.Errors.Select(x => x.Description).ToList()
            })
            {
                StatusCode = 500
            };
        }

        // everything is okay
        var token = generateJwtToken(newIdentityUser);

        return Ok(new UserRegistrationResponseDto()
        {
            Result = true,
            Token = token,
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new UserLoginResponseDto()
            {
                Result = false,
                Errors = new List<string>() { "Invalid Payload" }
            });
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if (existingUser is null)
        {
            return BadRequest(new UserLoginResponseDto()
            {
                Result = false,
                Errors = new List<string>() { "Invalid Authentication Request" } // We don't want to give too much information on why the request has failed for security reasons
            });
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
        if (!isPasswordCorrect)
        {
            return BadRequest(new UserLoginResponseDto()
            {
                Result = false,
                Errors = new List<string>() { "Invalid Authentication Request" } // We don't want to give too much information on why the request has failed for security reasons
            });
        }

        var token = generateJwtToken(existingUser);
        return Ok(new UserLoginResponseDto()
        {
            Result = true,
            Token = token
        });
    }

    private string generateJwtToken(IdentityUser user)
    {
        var secret = Encoding.UTF8.GetBytes(_jwtConfigOptions.Secret);

        var jwtTokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtConfigOptions.Issuer,
            Expires = DateTime.Now.AddHours(1),
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            })
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = jwtTokenHandler.CreateToken(jwtTokenDescriptor);

        string stringToken = jwtTokenHandler.WriteToken(securityToken);

        return stringToken;
    }
}
