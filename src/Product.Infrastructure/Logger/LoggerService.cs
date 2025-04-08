using Microsoft.Extensions.Logging;
using Product.Application.Logger;

namespace Product.Infrastructure.Logger;

public class LoggerService<T> : ILoggerService<T>
{
    private readonly ILogger<T> _logger;

    public LoggerService(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogError(string message, Exception ex = null)
    {
        _logger.LogError(ex, message);
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(message);
    }
}
