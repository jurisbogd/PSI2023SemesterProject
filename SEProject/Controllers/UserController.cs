using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEProject.EventServices;
using SEProject.Models;
using SEProject.Services;

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

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(string username, string email, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var apiEndpoint = "http://localhost:5123/api/User/CreateUser";

                // Create a dictionary to hold your data
                var data = new Dictionary<string, string>
                {
                    { "username", username },
                    { "email", email },
                    { "password", password }
                };
                Console.WriteLine($"data: {data}");
                // Convert the data to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                Console.WriteLine($"jsonContent: {jsonContent}");
                // Send the POST request with the JSON data in the request body
                var response = await httpClient.PostAsync(apiEndpoint, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LogIn", "User");
                }

                var model = new LoginViewModel
                {
                    ErrorMessage = "Sorry the email is already in use."
                };

                return View("SignUp", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserLogIn(string email, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var apiEndpoint = "http://localhost:5123/api/User/UserLogIn";

                // Create a dictionary to hold your login data
                var data = new Dictionary<string, string>
                {
                    { "email", email },
                    { "password", password }
                };

                // Convert the data to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Send the POST request with the JSON data in the request body
                var response = await httpClient.PostAsync(apiEndpoint, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, response.Content.ReadAsStringAsync().Result)
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
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
