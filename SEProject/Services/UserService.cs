using SEProject.Models;
using SEProject.EventArguments;

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
        public virtual void OnUserChanged(UserEventArgs e)
        {
            if(UserChanged != null)
            {
                UserChanged(this, e);
            }
        }

        public User FindUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }
    }
}
