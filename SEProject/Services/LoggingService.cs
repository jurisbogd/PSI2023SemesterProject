using System;
using System.IO;

namespace SEProject.Services;

public class LoggingService : ILoggingHandler
{
    public struct UserParameters
    {
        public string osName;
        public string osVersion;
        public UserParameters(string _osName, string _osVersion)
        {
            osName = _osName;
            osVersion = _osVersion;
        }
    }
    private List<LogEntry> logEntries = new List<LogEntry>();

     public void Log(LogEntry entry)
    {
        try
        {
            OperatingSystem os = Environment.OSVersion;
            UserParameters parameters = new UserParameters(os.Platform.ToString(), os.Version.ToString());

            using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\log.txt", true))
            {
                writer.WriteLine($"Timestamp: {entry.Timestamp}");
                writer.WriteLine($"Level: {entry.Level}");
                writer.WriteLine($"Message: {entry.Message}");
                writer.WriteLine($"OS Name: {parameters.osName}, OS Version: {parameters.osVersion}");
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