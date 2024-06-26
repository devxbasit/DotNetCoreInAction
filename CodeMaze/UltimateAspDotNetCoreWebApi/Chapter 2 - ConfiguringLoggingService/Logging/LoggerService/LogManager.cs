using Contracts;
using NLog;

namespace LoggerService;

public class LogManager : ILogManager
{
    private static readonly ILogger Logger = NLog.LogManager.GetCurrentClassLogger();
    
    public void LogInfo(string message) => Logger.Info(message);
    public void LogWarn(string message) => Logger.Warn(message);
    public void LogDebug(string message) => Logger.Debug(message);
    public void LogError(string message) => Logger.Error(message);
}