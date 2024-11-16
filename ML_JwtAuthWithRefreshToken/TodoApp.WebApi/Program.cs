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
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secret = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfigOptions:Secret").Value);

    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings
        // and verify we have generated the jwt token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JwtConfigOptions:Issuer").Value,

        ValidateAudience = false,

        RequireExpirationTime = false,

        ValidateLifetime = true,
    };
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
