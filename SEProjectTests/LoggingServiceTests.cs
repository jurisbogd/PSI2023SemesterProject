using Microsoft.Extensions.Logging;
using SEProject.Models;
using SEProject.Services;

namespace SEProjectTests;

public class LoggingServiceTests : IDisposable
{
    private string? _logFilePath;

    [Fact]
    public void Log_AddsEntryToLogEntries()
    {
        // Arrange
        var loggingService = new LoggingService();
        var logEntry = new LogEntry("Test Message");

        // Act
        loggingService.Log(logEntry);

        // Assert
        var logs = loggingService.GetLogs();
        Assert.Contains(logEntry, logs);
    }

    [Fact]
    public void Log_HandlesExceptionAndLogsErrorMessage()
    {
        // Arrange
        var loggingService = new LoggingService();
        var exception = new IOException("Simulated IO Exception");
        var logEntry = new LogEntry("Test Message", exception);

        // Act
        loggingService.Log(logEntry);

        // Assert
        var logs = loggingService.GetLogs();
        Assert.Contains(logEntry, logs);
    }

    [Fact]
    public void Log_HandlesLevelAndLogsMessage()
    {
        // Arrange
        var loggingService = new LoggingService();
        var logEntry = new LogEntry(message: "Test Message", level: LogLevel.Information);

        // Act
        loggingService.Log(logEntry);

        // Assert
        var logs = loggingService.GetLogs();
        Assert.Contains(logEntry, logs);
    }

    public void Dispose()
    {
        if (File.Exists(_logFilePath))
        {
            File.Delete(_logFilePath);
        }
    }
}
