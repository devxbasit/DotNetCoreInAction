using ColorApi.Configuration;
using ColorApi.Data;
using ColorApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // https://www.youtube.com/watch?v=4V7CwC_4oss - Les Jackson
    // https://www.youtube.com/watch?v=ocMwNAt3-G0 - Kakashi Dota

    var dbConnectionOptions = builder.Configuration.GetSection("DbConnectionOptions")?.Get<DbConnectionOptions>() ?? throw new NullReferenceException();
    var server = dbConnectionOptions.Server;
    var port = dbConnectionOptions.Port;
    var userId = dbConnectionOptions.UserId;
    var password = dbConnectionOptions.Password;

    var connectionString = $"Server={server},{port}; Database=ColorApiDb; User Id={userId}; Password={password}; Trust Server Certificate=true";

    Console.WriteLine($"--> connectionString: {connectionString}");

    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.MapControllers();

await InitialDbCreate();
app.Run();

async Task InitialDbCreate()
{
    using var serviceScope = app.Services.CreateScope();
    await using var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    Console.WriteLine("--> Initial DB Create");
}
