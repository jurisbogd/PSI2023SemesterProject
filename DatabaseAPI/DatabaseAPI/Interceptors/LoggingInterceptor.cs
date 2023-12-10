using DatabaseAPI.Models;
using DatabaseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatabaseAPI.Interceptors
{
    public class LoggingInterceptor : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public LoggingInterceptor(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the email already exists in the database
            var model = context.ActionArguments["model"] as CreateUserModel;
            if (model != null)
            {
                var existingUser = await _userService.FindUserByEmailAsync(model.email);
                if (existingUser != null)
                {
                    Console.WriteLine($"Email {model.email} already exists in the database. Action aborted.");
                    context.Result = new BadRequestObjectResult("Sorry, the email is already in use");
                    return;
                }
            }

            // Log the start of the action
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} started at {DateTime.Now}");
            var resultContext = await next();

            // Log the end of the action
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} completed at {DateTime.Now}");
            Console.WriteLine($"Status code: {resultContext.HttpContext.Response.StatusCode}");
        }
    }
}
