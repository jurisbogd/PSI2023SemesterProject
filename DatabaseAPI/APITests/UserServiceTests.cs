using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace APITests
{
    public class UserServiceTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;

        public UserServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            _options = optionsBuilder.Options;
        }

        [Fact]
        public void CreateNewUser_ShouldReturnUserWithCorrectValues()
        {
            // Arrange
            using (var context = new DatabaseContext(_options)) 
            {
                IUserService userService = new UserService(context);

                string username = "testUser";
                string email = "test@example.com";
                string password = "password123";

                // Act
                User newUser = userService.CreateNewUser(username, email, password);

                // Assert
                Assert.NotNull(newUser);
                Assert.Equal(username, newUser.Username);
                Assert.Equal(email, newUser.Email);
            }
        }

        [Fact]
        public async Task AddUserToTheDatabaseAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            using (var context = new DatabaseContext(_options))
            {
                IUserService userService = new UserService(context);

                string username = "testUser";
                string email = "test@example.com";
                string password = "password123";

                User newUser = userService.CreateNewUser(username, email, password);

                // Act
                await userService.AddUserToTheDatabaseAsync(newUser);

                // Assert
                var userFromDb = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

                Assert.NotNull(userFromDb);
                Assert.Equal(username, userFromDb.Username);
                Assert.Equal(email, userFromDb.Email);
            }
        }

        [Fact]
        public async Task FindUserByEmailAsync_ShouldReturnUserIfExists()
        {
            // Arrange
            using (var context = new DatabaseContext(_options))
            {
                IUserService userService = new UserService(context);

                string username = "testUser";
                string email = "test@example.com";
                string password = "password123";

                // Add a user to the database
                User newUser = userService.CreateNewUser(username, email, password);
                await userService.AddUserToTheDatabaseAsync(newUser);

                // Act
                var foundUser = await userService.FindUserByEmailAsync(email);

                // Assert
                Assert.NotNull(foundUser);
                Assert.Equal(username, foundUser.Username);
                Assert.Equal(email, foundUser.Email);
            }
        }

        [Fact]
        public async Task FindUserByEmailAsync_ShouldReturnNullForNonexistentUser()
        {
            // Arrange
            using (var context = new DatabaseContext(_options))
            {
                IUserService userService = new UserService(context);

                string nonExistentEmail = "nonexistent@example.com";

                // Act
                var foundUser = await userService.FindUserByEmailAsync(nonExistentEmail);

                // Assert
                Assert.Null(foundUser);
            }
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            using (var context = new DatabaseContext(_options))
            {
                IUserService userService = new UserService(context);

                string password = "password123";
                string salt = "somesalt";

                // Hash the password using the User class method
                string hashedPassword = new User().SetPassword(password);

                // Act
                bool result = userService.VerifyPassword(password, salt, hashedPassword);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
        {
            // Arrange
            using (var context = new DatabaseContext(_options))
            {
                IUserService userService = new UserService(context);

                string correctPassword = "password123";
                string incorrectPassword = "wrongpassword";
                string salt = "somesalt";

                // Hash the password using the User class method
                string hashedPassword = new User().SetPassword(correctPassword);

                // Act
                bool result = userService.VerifyPassword(incorrectPassword, salt, hashedPassword);

                // Assert
                Assert.False(result);
            }
        }
    }
}
