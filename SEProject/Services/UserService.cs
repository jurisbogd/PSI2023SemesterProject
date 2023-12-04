using SEProject.Models;

namespace SEProject.Services
{
    public class UserService : IUserService
    {
        private DatabaseContext _context;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating user: " + ex.Message);
            }
        }
    }
}
