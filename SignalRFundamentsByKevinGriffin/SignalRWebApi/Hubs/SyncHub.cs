using Microsoft.AspNetCore.SignalR;

namespace SignalRWebApi.Hubs;

public class SyncHub : Hub
{
    public async Task SyncTextBox(string textBox)
    {
        await Clients.Others.SendAsync("SyncTextBox", textBox);
    }
}
