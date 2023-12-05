using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> UserLogIn(string email, string password)
        {
            _userService.UserChanged += _userEventService.OnUserChanged;
            var retrievedUser = await _userService.FindUserByEmailAsync(email);

            if (retrievedUser != null && _userService.VerifyPassword(password, retrievedUser.Salt, retrievedUser.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, retrievedUser.UserID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel
            {
                ErrorMessage = "Invalid email or password. Please try again."
            };

            return View("LogIn", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
