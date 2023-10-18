namespace SEProject.Services;

public record LogEntry {
    public DateTime TimeStamp { get; init; }
    public string Message { get; init; }
    public LogLevel Level { get; init; }

    public LogEntry(string message, LogLevel level = LogLevel.Information) {
        TimeStamp = DateTime.Now;
        Message = message;
        Level = level;
    }
}