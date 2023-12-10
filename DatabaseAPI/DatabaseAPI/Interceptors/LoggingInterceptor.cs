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
            var modelKey = context.ActionArguments.Keys.FirstOrDefault(key => context.ActionArguments[key] is CreateUserModel);
            if (modelKey != null)
            {
                var model = context.ActionArguments[modelKey] as CreateUserModel;
                if (model != null)
                {
                    var existingUser = await _userService.FindUserByEmailAsync(model.email);
                    if (existingUser != null)
                    {
                        // Log a message and short-circuit the action
                        Console.WriteLine($"Email {model.email} already exists in the database. Action aborted.");

                        // Return a BadRequestObjectResult with a custom error message
                        context.Result = new BadRequestObjectResult("Sorry, the email is already in use");
                        return;
                    }
                }
            }

            // Log the start of the action
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} started at {DateTime.Now}");

            // Continue with the action execution
            var resultContext = await next();

            // Log the end of the action
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} completed at {DateTime.Now}");

            // If you want to log the result, you can access it using resultContext.Result
            // For example, you can log the status code:
            Console.WriteLine($"Status code: {resultContext.HttpContext.Response.StatusCode}");
        }
    }
}
