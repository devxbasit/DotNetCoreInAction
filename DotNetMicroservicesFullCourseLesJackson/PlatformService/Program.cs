using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.AsyncDataServices.Amqp;
using PlatformService.Configurations;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.Configure<RabbitMqConnectionOptions>(builder.Configuration.GetRequiredSection("RabbitMqConnectionOptions"));

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetRequiredSection("ConnectionStrings:PlatformApiDbConnectionString").Value);
    options.UseInMemoryDatabase("InMemDb");
});

builder.Services.AddGrpc();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

var app = builder.Build();

PrepDb.PrePopulation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

//app.UsePathBase("/api");

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();

app.Run();

