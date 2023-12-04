using Microsoft.AspNetCore.Mvc;

namespace SEProject.Controllers
{
    public class UserController : Controller
    {
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
            Console.WriteLine("username: " + username);
            Console.WriteLine("email: " + email);
            Console.WriteLine("password: " +  password);

            return RedirectToAction("SignUp");
        }
    }
}
