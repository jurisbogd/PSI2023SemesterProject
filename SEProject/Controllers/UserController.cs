﻿using Microsoft.AspNetCore.Mvc;
using SEProject.EventServices;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILoggingHandler _logger;
        private readonly IUserEventService _userEventService;

        public UserController(IUserService userService, ILoggingHandler logger, IUserEventService userEventService)
        {
            _userService = userService;
            _logger = logger;
            _userEventService = userEventService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public async Task<IActionResult> CreateAccount(string username, string email, string password) 
        {
            _userService.UserChanged += _userEventService.OnUserChanged;
            var newUser = _userService.CreateNewUser(username, email, password);
            await _userService.AddUserToTheDatabaseAsync(newUser);

            return RedirectToAction("SignUp");
        }

        public IActionResult UserLogIn(string email, string password)
        {
            Console.WriteLine("Email: " + email);
            Console.WriteLine("Not Hashed Password: " + password);

            var retrievedUser = _userService.FindUserByEmail(email);

            if (retrievedUser != null)
            {
                retrievedUser.ToString();
            }
            else
            {
                var model = new LoginViewModel
                {
                    EmailNotFoundErrorMessage = "No user with the given email could be found. Please check the email and try again."
                };

                return View("LogIn", model);
            }

           
            return RedirectToAction("LogIn");
        }
    }
}
