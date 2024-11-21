using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApp.WebApi.Configuration;
using TodoApp.WebApi.Configuration.Options;
using TodoApp.WebApi.Data;
using TodoApp.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString,
            sqlServerOptionsAction: (options) =>
            {
                options
                    .EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
            })
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging();
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.Configure<JwtConfigOptions>(builder.Configuration.GetSection("JwtConfigOptions"));


var secret = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfigOptions:Secret").Value);

var tokenValidationParameters = new TokenValidationParameters()
    {
        // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings
        // and verify we have generated the jwt token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JwtConfigOptions:Issuer").Value,

        ValidateAudience = false,
        RequireExpirationTime = false,

        // This property(ValidateLifetime) specifies whether the lifetime of the token should be validated against the current time.
        // When set to true, the authentication middleware will check if the token has expired based on its exp claim.
        // If the current time exceeds this value, the token will be considered invalid.
        // If set to false, the lifetime of the token will not be validated, meaning that even expired tokens can be accepted as valid.
        ValidateLifetime = true, // Ensure tokens are checked for expiration

        // It is used to specify the amount of allowable clock skew when validating the expiration time (exp claim) of a token
        // It accounts for potential discrepancies between the server's clock and the client's clock
        // Strict Expiration Validation - ensure that no expired tokens are accepted under any circumstances
        ClockSkew = TimeSpan.Zero

    };

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.SaveToken = true;
    jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
