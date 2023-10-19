using SEProject.Models;

namespace SEProject.Services;

public class LoggingService : ILoggingHandler
{
    private const string _logFilePath = "log.txt";
    private readonly List<LogEntry> _logEntries = new();

    public void Log(LogEntry entry)
    {
        _logEntries.Add(entry);

        try
        {
            var os = Environment.OSVersion;
            var parameters = new UserParameters(os.Platform.ToString(), os.Version.ToString());

            using var writer = new StreamWriter(_logFilePath, true);
            writer.WriteLine(entry.TimeStamp);
            writer.WriteLine(entry.Level);
            writer.WriteLine(entry.Message);
            writer.WriteLine($"OS: {parameters.OSName}, Version: {parameters.OSVersion}");
            writer.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while logging: {ex.Message}");
        }
    }

    public List<LogEntry> GetLogs()
    {
        return _logEntries;
    }
}