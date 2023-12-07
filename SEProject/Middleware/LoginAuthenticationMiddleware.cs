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
                //https://localhost:7081/FlashcardPack/CreateSampleFlashcardPack


                if (context.Request.Path == "/FlashcardPack/CreateSampleFlashcardPack" && !context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/User/LogIn");
                }

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AuthenticationMiddleware: {ex}");
                throw;
            }
        }
    }
}
