using SEProject.Models;

namespace SEProject.Middleware
{
    public class LoginAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                Console.WriteLine("AuthenticationMiddleware is being invoked.");

                //https://localhost:7081/FlashcardPack/CreateSampleFlashcardPack

                if (context.Request.Path == "/FlashcardPack/CreateSampleFlashcardPack" && !context.User.Identity.IsAuthenticated)
                {
                    // Redirect to the login page or return unauthorized response
                    context.Response.Redirect("/User/LogIn"); // Update the path to your login page
                    return;
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AuthenticationMiddleware: {ex}");
                throw; // Re-throw the exception to ensure it's not swallowed
            }
        }
    }
}
