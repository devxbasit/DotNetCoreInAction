using System.Net;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();


builder.Services.AddRateLimiter(options =>
{

    // fixed
    var fixedPolicy = options.AddFixedWindowLimiter("FixedPolicy1", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 1;
        options.QueueLimit = 2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    fixedPolicy.RejectionStatusCode = (int) HttpStatusCode.TooManyRequests;

    //
    // options.AddSlidingWindowLimiter("SlidingPolicy1", options =>
    // {
    //
    // });
    //
    //
    // options.AddConcurrencyLimiter("ConcurrentPolicy1", options =>
    // {
    //
    // });
    //
    // options.AddTokenBucketLimiter("TokenPolicy1", options =>
    // {
    //
    // });

});


var app = builder.Build();
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
