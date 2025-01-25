using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices.Amqp;
using PlatformService.Configurations;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.Configure<RabbitMqConnectionOptions>(builder.Configuration.GetRequiredSection("RabbitMqConnectionOptions"));

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemDb"); });

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

var app = builder.Build();

PrepDb.PrePopulation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UsePathBase("/api");

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();

