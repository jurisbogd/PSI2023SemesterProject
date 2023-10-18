namespace SEProject.Services;

public record LogEntry(DateTime Timestamp, string Message, LogLevel Level);
