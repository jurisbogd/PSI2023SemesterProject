using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SEProject.Models;
using SEProject.EventArguments;
using SEProject.EventServices;

using SEProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILoggingHandler, LoggingService>();

builder.Services.AddScoped<IFlashcardIOService, FlashcardIOService>();
builder.Services.AddScoped<IFlashcardPackDataHandler, FlashcardPackIOService>();

builder.Services.AddScoped<IFlashcardPackEventService, FlashcardPackEventService>();
builder.Services.AddScoped<IFlashcardEventService, FlashcardEventService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer("Server=tcp:flashcard-db.database.windows.net,1433;Initial Catalog=FlashcardDB;Persist Security Info=False;User ID=serveradmin;Password=Lapkritis123+;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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

