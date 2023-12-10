using Microsoft.AspNetCore.Mvc.Filters;

namespace DatabaseAPI.Interceptors
{
    public class LoggingInterceptor : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
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
