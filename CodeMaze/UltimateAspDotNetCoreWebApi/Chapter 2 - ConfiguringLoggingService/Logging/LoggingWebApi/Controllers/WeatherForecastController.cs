using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LoggingWebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogManager _logManager;
    
    public WeatherForecastController(ILogManager logManager)
    {
        this._logManager = logManager;
    }
    
    [HttpGet]
    public string GetMessage()
    {
        // https://localhost:7209/api/v1/WeatherForecast/GetMessage
        
        _logManager.LogInfo("Hello World");
        _logManager.LogWarn("Hello World");
        _logManager.LogDebug("Hello World");
        _logManager.LogError("Hello World");
        return "Hello World";
    }
}