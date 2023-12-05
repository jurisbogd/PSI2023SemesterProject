using SEProject.EventArguments;
using SEProject.Models;

namespace SEProject.EventServices;

public class UserEventService : IUserEventService
{
    private readonly ILoggingHandler _logger;

    public UserEventService(ILoggingHandler logger)
    {
        _logger = logger;
    }
    public void OnUserChanged(object source, UserEventArgs? e)
    {
        LogEntry logEntry;

        if (e == null)
        {
            logEntry = new LogEntry(message: "UserEventArgs object is null", level: LogLevel.Error);
        }
        else
        {
            logEntry = new LogEntry(
                message: $"User: ID - {e.User.UserID}, username - {e.User.Username} was updated. {e.Message}",
                level: LogLevel.Information);
        }

        _logger.Log(logEntry);
    }
}