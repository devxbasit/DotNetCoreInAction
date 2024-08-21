using System.Text.Json.Serialization;
using FoodOrdering.Context;
using FoodOrdering.Hubs;
using FoodOrdering.Hubs.Interface;
using FoodOrdering.Worker;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPort4200", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddHostedService<SeedingWorker>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=mydatabase.sqlite");
});

builder.Services.AddMvc().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowPort4200");

app.MapControllers();
app.MapHub<FoodHub>("/hub/food");

app.Run();


