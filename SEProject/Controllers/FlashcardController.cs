using Microsoft.AspNetCore.Mvc;
using SEProject.Models;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IWebHostEnvironment _env;
    List<Flashcard> Testflashcards;

    public FlashcardController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult CreateSampleFlashcard()
    {
        // Json must be located in project root folder
        string jsonFilePath = System.IO.Path.Combine(_env.ContentRootPath, "flashcards.json");

        // Read the JSON file
        string jsonData = System.IO.File.ReadAllText(jsonFilePath);
        Testflashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonData);

        return View(Testflashcards);
    }
}