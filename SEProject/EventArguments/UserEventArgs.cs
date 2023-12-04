using SEProject.Models;

namespace SEProject.EventArguments;

public class UserEventArgs : EventArgs
{
    public User User { get; }
    public string Message { get; set; }

    public UserEventArgs(User user, string message = "")
    {
        User = user;
        Message = message;
    }
}