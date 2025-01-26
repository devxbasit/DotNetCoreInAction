using AmigoPizzaWebApi.Context;
using AmigoPizzaWebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// user-secrets setup 
// dotnet user-secrets init
// dotnet user-secrets set "ConnectionStrings:AmigoPizzaConnectionString" "Data Source = localhost,1433; Initial Catalog = AmigoPizza; Integrated Security = false; User Id = sa; Password = strongPA55WORD!; TrustServerCertificate = true"

builder.Services.AddDbContext<AmigoPizzaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AmigoPizzaConnectionString"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

