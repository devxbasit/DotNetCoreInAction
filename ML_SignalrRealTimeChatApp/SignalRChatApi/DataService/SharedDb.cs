using System.Collections.Concurrent;
using SignalRChatApi.Models;

namespace SignalRChatApi.DataService;

public class SharedDb
{

    private readonly ConcurrentDictionary<string, UserConnection> _connections = new ConcurrentDictionary<string, UserConnection>();

    public ConcurrentDictionary<string, UserConnection> Connections => _connections;

}
