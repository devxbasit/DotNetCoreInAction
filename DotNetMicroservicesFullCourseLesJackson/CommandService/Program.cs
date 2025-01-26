using CommandService.AsyncDataService;
using CommandService.Configurations;
using CommandService.Data;
using CommandService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.Configure<RabbitMqConnectionOptions>(builder.Configuration.GetRequiredSection("RabbitMqConnectionOptions"));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetRequiredSection("ConnectionStrings:CommandApiDbConnectionString").Value);
});

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<ICommandRepository, CommandRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapControllers();
app.Run();
