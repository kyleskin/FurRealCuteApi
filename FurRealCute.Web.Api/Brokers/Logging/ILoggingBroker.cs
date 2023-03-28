namespace FurRealCute.Web.Api.Brokers.Logging;

public interface ILoggingBroker
{
    public void LogInformation(string message);
    public void LogTrace(string message);
    public void LogDebug(string message);
    public void LogWarning(string message);
    public void LogError(Exception exception);
    public void LogCritical(Exception exception);
}