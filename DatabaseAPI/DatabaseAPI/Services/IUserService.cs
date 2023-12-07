using DatabaseAPI.Models;

namespace DatabaseAPI.Services
{
    public interface IUserService
    {
        public User CreateNewUser(string username, string email, string password);
        public Task AddUserToTheDatabaseAsync(User user);
        public Task<User> FindUserByEmailAsync(string email);
        public bool VerifyPassword(string password, string salt, string hashedPassword);
    }
}
