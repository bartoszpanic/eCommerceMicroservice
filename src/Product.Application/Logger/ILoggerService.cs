namespace Product.Application.Logger;

public interface ILoggerService<T>
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception ex = null);
    void LogDebug(string message);
}
