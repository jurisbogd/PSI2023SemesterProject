using SEProject.Models;

namespace SEProject.Services
{
    public interface IUserService
    {
        public User CreateNewUser(string username, string email, string password);
        public void AddUserToTheDatabase(User user);
    }
}
