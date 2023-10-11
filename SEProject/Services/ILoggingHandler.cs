namespace SEProject.Services;

public interface ILoggingHandler
{
    void Log(LogEntry entry);
    List<LogEntry> GetLogs();
}