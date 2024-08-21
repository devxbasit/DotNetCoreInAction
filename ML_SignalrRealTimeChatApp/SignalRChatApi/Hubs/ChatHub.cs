using Microsoft.AspNetCore.SignalR;
using SignalRChatApi.DataService;
using SignalRChatApi.Models;

namespace SignalRChatApi.Hubs;

public class ChatHub : Hub
{
    private readonly SharedDb _sharedDb;

    public ChatHub(SharedDb sharedDb)
    {
        _sharedDb = sharedDb;
    }

    public async Task JoinSpecificChatRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);
        _sharedDb.Connections[Context.ConnectionId] = userConnection;
        await Clients.Group(userConnection.ChatRoom).SendAsync("JoinSpecificChatRoom", "admin", $"{userConnection.Username} has joined.");
    }

    public async Task SendMessage(string message)
    {
        if (_sharedDb.Connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.ChatRoom).SendAsync("ReceiveMessage", userConnection.Username, message);
        }
    }
}
