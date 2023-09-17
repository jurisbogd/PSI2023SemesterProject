using Microsoft.AspNetCore.Mvc;
using SEProject.Models;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IWebHostEnvironment _env;

    public FlashcardController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult CreateSampleFlashcard()
    {
        // Get the physical path to the JSON file using IWebHostEnvironment
        string jsonFilePath = System.IO.Path.Combine(_env.ContentRootPath, "flashcards.json");
        Console.WriteLine($"My Path: {jsonFilePath}");
        // Read the JSON file
        string jsonData = System.IO.File.ReadAllText(jsonFilePath);

        List<Flashcard> flashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonData);

        
        return View(flashcards);
    }
}