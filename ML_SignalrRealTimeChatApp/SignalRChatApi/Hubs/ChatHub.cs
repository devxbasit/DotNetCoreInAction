using Microsoft.AspNetCore.SignalR;
using SignalRChatApi.DataService;
using SignalRChatApi.Hubs.Interfaces;
using SignalRChatApi.Models;

namespace SignalRChatApi.Hubs;

public class ChatHub : Hub<IChatHub>
{
    private readonly SharedDb _sharedDb;

    public ChatHub(SharedDb sharedDb)
    {
        _sharedDb = sharedDb;
    }

    public async Task JoinSpecificChatRoom(UserConnection userConnection)
    {
        _sharedDb.Connections[Context.ConnectionId] = userConnection;
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);
        await Clients.Group(userConnection.ChatRoom).NewGroupNotification("Admin",$"{userConnection.Username} has joined.");
    }

    public async Task SendMessage(string message)
    {
        if (_sharedDb.Connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.ChatRoom).NewGroupMessage(userConnection.Username, message);
        }
    }

    public override async Task OnConnectedAsync()
    {
        Console.Write($"New client connected with ID: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.Write($"Client disconnected  with ID: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}
