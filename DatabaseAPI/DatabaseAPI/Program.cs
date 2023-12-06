using DatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFlashcardIOService, FlashcardIOService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer("Server=tcp:flashcard-db.database.windows.net,1433;Initial Catalog=FlashcardDB;Persist Security Info=False;User ID=serveradmin;Password=Lapkritis123+;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();