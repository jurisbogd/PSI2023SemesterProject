using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SEProject.Models;

using SEProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILoggingHandler, LoggingService>();

builder.Services.AddScoped<IFlashcardPackDataHandler, FlashcardPackFileIOService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer("Server=LAPTOP-G14SEIVM\\SQLEXPRESS;Database=FlashcardDB;Trusted_Connection=True;TrustServerCertificate=True;");
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

app.MapControllerRoute( // not sure if this route does anything
    name: "removeFlashcardRoute",
    pattern: "{controller=flashcard}/{action=RemoveSampleFlashcard}/{id}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

