using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.WebApi.Configuration.Options;
using TodoApp.WebApi.Data;
using TodoApp.WebApi.Domain;
using TodoApp.WebApi.Dto.Requests;
using TodoApp.WebApi.Models;

namespace TodoApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthManagementController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfigOptions _jwtConfigOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly AppDbContext _appDbContext;

    public AuthManagementController(
        UserManager<IdentityUser> userManager,
        IOptionsMonitor<JwtConfigOptions> jwtConfigOptions,
        TokenValidationParameters tokenValidationParameters,
        AppDbContext appDbContext)
    {
        _userManager = userManager;
        _jwtConfigOptions = jwtConfigOptions.CurrentValue;
        _tokenValidationParameters = tokenValidationParameters;
        _appDbContext = appDbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequestDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
                Errors = new List<string>() { "Invalid Payload" }
            });
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser is not null)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
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
            return new JsonResult(new AuthResult()
            {
                IsSuccess = false,
                Errors = identityResult.Errors.Select(x => x.Description).ToList()
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        // everything is okay, proceed to generate token
        var authResult = await generateJwtToken(newIdentityUser);

        if (authResult is null)
        {
            return new JsonResult(new AuthResult()
            {
                IsSuccess = false,
                Errors = ["Internal Server Error!"]
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        return Ok(authResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
                Errors = new List<string>() { "Invalid Payload" }
            });
        }

        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if (existingUser is null)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
                Errors = new List<string>() { "Invalid Authentication Request" } // We don't want to give too much information on why the request has failed for security reasons
            });
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
        if (!isPasswordCorrect)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
                Errors = new List<string>() { "Invalid Authentication Request" } // We don't want to give too much information on why the request has failed for security reasons
            });
        }

        var authResult = await generateJwtToken(existingUser);

        if (authResult is null)
        {
            return new JsonResult(new AuthResult()
            {
                IsSuccess = false,
                Errors = ["Internal Server Error!"]
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        return Ok(authResult);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResult()
            {
                IsSuccess = false,
                Errors = ["Invalid payload"]
            });
        }

        var authResult = await VerifyRefreshToken(refreshTokenRequestDto);

        if (authResult is null)
        {
            return new JsonResult(new AuthResult()
            {
                IsSuccess = false,
                Errors = ["Internal Server Error!"]
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        return Ok(authResult);
    }

    private async Task<AuthResult?> generateJwtToken(IdentityUser user)
    {
        try
        {
            var secret = Encoding.UTF8.GetBytes(_jwtConfigOptions.Secret);

            var jwtTokenDescriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfigOptions.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfigOptions.ExpiryTimeFrameInMinutes),

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                })
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(jwtTokenDescriptor);
            string jwtToken = jwtSecurityTokenHandler.WriteToken(securityToken);

            var refreshToken = new RefreshToken()
            {
                JwtId = securityToken.Id,
                Token = $"{RandomString(25)}-{Guid.NewGuid()}",
                UserId = user.Id,
                IsUsed = false,
                IsRevoked = false,
                AddedDateTime = DateTime.UtcNow,
                ExpiryDateTime = DateTime.UtcNow.AddYears(1)
            };

            await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                IsSuccess = true
            };
        }
        catch
        {
            // log error
            return null;
        }
    }

    public string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private async Task<AuthResult?> VerifyRefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        return null;
    }
}
