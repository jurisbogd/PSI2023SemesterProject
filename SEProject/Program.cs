using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SEProject.Models;
using SEProject.EventArguments;
using SEProject.EventServices;

using SEProject.Services;
using SEProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILoggingHandler, LoggingService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Adjust the login path as needed
        options.LogoutPath = "/Account/Logout"; // Adjust the logout path as needed
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.UseMiddleware<LoginAuthenticationMiddleware>();

app.MapControllerRoute( // not sure if this route does anything
    name: "removeFlashcardRoute",
    pattern: "{controller=flashcard}/{action=RemoveSampleFlashcard}/{id}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

public partial class Program { }