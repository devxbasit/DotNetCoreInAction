using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemDb"); });

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");


var app = builder.Build();
PrepDb.PrePopulation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

