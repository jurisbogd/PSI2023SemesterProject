namespace SEProject.Services;

public class LoggingService : ILoggingHandler
{
    public struct UserParameters
    {
        public string OSName { get; set; }
        public string OSVersion { get; set; }

        public UserParameters(string osName, string osVersion)
        {
            OSName = osName;
            OSVersion = osVersion;
        }
    }

    private List<LogEntry> _logEntries = new();

    public void Log(LogEntry entry)
    {
        try
        {
            var os = Environment.OSVersion;
            var parameters = new UserParameters(os.Platform.ToString(), os.Version.ToString());
            var filePath = "log.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Timestamp: {entry.Timestamp}");
                writer.WriteLine($"Level: {entry.Level}");
                writer.WriteLine($"Message: {entry.Message}");
                writer.WriteLine($"OS Name: {parameters.OSName}, OS Version: {parameters.OSVersion}");
                writer.WriteLine();
            }
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