﻿using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.Services
{
    public interface IUserService
    {
        event UserService.UserChangedEventHandler? UserChanged;
        public User CreateNewUser(string username, string email, string password);
        public Task AddUserToTheDatabaseAsync(User user);
        void OnUserChanged(UserEventArgs e);
    }
}