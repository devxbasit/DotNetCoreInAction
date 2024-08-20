using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebApi.Hubs;

public class SyncHub : Hub
{
    public async Task SyncTextBox(string textBox)
    {
        await Clients.Others.SendAsync("SyncTextBox", textBox);
    }

    public async Task StartNotifyAdminNotification()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "admin-notification-group");
    }

    public async Task StopNotifyAdminNotification()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin-notification-group");
    }
}
