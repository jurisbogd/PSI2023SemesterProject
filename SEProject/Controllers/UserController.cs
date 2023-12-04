using Microsoft.AspNetCore.Mvc;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult CreateAccount(string username, string email, string password) 
        {
            var newUser = _userService.CreateNewUser(username, email, password);
            _userService.AddUserToTheDatabase(newUser);

            return RedirectToAction("SignUp");
        }
    }
}
