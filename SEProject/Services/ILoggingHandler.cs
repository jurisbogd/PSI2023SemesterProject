namespace SEProject.Models;

public interface ILoggingHandler
{
    void Log(LogEntry entry);
    List<LogEntry> GetLogs();
}