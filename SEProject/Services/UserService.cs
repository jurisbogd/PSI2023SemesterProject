using SEProject.Models;

namespace SEProject.Services
{
    public class UserService : IUserService
    {
        public User CreateNewUser(string username, string email, string password)
        {
            User newUser = new User(username, email, password);

            return newUser;
        }
    }
}
