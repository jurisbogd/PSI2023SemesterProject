using SEProject.EventArguments;

namespace SEProject.EventServices;

public interface IUserEventService
{
    void OnUserChanged(object source, UserEventArgs e);
}