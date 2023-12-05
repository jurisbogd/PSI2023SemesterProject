using SEProject.Models;
using SEProject.EventArguments;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace SEProject.Services
{
    public class UserService : IUserService
    {
        private DatabaseContext _context;
        public delegate void UserChangedEventHandler(object source, UserEventArgs args);
        public event UserChangedEventHandler? UserChanged;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public User CreateNewUser(string username, string email, string password)
        {
            User newUser = new User(username, email, password);

            return newUser;
        }

        public async Task AddUserToTheDatabaseAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Console.WriteLine("User saved to the database successfully");
                OnUserChanged(new UserEventArgs(user, "Created"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating user: " + ex.Message);
            }
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            OnUserChanged(new UserEventArgs(user, "Found"));
            return user;
        }

        public bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = sha256.ComputeHash(combinedBytes);
                var enteredPasswordHash = Convert.ToBase64String(hashBytes);

                return hashedPassword == enteredPasswordHash;
            }
        }
        public virtual void OnUserChanged(UserEventArgs e)
        {
            if(UserChanged != null)
            {
                UserChanged(this, e);
            }
        }
    }
}
