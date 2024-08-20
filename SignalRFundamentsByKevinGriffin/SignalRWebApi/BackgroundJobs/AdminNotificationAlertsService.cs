using Microsoft.AspNetCore.SignalR;
using SignalRWebApi.Hubs;

namespace SignalRWebApi.BackgroundJobs;


public class AdminNotificationAlertsService: BackgroundService
{
    private readonly IHubContext<SyncHub> _syncHubContext;
    public AdminNotificationAlertsService(IHubContext<SyncHub> syncHubContext)
    {
        _syncHubContext = syncHubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = new Random();
        while (!stoppingToken.IsCancellationRequested)
        {
             await Task.Delay(TimeSpan.FromSeconds(3));

             var notification = $"New Notification: {random.NextInt64()}";
             await _syncHubContext.Clients.Group("admin-notification-group").SendAsync("newAdminNotification", notification);

        }
    }
}
