using Microsoft.Extensions.Logging;

namespace SEProject.Services;

public record LogEntry {
    public DateTime TimeStamp { get; init; }
    public string Message { get; init; }
    public LogLevel Level { get; set; }

    public LogEntry(
        string message,
        Exception? exception = null,
        DateTime? timeStamp = null,
        LogLevel level = LogLevel.Information
    ) {
        Message = exception == null ? message : message + Environment.NewLine + exception;
        TimeStamp = timeStamp ?? DateTime.Now;
        Level = level;
    }
}