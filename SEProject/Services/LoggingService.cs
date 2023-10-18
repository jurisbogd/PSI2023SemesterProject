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
    private List<LogEntry> logEntries = new List<LogEntry>();

     public void Log(LogEntry entry)
    {
        try
        {
            OperatingSystem os = Environment.OSVersion;
            UserParameters parameters = new UserParameters(os.Platform.ToString(), os.Version.ToString());
            string filePath = "log.txt";
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
        return logEntries;
    }
}