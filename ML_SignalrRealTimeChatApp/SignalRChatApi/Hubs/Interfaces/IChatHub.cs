namespace SignalRChatApi.Hubs.Interfaces;

public interface IChatHub
{
    Task NewGroupNotification(string sender, string message);
    Task NewGroupMessage( string username,  string message);
}
