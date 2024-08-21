using FoodOrdering.Context;
using FoodOrdering.Enums;
using FoodOrdering.Hubs.Interface;
using FoodOrdering.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Hubs;

public class FoodHub : Hub<IFoodHub>
{
    private readonly AppDbContext _dbContext;

    public FoodHub(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task OrderFoodItem(FoodRequest foodRequest)
    {
        _dbContext.Orders.Add(new Order()
        {
            OrderDate = DateTime.Now,
            FoodItemId = foodRequest.FoodItemId,
            TableNumber = foodRequest.TableNumber,
            OrderState = OrderState.Ordered
        });

        await _dbContext.SaveChangesAsync();

        await EmitActiveOrders();
    }

    public async Task UpdateFoodItemOrder(int orderId, OrderState orderState)
    {
        var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

        if (orderId != null)
        {
            order.OrderState = orderState;
        }

        await _dbContext.SaveChangesAsync();

        await EmitActiveOrders();
    }


    private async Task EmitActiveOrders()
    {
        var orders = await _dbContext.Orders.Include(x => x.FoodItem).Where(x => x.OrderState != OrderState.Completed).ToListAsync();
        Clients.All.PendingFoodUpdated(orders);
    }


    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"New client connected with ID: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Client disconnected with ID: {Context.ConnectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}
