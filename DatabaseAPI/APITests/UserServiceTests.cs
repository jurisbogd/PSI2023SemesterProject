using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.EntityFrameworkCore;
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
    }
}
