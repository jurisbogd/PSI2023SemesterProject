namespace SEProject.Services;

public interface ILoggingHandler
{
    public void Log(LogEntry entry);
    
    public void Log(string message, Exception? exception = null, LogLevel level = LogLevel.Information);

    List<LogEntry> GetLogs();
}